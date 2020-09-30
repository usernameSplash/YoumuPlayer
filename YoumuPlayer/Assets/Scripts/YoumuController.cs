using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YoumuController : MonoBehaviour
{
    Animator _ani;
    SpriteRenderer _sprite;
    Rigidbody2D _rigid;
    Vector3 _scale;
    Vector3 _scaleFlippedX;
    Dictionary<KeyCode, Action> _inputHandler;

    public float _speed;
    float _jumpPower;

    public bool _isFocusing; // 정신집중
    public bool _isJumping;

    Vector3 _dirVec;    //이동 방향
    Vector3 DirVec
    {
        get { return _dirVec; }
        set
        {
            if (_isFocusing) return;

            _dirVec = value;
        }
    }

    public enum LookDir                        //바라보고 있는 방향
    {
        None,
        Left,
        Right
    }

    LookDir _dir;
    public LookDir Dir
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
                case LookDir.Left:
                    // _sprite.flipX = true;
                    transform.localScale = _scaleFlippedX;
                    DirVec = Vector3.left;
                    break;
                case LookDir.Right:
                    // _sprite.flipX = false;
                    transform.localScale = _scale;
                    DirVec = Vector3.right;
                    break;
            }
        }
    }

    void Awake()
    {
        _ani = GetComponent<Animator>();
        _ani.speed = 1.2f;

        _sprite = GetComponent<SpriteRenderer>();
        _rigid = GetComponent<Rigidbody2D>();

        _scale = new Vector3(3.0f, 3.0f, 3.0f);
        _scaleFlippedX = new Vector3(-3.0f, 3.0f, 3.0f);

        _inputHandler = new Dictionary<KeyCode, Action>();

        _speed = 0.0f;
        _jumpPower = 0.0f;

        _isFocusing = false;
        _isJumping = false;

        DirVec = Vector3.zero;
        Dir = LookDir.Right;
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
            if (!_ani.GetBool("isDash"))
            {
                _speed = 3.0f;
            }
            _ani.SetBool("isWalk", true);
            Dir = LookDir.Right;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (!_ani.GetBool("isDash"))
            {
                _speed = 3.0f;
            }
            _ani.SetBool("isWalk", true);
            Dir = LookDir.Left;
        }
        else
        {
            _ani.SetBool("isWalk", false);
            _ani.SetBool("isDash", false);
            _speed = 0.0f;
        }
    }

    void GetAttackInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _ani.SetBool("isSlash", true);
        }
        else
        {
            _ani.SetBool("isSlash", false);
        }
    }

    void Move()
    {
        if (_speed <= 0.01f)
        {
            return;
        }

        transform.position += DirVec * _speed * Time.deltaTime;
    }

    void Jump()
    {
        if (!_isJumping && _jumpPower > 0.0f)
        {
            _ani.SetBool("isJump", true);
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    void IsOnAir()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Floor");

        // Debug.DrawRay(transform.position + new Vector3(0.0f, 1.0f), Vector3.down * 2.0f, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(transform.position + new Vector3(0.0f, 1.0f), Vector3.down, 2.0f, layerMask);
        // Debug.Log(rayHit.distance);

        if (rayHit.collider != null)
        {
            if (rayHit.distance < 1.0f)
            {
                _ani.SetBool("isJump", false);
                _isJumping = false;
            }
            else
            {
                _ani.SetBool("isJump", true);
                _isJumping = true;
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

    //대시 공격에서 실제로 돌진하는 부분
    void DashAttackExecute()
    {
        _rigid.AddForce(DirVec * 24.0f, ForceMode2D.Impulse);
        StartCoroutine(DashAttackStop());
    }

    IEnumerator DashAttackStop()
    {
        while (true)
        {
            if (Mathf.Abs(_rigid.velocity.x) <= 0.1f)
            {
                break;
            }
            yield return null;
        }
        _rigid.velocity = new Vector2(0.0f, _rigid.velocity.y);
        _ani.SetTrigger("DashAttackStop");
        yield break;
    }


    void LateUpdate()
    {
        //카메라
    }



}
