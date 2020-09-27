using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    This component is not used in practice.
    커맨드 큐 시스템(like 격투게임 콤보) 구현을 시도했었다.
    큐에 커맨드 저장까지는 했는데 Invoke, 상태관리에 어려움을 느낌
*/
public class YoumuInput : MonoBehaviour
{
    Coroutine _coroutineMove;
    Coroutine _coroutineCommand;

    String _lastMoveKey;

    List<String> _moveList = new List<String>();
    List<String> _commandList = new List<String>();

    public Action<List<KeyCode>> KeyAction = null;


    void Update()
    {
        GetMoveInput();
        GetCommandInput();

        Debug.Log(_moveList.Count);

    }

    void GetMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _coroutineMove = StartCoroutine(AddCommand(_moveList, "A"));
            _lastMoveKey = "A";
        }


        if (Input.GetKeyDown(KeyCode.D))
        {
            _coroutineMove = StartCoroutine(AddCommand(_moveList, "D"));
            _lastMoveKey = "D";
        }

    }

    void GetCommandInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(AddCommand(_commandList, "J"));
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(AddCommand(_commandList, "K"));
        }
    }

    //기본적으로, 어떤 key의 입력을 받아 리스트(큐)에 넣고 1초 후에 커맨드 큐에서 삭제되도록 함
    IEnumerator AddCommand(List<String> list, String key, float time = 1.0f)
    {
        list.Add(key);

        yield return new WaitForSeconds(time);

        list.Remove(key);
    }
}
