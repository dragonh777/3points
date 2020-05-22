using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //스파인 애니메이션을 위한 것
    [Header("References")]
    public SkeletonAnimation skeletonAnimation;
    public Transform graphicsRoot;
    public SkeletonUtilityBone aimPivotBone;

    //public AnimationReferenceAsset[] AnimClip;
    public enum ActionState { IDLE, WALK, RUN, JUMP, FALL}

    public ActionState state
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != value)
            {
                _state = value;
            }
        }
    }

    private ActionState _state;

    [Header("Animations")]
    [SpineAnimation]
    public string walkAnim;
    [SpineAnimation]
    public string walkBackwardAnim;
    [SpineAnimation]
    public string runAnim;
    [SpineAnimation]
    public string idleAnim;
    [SpineAnimation]
    public string jumpAnim;
    [SpineAnimation]
    public string fallAnim;


    //public SkeletonUtilityBone aimPivot;

    //움직임 관련
    public float moveSpeed = 6f;
    public float jumpPower = 14f;
    public float dashPower = 1f;
    public float maxDashTime = 1.0f;
    public float dashStoppingTime = 0.1f;

    public int jumpCount = 2;

    private float currentDashTime;

    private bool isGround = false;
    private bool isJump = false;
    private bool isDash = false;
    private bool dashCheck = true;

    public static bool canMove = true;

    //방향 관련
    private float dir1;


    //애니메이션에 대한 Enum
    //public enum AnimState
    //{
    //    idle, run, jump, fall, stop, dash
    //}

    //public AnimState state
    //{
    //    get
    //    {
    //        return _AnimState;
    //    }
    //    set
    //    {
    //        if (_AnimState != value)
    //        {
    //            _AnimState = value;
    //        }
    //    }
    //}

    ////현재 애니메이션 처리가 무엇인지에 대한 변수
    //private AnimState _AnimState;

    ////현재 어떤 애니메이션이 재생되고 있는지에 대한 변수
    //private string CurrentAnimation;

    //무브처리
    private Rigidbody2D rigid;
    private Vector3 moveAmount;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();

    }
    // Start is called before the first frame update
    void Start()
    {
        jumpCount = 0;
        currentDashTime = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        dir1 = Input.GetAxisRaw("Horizontal");          //방향 잡아줌

        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
        }
        if (Input.GetButtonDown("Dash") && dashCheck)
        {   // 대쉬키 left shift
            isDash = true;
        }
        //애니메이션 적용
        //SetCurrentAnimation(_AnimState);
    }

    private void FixedUpdate()
    {
        Move();
           

        if (isJump)
        {
            isJump = false;
            Jump();
        }
        if (isDash)
        {
            isDash = false;
            Dash();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Bottom")
        {
            jumpCount = 2;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Bottom")
        {
            isGround = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Bottom")
        {
            isGround = false;
        }
    }

    //private void _AsyncAnimation(AnimationReferenceAsset animCip, bool loop, float timeScale)
    //{
    //    //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 x
    //    if (animCip.name.Equals(CurrentAnimation))
    //        return;

    //    //해당 애니메이션으로 변경한다.
    //    skeletonAnimation.state.SetAnimation(0, animCip, loop).TimeScale = timeScale;
    //    skeletonAnimation.loop = loop;
    //    skeletonAnimation.timeScale = timeScale;

    //    //현재 재생되고 있는 애니메이션 값을 변경
    //    CurrentAnimation = animCip.name;
    //}

    //private void SetCurrentAnimation(AnimState _state)
    //{
    //    _AsyncAnimation(AnimClip[(int)_state], true, 1f);
    //}


    void Move()
    {
        if(!canMove) {
            Stun();
            return;
        }

        Vector3 dir = Vector3.zero;
        
        if (dir1 == 0f)                         //방향이 0이면(움직이지 않으면)
        {
            //if (isGround)                       //애니메이션 재생
            //    _AnimState = AnimState.idle;
            //else if (rigid.velocity.x != 0)
            //    _AnimState = AnimState.dash;
            //else if (rigid.velocity.y > 0)
            //    _AnimState = AnimState.jump;
            //else
            //    _AnimState = AnimState.fall;
        }
        else
        {
            //if (isGround)
            //{
            //    _AnimState = AnimState.run;
            //    if (rigid.velocity.x != 0)
            //        _AnimState = AnimState.dash;
            //}
            //else if (rigid.velocity.x != 0)
            //    _AnimState = AnimState.dash;
            //else if (rigid.velocity.y > 0)
            //    _AnimState = AnimState.jump;
            //else
            //    _AnimState = AnimState.fall;

            dir = new Vector3(dir1, 0, 0);                  //방향에 좌우 따라 맞춤
            transform.localScale = new Vector2(dir1, 1);
        }
        
        moveAmount = dir * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);
    }

    void Jump()
    {
        if (!canMove) {
            Stun();
            return;
        }

        if (jumpCount > 0)
        {
            rigid.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

            jumpCount--;
        }
    }

    void Dash()
    {
        if (!canMove) {
            Stun();
            return;
        }

        rigid.velocity = Vector2.zero;
        currentDashTime = 0f;
        dashCheck = false;

        while (currentDashTime < maxDashTime)
        {
            if (dir1 < 0)
            {
                Vector2 dashVelocity = new Vector2(-dashPower, 0);
                rigid.AddForce(dashVelocity, ForceMode2D.Impulse);
                currentDashTime += dashStoppingTime;
            }
            else if (dir1 > 0)
            {
                Vector2 dashVelocity = new Vector2(dashPower, 0);
                rigid.AddForce(dashVelocity, ForceMode2D.Impulse);
                currentDashTime += dashStoppingTime;
            }
            else
            {
                break;
            }
        }
        StartCoroutine("DashCoroutine");
    }
    IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(1.5f);  // 현재 1.5초딜레이
        dashCheck = true;
    }

    //void UpdateAnim()
    //{
    //    switch (state)
    //    {
    //        case ActionState.IDLE:
    //            if (CenterOnGround)
    //            {
    //                skeletonAnimation.AnimationName = idleAnim;
    //            }
    //            else
    //            {
    //                //TODO:  deal with edge animations for this character rig
    //                if (onIncline)
    //                    skeletonAnimation.AnimationName = idleAnim;
    //                else if (BackOnGround)
    //                {
    //                    skeletonAnimation.AnimationName = idleAnim;
    //                }
    //                else if (ForwardOnGround)
    //                {
    //                    skeletonAnimation.AnimationName = idleAnim;
    //                }
    //            }
    //            break;
    //        case ActionState.WALK:
    //            if (aiming)
    //            {
    //                if (Mathf.Sign(aimStick.x) != Mathf.Sign(moveStick.x))
    //                    skeletonAnimation.AnimationName = walkBackwardAnim;
    //                else
    //                    skeletonAnimation.AnimationName = walkAnim;
    //            }
    //            else
    //            {
    //                skeletonAnimation.AnimationName = walkAnim;
    //            }

    //            break;
    //        case ActionState.RUN:
    //            skeletonAnimation.AnimationName = runAnim;
    //            break;
    //        case ActionState.JUMP:
    //            skeletonAnimation.AnimationName = jumpAnim;
    //            break;
    //        case ActionState.FALL:
    //            skeletonAnimation.AnimationName = fallAnim;
    //            break;
    //        case ActionState.JETPACK:
    //            if (moveStick.x > deadZone)
    //                skeletonAnimation.AnimationName = flipped ? jetpackBackwardAnim : jetpackForwardAnim;
    //            else if (moveStick.x < -deadZone)
    //                skeletonAnimation.AnimationName = flipped ? jetpackForwardAnim : jetpackBackwardAnim;
    //            else
    //                skeletonAnimation.AnimationName = jetpackNeutralAnim;
    //            break;
    //    }
    //}

    void Stun() // 차후 피격애니메이션 넣으면 됨
    {
        //_AnimState = AnimState.idle;
    }
}
