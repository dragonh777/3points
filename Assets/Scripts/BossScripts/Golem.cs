using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Golem : MonoBehaviour
{
    public GameObject hand; // 로켓펀치 손 프리팹
    public Image HPbar;
    public Image SPBar;

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
    private bool hitFlag = false;   // t: 맞고있을 때(잠시무적)

    public static float HP = 150f;
    public static float SP = 120f;
    public float currentHP;
    public float currentSP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
        currentSP = SP;

        _rigid = GetComponent<Rigidbody2D>();
        _rollSprite = transform.GetChild(1).gameObject;
        _crashBound = transform.GetChild(2).gameObject;

        _GFX = transform.GetChild(0).gameObject;
        _animator = _GFX.GetComponent<Animator>();

        ReGen();
    }

    // Update is called once per frame
    void Update()
    {
        HPbar.fillAmount = currentHP / HP;
        SPBar.fillAmount = currentSP / SP;

        // 맞고있을때, 롤링중에는 체력 안닳음, 
        if(Input.GetKeyDown(KeyCode.X) && !hitFlag && !rollinFlag) {
            hitFlag = true;
            currentHP -= 10f;
            
            if(attackFlag) {   // 공격모션중엔 체력만 닳고 모션취소 x
                hitFlag = false;
            }
            else if(!attackFlag) {  // 공격안하고있을 때
                Hit();
            }
            else {
                hitFlag = false;
            }
        }

        // 죽을 때
        if(currentHP <= 0) {
            hitFlag = true;
            Die();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    // 체력, 마나 리젠, 임시용
    void ReGen() 
    {
        if(currentHP != HP) {
            currentHP += 10;
        }
        if(currentSP != SP) {
            currentSP += 20;
        }

        Invoke("ReGen", 5f);
    }

    void Move()
    {
        // 공격중이면 움직임 x
        if(attackFlag || hitFlag) {
            return;
        }

        Vector3 moveVelocity = Vector3.zero;

        if(Input.GetAxisRaw("Horizontal") < 0) {
            moveVelocity = Vector3.left;
            // transform.localScale = new Vector3(-1, 1, 1);

            if(!rollinFlag) {   // 평상시
                transform.localScale = new Vector3(-1, 1, 1);
                _animator.Play("golem_walk_Full");
            }
            else {  // 롤상태
                if(transform.localScale.x > 0) {
                    _rollSprite.transform.Rotate(new Vector3(0, 0, 3));
                }
                else {
                    _rollSprite.transform.Rotate(new Vector3(0, 0, -3));
                }
            }
        }
        else if(Input.GetAxisRaw("Horizontal") > 0) {
            moveVelocity = Vector3.right;
            //transform.localScale = new Vector3(1, 1, 1);

            if(!rollinFlag) {   // 평상시
                transform.localScale = new Vector3(1, 1, 1);
                _animator.Play("golem_walk_Full");
            }
            else {  // 롤상태
                if(transform.localScale.x > 0) {
                    _rollSprite.transform.Rotate(new Vector3(0, 0, -3));
                }
                else {
                    _rollSprite.transform.Rotate(new Vector3(0, 0, 3));
                }
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
    public void HandCrash(float cost)
    {
        hitFlag = false;
        if(attackFlag || currentSP < cost) {
            return;
        }
        attackFlag = true;

        currentSP -= cost;

        _animator.Play("golem_attack");
    }

    public void RocketPunch(float cost)
    {
        hitFlag = false;
        if(attackFlag || currentSP < cost) {
            return;
        }
        attackFlag = true;

        currentSP -= cost;

        _animator.Play("golem_punch");
    }

    public void RollingThunder(float cost)
    {
        hitFlag = false;
        if(attackFlag || currentSP < cost) {
            return;
        }
        attackFlag = true;

        currentSP -= cost;

        _rollSprite.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);

        _animator.Play("golem_circle_on");
    }

    void Rolling_End()
    {
        _GFX.SetActive(true);
        _rollSprite.SetActive(false);
        _rollSprite.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        _animator.Play("golem_circle_off");
    }

    void Hit()
    {
        _animator.Play("golem_hit");
    }

    void Die()
    {
        _animator.Play("golem_die");
        _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
    }

    // 맞는거 끝났을 때(애니메이션 이벤트)
    public void HitEnd()    
    {
        hitFlag = false;
    }

    // 죽는모션 이후(애니메이션 이벤트)
    public void AfterDeath()
    {
        Destroy(gameObject);
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
