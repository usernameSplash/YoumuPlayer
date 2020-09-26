using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Animator ani;
    SpriteRenderer _sprite;
    float speed = 3.0f;

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
        ani = gameObject.GetComponent<Animator>();
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.J))
        {
            ani.Play("Youmu_Pierce");
        }
        else if (Input.GetKey(KeyCode.K))
        {
            ani.Play("Youmu_Slash");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            ani.Play("Youmu_Sit");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ani.Play("Youmu_Walk");
            Dir = MoveDir.Right;
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            ani.Play("Youmu_Walk");
            Dir = MoveDir.Left;
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            ani.Play("Youmu_Stand");
        }
        else
        {
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("Youmu_Stand"))
            {
                if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f)
                {
                    ani.Play("Youmu_Idle");
                }
            }
            else
            {
                ani.Play("Youmu_Slash");
            }
        }

    }

}
