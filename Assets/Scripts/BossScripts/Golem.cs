using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Golem : MonoBehaviour
{
    public GameObject hand; // 로켓펀치 손 프리팹

    // EmptyObject꺼
    private Rigidbody2D _rigid;
    private GameObject _rollSprite; // 롤링 스프라이트
    private GameObject _crashBound; // 바닥찍기 어택바운드
    // GFX꺼
    private Animator _animator;
    private GameObject _GFX;    // 애니메이션 있는 그래픽 오브젝트

    private float movePower = 1f;
    private float rollMovePower = 2f;
    private bool attackFlag = false;    // t: 공격중, f: 아님
    private bool rollinFlag = false;    // t: 롤링상태, Move함수에서 애니메이션 제어욤

    // Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _rollSprite = transform.GetChild(1).gameObject;
        _crashBound = transform.GetChild(2).gameObject;

        _GFX = transform.GetChild(0).gameObject;
        _animator = _GFX.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        // 공격중이면 움직임 x
        if(attackFlag) {
            return;
        }

        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw("Horizontal") < 0) {
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(-1, 1, 1);

            if(!rollinFlag) {   // 평상시
                _animator.Play("golem_walk_Full");
            }
            else {  // 롤상태
                _rollSprite.transform.Rotate(new Vector3(0, 0, -3));
            }
        }
        else if(Input.GetAxisRaw("Horizontal") > 0) {
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(1, 1, 1);

            if(!rollinFlag) {   // 평상시
                _animator.Play("golem_walk_Full");
            }
            else {  // 롤상태
                _rollSprite.transform.Rotate(new Vector3(0, 0, -3));
            }
        }
        else if(Input.GetAxisRaw("Horizontal") == 0) {
            if(!rollinFlag) {   // 평상시
                _animator.Play("golem_idle");
            }
        }

        if(!rollinFlag) {   // 평상시
            transform.position += moveVelocity * movePower * Time.deltaTime;
        }
        else {  // 롤상태
            transform.position += moveVelocity * rollMovePower * Time.deltaTime;
        }
    }

    // 공격스킬
    public void HandCrash()
    {
        if(attackFlag) {
            return;
        }
        attackFlag = true;

        _animator.Play("golem_attack");
    }

    public void RocketPunch()
    {
        if(attackFlag) {
            return;
        }
        attackFlag = true;

        _animator.Play("golem_punch");
    }

    public void RollingThunder()
    {
        if(attackFlag) {
            return;
        }
        attackFlag = true;

        _animator.Play("golem_circle_on");
    }

    void Rolling_End()
    {
        _GFX.SetActive(true);
        _rollSprite.SetActive(false);
        _rollSprite.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        _animator.Play("golem_circle_off");
    }

    // 롤 상태에 처리(애니메이션 이벤트)
    public void RollingMode()
    {
        rollinFlag = true;
        attackFlag = false;

        _animator.Play("golem_circle");
        _rollSprite.SetActive(true);
        _GFX.SetActive(false);

        Invoke("Rolling_End", 3f);
    }

    // 로켓펀치 손 생성 애니메이션 이벤트
    public void ShootRocketPunch()
    {
        Instantiate(hand);
    }

    // 바닥찍기 바운드생성
    public void CrashBoundAtive()
    {
        _crashBound.SetActive(true);
    }

    // 롤 끝나면 플래그 변경 위함(애니메이션 이벤트)
    public void EndRollindEndMotion()
    {
        rollinFlag = false;
    }

    // 공격끝나면 플래그 변경위함(애니메이션 이벤트)
    public void EndAttackMotion()
    {
        attackFlag = false;
        _crashBound.SetActive(false);
    }
    
}
