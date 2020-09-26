using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Animator _ani;
    SpriteRenderer _sprite;
    float _speed = 3.0f;
    bool _isGround = true;

    enum MoveDir
    {
        None,
        Left,
        Right
    }
    MoveDir _dir = MoveDir.Right;
    MoveDir Dir
    {
        get { return _dir; }
        set
        {
            _dir = value;

            switch (_dir)
            {
                case MoveDir.Left:
                    _sprite.flipX = true;
                    break;
                case MoveDir.Right:
                    _sprite.flipX = false;
                    break;
            }
        }
    }

    void Start()
    {
        _ani = gameObject.GetComponent<Animator>();
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            _ani.SetBool("isSit", true);
        }
        else
        {
            _ani.SetBool("isSit", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            _ani.SetBool("isWalk", true);
            Dir = MoveDir.Right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _ani.SetBool("isWalk", true);
            Dir = MoveDir.Left;
        }
        else
        {
            _ani.SetBool("isWalk", false);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            if (_ani.GetCurrentAnimatorStateInfo(0).IsName("Youmu_Slash"))
            {
                Debug.Log("A");
                _ani.SetTrigger("Pierce");
            }
            else
            {
                _ani.SetBool("isSlash", true);
            }
        }
        else
        {
            _ani.SetBool("isSlash", false);
        }

    }
    void LateUpdate()
    {
        //카메라
    }

}
