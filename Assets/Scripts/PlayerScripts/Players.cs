using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Players : MonoBehaviour
{

    //public enum ActionState { IDLE, RUB, RUN, JUMP, DASH, FALL }

    //public ActionState state
    //{
    //    get
    //    {
    //        return _state;
    //    }
    //    set
    //    {
    //        if (_state != value)
    //        {
    //            _state = value;
    //        }
    //    }
    //}


    //private ActionState _state;

    public enum AnimState
    {
        IDLE, RUB, RUN, JUMP, DASH, FALL
    }

    private AnimState _AnimState;

    private string CurrentAnimation;


    [Header("Speeds & Timings")]
    public float moveSpeed = 6f;
    public float jumpSpeed = 1f;
    public float dashSpeed = 1f;
    public float aimDuration = 1f;
    public float jumpDuration = 0.5f;
    [Range(1, 3)]
    public int maxJumps = 1;

    

    [Header("Animations")]
    //[SpineAnimation]
    //public string runBackwardAnim;
    //[SpineAnimation]
    //public string runAnim;
    [SpineAnimation]
    public string idleAnim;
    //[SpineAnimation]
    //public string jumpAnim;
    //[SpineAnimation]
    //public string fallAnim;
    [SpineAnimation]
    public string shootAnim;
    //[SpineAnimation]
    //public string dashAnim;


    //[SpineBone(dataField: "skeletonAnimation")]
    
    public Transform graphicsRoot;
    public SkeletonUtilityBone aimPivotBone;


    [Header("References")]
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    public Rigidbody2D RB
    {
        get
        {
            return this.rb;
        }
    }

    public bool Flipped
    {
        get
        {
            return this.flipped;
        }
    }

    private Vector3 moveAmount;
    private Vector3 right;
    private Vector3 left;
    private Rigidbody2D rb;
    private bool isGround = false;
    private bool isJump = false;
    private bool isDash = false;
    private bool dashCheck = true;
    private bool flipped;
    private bool aiming = false;
    private float dir1;
    private float aTime = 0f;


    float rotSpeed = 20f;
    float speed = 5f;

    float angle;
    Vector2 target, mouse;
    Vector2 aimStick = Vector2.zero;

    Plane mouseCastPlane;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = transform.position;
        right = new Vector3(1, 1, 1);
        left = new Vector3(-1, 1, 1);

        mouseCastPlane = new Plane(Vector3.forward, transform.position);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Bottom")
        {
            maxJumps = 2;
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

    

    void Move()
    {
        dir1 = Input.GetAxisRaw("Horizontal");

        Vector3 dir = Vector3.zero;



        if (dir1 == 0f)                         //방향이 0이면(움직이지 않으면)
        {
            if (isGround)                       //애니메이션 재생
                _AnimState = AnimState.IDLE;
            //else if (rb.velocity.x != 0)
            //    _AnimState = AnimState.DASH;
            else if (rb.velocity.y > 0)
                _AnimState = AnimState.JUMP;
            else
                _AnimState = AnimState.FALL;
        }
        else
        {
            if (isGround)
            {
                if ((aimStick.x > 0 && dir1 > 0) || (aimStick.x < 0 && dir1 < 0))
                    _AnimState = AnimState.RUN;
                else if ((aimStick.x > 0 && dir1 < 0) || (aimStick.x < 0 && dir1 > 0))
                    _AnimState = AnimState.RUB;

                //if (rb.velocity.x != 0)
                //    _AnimState = AnimState.DASH;
            }
            //else if (rb.velocity.x != 0)
            //    _AnimState = AnimState.DASH;
            else if (rb.velocity.y > 0)
                _AnimState = AnimState.JUMP;
            else
                _AnimState = AnimState.FALL;

            dir = new Vector3(dir1, 0, 0);                  //방향에 좌우 따라 맞춤
                                                            //transform.localScale = new Vector2(dir1, 1);
        }

        moveAmount = dir * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);

    }

    void Jump()
    {
        //if (!canMove)
        //{
        //    Stun();
        //    return;
        //}

        if (maxJumps > 0)
        {
            rb.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpSpeed);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

            maxJumps--;
        }
    }

    void UpdateAnim()
    {
        //switch (state)
        //{
        //    case ActionState.IDLE:
        //        skeletonAnimation.AnimationName = idleAnim;
        //        break;
        //    //case ActionState.WALK:
        //    //    if (aiming)
        //    //    {
        //    //        if (Mathf.Sign(aimStick.x) != Mathf.Sign(moveStick.x))
        //    //            skeletonAnimation.AnimationName = walkBackwardAnim;
        //    //        else
        //    //            skeletonAnimation.AnimationName = walkAnim;
        //    //    }
        //    //    else
        //    //    {
        //    //        skeletonAnimation.AnimationName = walkAnim;
        //    //    }

        //    //    break;
        //    case ActionState.RUN:
        //        skeletonAnimation.AnimationName = runAnim;
        //        break;
        //    case ActionState.RUB:
        //        skeletonAnimation.AnimationName = jumpAnim;
        //        break;
        //    case ActionState.JUMP:
        //        skeletonAnimation.AnimationName = jumpAnim;
        //        break;
        //    //case ActionState.FALL:
        //    //    skeletonAnimation.AnimationName = fallAnim;
        //    //    break;
        
    }

    void ChaseMouse()
    {
        if(Input.GetButtonDown("BasicAttack"))
        {
            skeletonAnimation.state.SetAnimation(1, shootAnim, false);
            aTime = 0f;
            aiming = true;
            
        }

        if (aimDuration < aTime)
        {
            skeletonAnimation.state.SetAnimation(1, idleAnim, false);
            aTime = 0f;
            aiming = false;
        }



        bool flip = false;

        float dist = 0;
        var aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (mouseCastPlane.Raycast(aimRay, out dist))
        {
            Vector2 aimPivotPos = aimPivotBone.transform.position;
            Vector2 targetPos = aimRay.GetPoint(dist);
            aimStick = (targetPos - aimPivotPos).normalized;
        }


        

        if (flipped)
        {
            this.transform.localScale = left;
        }
        else
        {
            this.transform.localScale = right;
        }

        

        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;

        //float a = angle - 90;

        //if (a < -90)
        //{
        //    this.transform.localScale = new Vector3(-1, 1, 1);
        //}
        //else if (a > 90)
        //{
        //    this.transform.localScale = new Vector3(-1, 1, 1);
        //}

        //aimPivotBone.transform.localRotation = Quaternion.AngleAxis(a, Vector3.forward);





        float a = Mathf.Atan2(aimStick.y, aimStick.x) * Mathf.Rad2Deg;
        if (a < 0)
            a += 360;

        if (a < 270 && a > 90)
            flip = true;
        else
            flip = false;

        flipped = flip;

        
        //aimPivotBone.transform.localRotation = Quaternion.AngleAxis(a, Vector3.forward);
        //aimPivotBone.transform.localRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        float minAngle = -180;
        float maxAngle = 180;

        a = flip ? 180 + Mathf.Clamp(Mathf.DeltaAngle(0, a - 180), -maxAngle, -minAngle) : Mathf.Clamp(Mathf.DeltaAngle(0, a), minAngle, maxAngle);

        aimPivotBone.transform.localRotation = Quaternion.RotateTowards(aimPivotBone.transform.localRotation, Quaternion.AngleAxis(flip ? 180 - a : a, Vector3.forward), 300 * Time.deltaTime);

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
        }
        if (aiming)
            aTime += Time.deltaTime;

        ChaseMouse();
        UpdateAnim();

        SetCurrentAnimation(_AnimState);
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            isJump = false;
            Jump();
        }
        Move();
    }
}
