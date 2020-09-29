using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct Command
{
    public readonly static ulong Arrow_Down = 0x1;
    public readonly static ulong Arrow_Up = 0x100;
    public readonly static ulong Arrow_Left = 0x10000;
    public readonly static ulong Arrow_Right = 0x1000000;
    public readonly static ulong Attack_J = 0x100000000;
    public readonly static ulong Attack_K = 0x10000000000;
    public readonly static ulong Attack_U = 0x1000000000000;
    public readonly static ulong Attack_I = 0x100000000000000;
    static public Dictionary<string, ulong> CommandKeyList = new Dictionary<string, ulong>();
}

public class CommandController : MonoBehaviour
{
    ulong _curCommand;
    int _inputCounter; // Times that Command keys entered.
    Dictionary<ulong, Action> _commandList;  // Command and Callback Funcs.

    void CommandAdder(ulong[] commands, Action action)
    {
        int count = 0;
        ulong tempCommand = 0;
        foreach (var command in commands)
        {
            tempCommand |= command << count;
            count++;
        }
        _commandList.Add(tempCommand, action);
    }

    void Awake()
    {
        _curCommand = 0;
        _inputCounter = 0;
        _commandList = new Dictionary<ulong, Action>();

        Command.CommandKeyList.Add("Arrow_Down", Command.Arrow_Down);
        Command.CommandKeyList.Add("Arrow_Up", Command.Arrow_Up);
        Command.CommandKeyList.Add("Arrow_Left", Command.Arrow_Left);
        Command.CommandKeyList.Add("Arrow_Right", Command.Arrow_Right);
        Command.CommandKeyList.Add("Attack_J", Command.Attack_J);
        Command.CommandKeyList.Add("Attack_K", Command.Attack_K);
        Command.CommandKeyList.Add("Attack_U", Command.Attack_U);
        Command.CommandKeyList.Add("Attack_I", Command.Attack_I);

        CommandAdder(new ulong[] { Command.Arrow_Left, Command.Arrow_Left }, LeftDash);
        CommandAdder(new ulong[] { Command.Arrow_Right, Command.Arrow_Right }, RightDash);
        CommandAdder(new ulong[] { Command.Attack_J, Command.Attack_J }, DoubleAttack);
        CommandAdder(new ulong[] { Command.Arrow_Left, Command.Arrow_Down, Command.Arrow_Right, Command.Attack_J }, LeftDashAttack);
        CommandAdder(new ulong[] { Command.Arrow_Right, Command.Arrow_Down, Command.Arrow_Left, Command.Attack_J }, RightDashAttack);
    }

    void Update()
    {
        // Update _inputCounter when any key in CommandKeyList is down.
        foreach (var pair in Command.CommandKeyList)
        {
            if (Input.GetButtonDown(pair.Key))
            {
                _curCommand |= pair.Value << _inputCounter;

                StartCoroutine(UpdateCommand(pair.Value, _inputCounter));
                _inputCounter++;

            }
        }

        // Invoke callback function when _curCommand is one of commands, 
        Action callback;
        if (_commandList.TryGetValue(_curCommand, out callback))
        {
            //커맨드 삭제 명령을 멈춘다.
            StopAllCoroutines();
            callback.Invoke();
            _curCommand = 0;
            _inputCounter = 0;
        }
    }

    /*
        After a certain period of seconds since input commandKey,
        Update _curCommand, _inputCount.

        커맨드키 입력 후 일정 시간(0.5초) 후 _curCommand와 _inputCounter를 감소시켜
        커맨드 완성이 무산된 입력을 삭제한다.
    */
    IEnumerator UpdateCommand(ulong key, int count)
    {
        yield return new WaitForSeconds(0.4f);
        _inputCounter--;
        _curCommand = _curCommand & (~(key << count));
    }

    //Callback Functions
    void LeftDash()
    {
        Debug.Log("LeftDash");
    }

    void RightDash()
    {
        Debug.Log("RightDash");
    }

    void DoubleAttack()
    {
        Debug.Log("DoubleAttack");
    }

    void LeftDashAttack()
    {
        Debug.Log("LeftDashAttack");
    }
    void RightDashAttack()
    {
        Debug.Log("RightDashAttack");
    }
}
