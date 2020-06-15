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
        IDLE, WAL, WAR, ATTACK, GTC, CTG, CIR
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
    public BoxCollider2D attackRange;

    private AnimState _AnimState;
    private string CurrentAnimation;
    public float moveDelay = 1f;

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



    //bool rangeOn;

    private bool isDelay = true;
    private float sTime = 0f;
    private bool isMove = false;
    int step = 0;
    float skTime = 0f;
    bool skill1Range = false;

    Vector3 moveAmount;

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


    // Update is called once per frame
    void FixedUpdate()
    {
        BossMove();
    }

    void Update()
    {
        

        sTime += Time.deltaTime;
        skTime += Time.deltaTime;
        regen();
        //BossStep();
        //rangeOn = Physics2D.OverlapCircle(bossPos.position, radius, 1 << LayerMask.NameToLayer("Player"));
        //if (isDelay)
        //    SetCurrentAnimation(_AnimState);
        //else if (!isDelay)
        //    SetCurrentAnimation(_AnimState, isDelay);
        SetCurrentAnimation(_AnimState);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            hpS.value -= 10f;
            hp -= 10;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(skill1Range)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                WheelGooo.hit = true;
                Debug.Log("hit");
            }
            skill1Range = false;
        }
    }

    void regen()
    {
        //hp 리젠
        hpRegen += hpRegenPerSecond * Time.deltaTime;
        hpS.value += hpRegenPerSecond * Time.deltaTime;
        hpText.text = hp + "/" + maxHp;
        if (hpRegen > 1.0f)
        {
            if (hp < maxHp)
                hp += 1;
            hpRegen = 0f;
        }

        //sp 리젠
        spRegen += spRegenPerSecond * Time.deltaTime;
        spS.value += spRegenPerSecond * Time.deltaTime;
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
        float dir1 = Input.GetAxisRaw("Horizontal");

        Vector3 dir = Vector3.zero;

        if (dir1 > 0)
            attackRange.offset = new Vector2(2f, 1.75f);

        else if (dir1 < 0)
            attackRange.offset = new Vector2(2f, 1.75f);

        if (skill1Range)
        {
            _AnimState = AnimState.ATTACK;
            SetCurrentAnimation(_AnimState, false);
        }
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
        if (isMove)
        {
            //float dir = target.position.x - transform.position.x;
            //if (dir < 0)
            //{
            //    moveAmount = bossSpeed * Vector3.left * Time.deltaTime;
            //    renderer.flipX = true;
            //}
            //else if (dir > 0)
            //{
            //    moveAmount = bossSpeed * Vector3.right * Time.deltaTime;
            //    renderer.flipX = false;
            //}
            //moveAmount = bossSpeed * Vector3.right * Time.deltaTime;
            transform.Translate(moveAmount);
        }
    }

    public void Skill1()
    {
        skTime = 0f;
        skill1Range = true;
    }

    //void Stomp()
    //{

    //}
}
