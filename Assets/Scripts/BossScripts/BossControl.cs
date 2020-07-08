using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
    public enum AnimState
    {
        IDLE, WAL, WAR, ATTACK, GTC, CTG, CIR, DIE
    }

    [Header("HP")]
    public int HP;
    public float hpRegenPerSecond = 2f;
    public Slider hpS;
    public TextMeshProUGUI hpText;

    public static int hp;
    private float hpRegen = 0f;
    private int maxHp;

    [Header("SP")]
    public int SP;
    public float spRegenPerSecond = 2f;
    public Slider spS;
    public TextMeshProUGUI spText;

    public static int sp;
    private float spRegen = 0f;
    private int maxSp;


    [Header("References")]
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    public BoxCollider2D main;
    public CircleCollider2D rollRange;
    public BoxCollider2D attackRange;

    private AnimState _AnimState;
    private string CurrentAnimation;
    public float moveDelay = 1f;

    public float rollSpeed = 100f;
    //public float moveDelay = 1.5f;
    public float bossSpeed = 5f;
    //public int bossStep = 3;

    //public float radius = 1f;

    //public Transform bossPos;

    //private int bStep;

    //private float timeLeft;
    //private float nextTime = 0.0f;

    //private float fTime = 0.0f;
    //private float firstTime = 0.0f;

    //private Transform transform;

    //public Transform target;
    //public BoxCollider2D collider;
    //SpriteRenderer renderer;
    Rigidbody2D rigid;
    Transform transform;

    bool mHit = false;
    bool rollOn = false;

    float dir1;

    private bool isMove = true;
    private float sTime = 0f;
    private float rollTime = 0f;
    float deathTime = 0f;
    int step = 0;
    float skTime = 0f;
    bool skill1Range = false;
    bool death = false;

    Vector3 moveAmount;
    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        hp = HP;
        maxHp = HP;
        hpS.maxValue = (float)HP;
        hpS.value = (float)HP;

        sp = SP;
        maxSp = SP;
        spS.maxValue = (float)SP;
        spS.value = (float)SP;
        //bStep = Random.Range(2, 5);
        //transform = GetComponent<Transform>();
        //renderer = GetComponent<SpriteRenderer>();
        //timeLeft = moveDelay;
        //rangeOn = false;
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    private void _AsyncAnimation(AnimationReferenceAsset animCip, bool loop, float timeScale)
    {
        //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 x
        if (animCip.name.Equals(CurrentAnimation))
            return;

        //해당 애니메이션으로 변경한다.
        skeletonAnimation.state.SetAnimation(0, animCip, loop).TimeScale = timeScale;


        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //현재 재생되고 있는 애니메이션 값을 변경
        CurrentAnimation = animCip.name;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        _AsyncAnimation(AnimClip[(int)_state], true, 1f);
    }

    private void SetCurrentAnimation(AnimState _state, bool loop)
    {
        _AsyncAnimation(AnimClip[(int)_state], loop, 1f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (rollOn)
            {
                //WheelGooo.hit = true;
            }
            if (!death && !rollOn)
            {
                hpS.value -= 10f;
                hp -= 10;
            }
            
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (skill1Range)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                if (mHit && skTime > 0.85f)
                {
                    //WheelGooo.hit = true;
                    Debug.Log("hit");
                    mHit = false;
                }
            }
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMove && !death)
            BossMove();
    }

    void Update()
    {
        skillManager();
        regen();
        //BossStep();
        //rangeOn = Physics2D.OverlapCircle(bossPos.position, radius, 1 << LayerMask.NameToLayer("Player"));
        //if (isDelay)
        //    SetCurrentAnimation(_AnimState);
        //else if (!isDelay)
        //    SetCurrentAnimation(_AnimState, isDelay);
        SetCurrentAnimation(_AnimState);
        deathTime += Time.deltaTime;
        if (death)
        {
            Debug.Log("death");
            deathTime = 0f;
            _AnimState = AnimState.DIE;
            SetCurrentAnimation(_AnimState, false);
            Debug.Log("Daim");
            if (deathTime > 1.0f)
            {
                Debug.Log("destroy");
                Destroy(gameObject);
            }
        }
            
        if (!death && hp <= 0)
        {
            hpS.value = 0;
            hp = 0;
            death = true;
            Debug.Log("death true");
        }
    }

    void regen()
    {
        //hp 리젠
        if (!death)
        {
            hpRegen += hpRegenPerSecond * Time.deltaTime;
            hpS.value += hpRegenPerSecond * Time.deltaTime;
        }
        hpText.text = hp + "/" + maxHp;
        if (hpRegen > 1.0f)
        {
            if (hp < maxHp)
                hp += 1;
            hpRegen = 0f;
        }

        //sp 리젠
        if (!death)
        {
            spRegen += spRegenPerSecond * Time.deltaTime;
            spS.value += spRegenPerSecond * Time.deltaTime;
        }
        spText.text = sp + "/" + maxSp;
        if (spRegen > 1.0f)
        {
            if (sp < maxSp)
                sp += 1;
            spRegen = 0f;
        }
    }
    void BossMove()
    {
        dir1 = Input.GetAxisRaw("Horizontal");

        dir = Vector3.zero;

        if (dir1 == 0f)
        {
            _AnimState = AnimState.IDLE;
        }
        else
        {
            dir = new Vector3(dir1, 0, 0);
            transform.localScale = new Vector2(dir1, 1);
            if (moveDelay < sTime)
            {
                step++;
                if ((step % 2) == 0)
                {
                    _AnimState = AnimState.WAL;
                    SetCurrentAnimation(_AnimState, false);
                    moveAmount = dir * bossSpeed * Time.deltaTime;
                    transform.Translate(moveAmount);
                    sTime = 0f;
                }
                else if ((step % 2) != 0)
                {
                    _AnimState = AnimState.WAR;
                    SetCurrentAnimation(_AnimState, false);
                    moveAmount = dir * bossSpeed * Time.deltaTime;
                    transform.Translate(moveAmount);
                    sTime = 0f;
                }
            }
        }
    }

    void skillManager()
    {
        sTime += Time.deltaTime;
        skTime += Time.deltaTime;
        rollTime += Time.deltaTime;
        

        if (skill1Range)
        {
            if (skTime < 1.4f)
            {
                _AnimState = AnimState.ATTACK;
                SetCurrentAnimation(_AnimState, false);
            }
            if (skTime > 1.4f)
            {
                skTime = 0f;
                skill1Range = false;
                isMove = true;
                attackRange.enabled = false;
                SetCurrentAnimation(_AnimState);
            }

        }

        if (rollOn)
        {
            if (rollTime < 0.3f)
            {
                _AnimState = AnimState.GTC;
                SetCurrentAnimation(_AnimState, false);
            }
            if (rollTime > 0.3f && rollTime < 1.3f)
            {
                _AnimState = AnimState.CIR;
                SetCurrentAnimation(_AnimState, false);
                //rigid.velocity = new Vector2((transform.localScale.x * rollSpeed * Time.deltaTime), rigid.velocity.y);
                //moveAmount = dir * rollSpeed * Time.deltaTime;
                //transform.Translate(moveAmount);
                //transform.localRotation = new Quaternion(0, 0, (transform.localScale.x * 8f), 0);
            }
            if (rollTime > 1.3f && rollTime < 1.6f)
            {
                _AnimState = AnimState.CTG;
                //rigid.velocity = new Vector2(0f, 0f);
                
                rollRange.enabled = false;
                main.enabled = true;
                SetCurrentAnimation(_AnimState, false);
            }
            if (rollTime > 1.6f)
            {
                rollTime = 0f;
                rollOn = false;
                isMove = true;
                //transform.localRotation = new Quaternion(0, 0, 0, 0);
                SetCurrentAnimation(_AnimState);
            }
        }
    }
    public void Skill1()
    {
        attackRange.enabled = true;
        skTime = 0f;
        Debug.Log("s");
        skill1Range = true;
        mHit = true;
        isMove = false;
    }

    public void Roll()
    {
        main.enabled = false;
        rollRange.enabled = true;
        rollTime = 0f;
        Debug.Log("r");
        rollOn = true;
        isMove = false;
        //skTime = 0f;
        //Debug.Log("s");
        //skill1Range = true;
        //mHit = true;
        //isMove = false;
    }

    //void Stomp()
    //{

    //}
}
