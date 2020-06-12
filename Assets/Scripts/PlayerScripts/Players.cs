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
        IDLE, RUB, RUN, JUMP, JUMPD, DASH, FALL
    }

    private AnimState _AnimState;

    private string CurrentAnimation;


    [Header("Speeds & Timings")]
    public float moveSpeed = 6f;
    public float backmoveSpeed = 3f;
    public float jumpSpeed = 1f;
    public float dashSpeed = 1f;
    public float aimDuration = 1f;
    public float jumpDuration = 0.5f;
    public float attackDelay = 0.5f;
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
    public Transform aimPivot;
    public SkeletonUtilityBone aimPivotBone;


    [Header("References")]
    public GameObject bulletEffect;
    public GameObject bulletPrefab;
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
    private bool flip = false;
    private bool aiming = false;
    private float dir1;
    private float aTime = 0f;
    private float atTime;
    private float movetmp = 0f;
    private float a = 0f;
    private float b = 0f;
    private float atd = 0f;

    Quaternion vec;

    float minAngle = -180;
    float maxAngle = 180;

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

        movetmp = moveSpeed;

        atTime = attackDelay;

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
            {
                if (maxJumps == 0)
                    _AnimState = AnimState.JUMPD;
                else
                    _AnimState = AnimState.JUMP;
            }
            else
                _AnimState = AnimState.FALL;
        }
        else
        {
            if (isGround)
            {
                if (aiming)
                {
                    if ((aimStick.x > 0 && dir1 > 0) || (aimStick.x < 0 && dir1 < 0))
                    {
                        moveSpeed = movetmp;
                        _AnimState = AnimState.RUN;
                    }
                    else if ((aimStick.x > 0 && dir1 < 0) || (aimStick.x < 0 && dir1 > 0))
                    {
                        moveSpeed = backmoveSpeed;
                        _AnimState = AnimState.RUB;
                    }
                }
                else
                {
                    moveSpeed = movetmp;
                    _AnimState = AnimState.RUN;
                }

                //if (rb.velocity.x != 0)
                //    _AnimState = AnimState.DASH;
            }
            //else if (rb.velocity.x != 0)
            //    _AnimState = AnimState.DASH;
            else if (rb.velocity.y > 0)
            {
                if (maxJumps == 0)
                    _AnimState = AnimState.JUMPD;
                else
                    _AnimState = AnimState.JUMP;
            }
            else
                _AnimState = AnimState.FALL;
            //방향에 좌우 따라 맞춤
            dir = new Vector3(dir1, 0, 0);
            transform.localScale = new Vector2(dir1, 1);
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

    void fire()
    {
        float dist = 0;
        var aimRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (mouseCastPlane.Raycast(aimRay, out dist))
        {
            Vector2 aimPivotPos = aimPivotBone.transform.position;
            Vector2 targetPos = aimRay.GetPoint(dist);
            aimStick = (targetPos - aimPivotPos).normalized;
        }

        if (a < 0)
            a += 360;

        if (a < 270 && a > 90)
            flip = true;
        else
            flip = false;
        flipped = flip;





        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;

        a = Mathf.Atan2(aimStick.y, aimStick.x) * Mathf.Rad2Deg;
        b = Mathf.Atan2(aimStick.y, aimStick.x) * Mathf.Rad2Deg;

        a = flip ? 180 + Mathf.Clamp(Mathf.DeltaAngle(0, a - 180), -maxAngle, -minAngle) : Mathf.Clamp(Mathf.DeltaAngle(0, a), minAngle, maxAngle);
        b = flip ? 180 + Mathf.Clamp(Mathf.DeltaAngle(0, b - 180), -maxAngle, -minAngle) : Mathf.Clamp(Mathf.DeltaAngle(0, b), minAngle, maxAngle);

        aimPivotBone.transform.localRotation = Quaternion.RotateTowards(aimPivotBone.transform.localRotation, Quaternion.AngleAxis(flip ? 180 - a : a, Vector3.forward), 300 * Time.deltaTime);
        vec = Quaternion.RotateTowards(vec, Quaternion.AngleAxis(flip ? 180 + b : b, Vector3.forward), 300 * Time.deltaTime);
    }

    void ChaseMouse()
    {


        

        if (flipped)
        {
            this.transform.localScale = left;
        }
        else
        {
            this.transform.localScale = right;
        }

        if (Input.GetButtonDown("BasicAttack"))
        {
            //Debug.Log(aimPivotBone.transform.localRotation.z);
            
        }

        //if (aimDuration < aTime)
        //{
        //    skeletonAnimation.state.SetAnimation(1, idleAnim, false);
        //    aTime = 0f;
        //    aiming = false;
        //}
    }
    // Update is called once per frame
    void Update()
    {

        fire();

        if (Input.GetButtonDown("Jump"))
        {
            isJump = true;
        }
        if (Input.GetButtonDown("BasicAttack"))
        {
            //Instantiate(bulletPrefab, aimPivotBone.transform.position, aimPivotBone.transform.rotation);
            //skeletonAnimation.state.SetAnimation(1, shootAnim, false);
            //aTime = 0f;
            if (attackDelay < atTime)
            {
                skeletonAnimation.state.SetAnimation(1, shootAnim, false);
                Instantiate(bulletPrefab, aimPivot.transform.position, vec);
                Instantiate(bulletEffect, aimPivot.transform.position, vec);
                atTime = 0f;
            }
            aTime = 0f;
            aiming = true;

        }
        //Debug.Log(aimPivotBone.transform.position.x);
        if (aimDuration < aTime)
        {
            skeletonAnimation.state.SetAnimation(1, idleAnim, false);
            aTime = 0f;
            aiming = false;
        }

        if (aiming)
        {
            aTime += Time.deltaTime;
            ChaseMouse();
        }

        //ChaseMouse();
        atTime += Time.deltaTime;
        

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
