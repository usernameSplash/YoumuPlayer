using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    Animator _ani;
    SpriteRenderer _sprite;
    Rigidbody2D _rigid;
    Vector3 _Scale = new Vector3(3.0f, 3.0f, 3.0f);
    Vector3 _ScaleFlippedX = new Vector3(-3.0f, 3.0f, 3.0f);

    float _speed = 3.0f;
    float _maxSpeed = 7.0f;
    float _jumpPower = 0.0f;

    public bool _isFocusing = false; // 정신집중
    public bool _isJumping = false;

    enum MoveDir
    {
        None,
        Left,
        Right
    }
    Vector3 _dirVec = Vector3.zero;
    Vector3 DirVec
    {
        get { return _dirVec; }
        set
        {
            if (_isFocusing) return;

            _dirVec = value;
        }
    }

    MoveDir _dir = MoveDir.Right;
    MoveDir Dir
    {
        get { return _dir; }
        set
        {
            if (_isFocusing) return;
            if (_dir == value) return;

            _dir = value;

            switch (_dir)
            {
                //스프라이트 뿐만 아니라 component, child들까지 모두 반전시켜야 하기 때문에 scale을 조정하는 방향으로 수정
                case MoveDir.Left:
                    // _sprite.flipX = true;
                    transform.localScale = _ScaleFlippedX;
                    break;
                case MoveDir.Right:
                    // _sprite.flipX = false;
                    transform.localScale = _Scale;
                    break;
            }
        }
    }

    void Start()
    {
        _ani = GetComponent<Animator>();
        _ani.speed = 1.2f;
        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {
        GetMovingInput();
        GetAttackInput();
        IsOnAir();

        //예) 공격 애니메이션 도중 이동, 점프를 못하게 함
        if (!_isFocusing)
        {
            Move();
            Jump();
        }
    }

    void GetMovingInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            _jumpPower = 16.0f;
        }
        else
        {
            _jumpPower = 0.0f;
        }

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
            _dirVec = Vector3.right;
            Dir = MoveDir.Right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            _dirVec = Vector3.left;
            Dir = MoveDir.Left;
        }
        else
        {
            _dirVec = Vector3.zero;
        }
    }

    void GetAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (_ani.GetCurrentAnimatorStateInfo(0).IsName("Youmu_Slash"))
            {
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

    void Move()
    {
        if (_dirVec.magnitude <= 0.01f)
        {
            _ani.SetBool("isWalk", false);
            return;
        }

        _ani.SetBool("isWalk", true);

        transform.position += _dirVec * _speed * Time.deltaTime;
    }

    void Jump()
    {
        if (!_isJumping)
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
    }

    void IsOnAir()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Floor");

        // Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f), Vector3.down * 2.0f, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position + new Vector3(0.0f, 1.0f), Vector3.down, 2.0f, layerMask);
        // Debug.Log(rayHit.distance);

        if (rayHit.collider != null)
        {
            if (rayHit.distance < 1.2f)
            {
                _ani.SetBool("isJump", false);
                _isJumping = false;
            }
        }
        else
        {
            if (Mathf.Abs(_rigid.velocity.y) > 0.1f)
            {
                _ani.SetBool("isJump", true);
                _isJumping = true;
            }
        }
    }

    //정신 집중 여부를 갱신하는 함수, AnimationEvent로 사용중
    public void OnFocus()
    {
        _isFocusing = true;
    }
    public void LeaveFocus()
    {
        _isFocusing = false;
    }


    void LateUpdate()
    {
        //카메라
    }



}
