using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class Players : MonoBehaviour
{

    public enum ActionState { IDLE, RUN, JUMP, DASH, FALL }

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


    [Header("Speeds & Timings")]
    public float moveSpeed = 6f;
    public float jumpSpeed = 1f;
    public float dashSpeed = 1f;
    public float jumpDuration = 0.5f;
    [Range(1, 3)]
    public int maxJumps = 1;


    [Header("Animations")]
    [SpineAnimation]
    public string runBackwardAnim;
    [SpineAnimation]
    public string runAnim;
    [SpineAnimation]
    public string idleAnim;
    [SpineAnimation]
    public string jumpAnim;
    [SpineAnimation]
    public string fallAnim;
    [SpineAnimation]
    public string shootAnim;
    //[SpineAnimation]
    //public string dashAnim;


    [SpineBone(dataField: "skeletonAnimation")]
    public Transform graphicsRoot;
    public SkeletonUtilityBone aimPivotBone;


    [Header("References")]
    public BoxCollider2D primaryCollider;
    public SkeletonAnimation skeletonAnimation;

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
    private Rigidbody2D rb;
    private bool isGround = false;
    private bool isJump = false;
    private bool isDash = false;
    private bool dashCheck = true;
    private bool flipped;
    private bool aiming;
    private float dir1;


    float rotSpeed = 20f;
    float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


    }

    void Move()
    {
        dir1 = Input.GetAxisRaw("Horizontal");

        Vector3 dir = Vector3.zero;

        if (dir1 == 0f)                         //방향이 0이면(움직이지 않으면)
        {
            if (isGround)                       //애니메이션 재생
                state = ActionState.IDLE;
            //else if (rb.velocity.x != 0)
            //    state = ActionState.DASH;
            else if (rb.velocity.y > 0)
                state = ActionState.JUMP;
            //else
            //    state = ActionState.FALL;
        }
        else
        {
            if (isGround)
            {
                state = ActionState.RUN;
                //if (rb.velocity.x != 0)
                //    state = ActionState.DASH;
            }
            //else if (rb.velocity.x != 0)
            //    state = ActionState.DASH;
            else if (rb.velocity.y > 0)
                state = ActionState.JUMP;
            //else
            //    state = ActionState.FALL;

            dir = new Vector3(dir1, 0, 0);                  //방향에 좌우 따라 맞춤
            transform.localScale = new Vector2(dir1, 1);
        }

        moveAmount = dir * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);
        //Debug.Log("a");
    }

    void UpdateAnim()
    {
        switch (state)
        {
            case ActionState.IDLE:
                skeletonAnimation.AnimationName = idleAnim;
                break;
            //case ActionState.WALK:
            //    if (aiming)
            //    {
            //        if (Mathf.Sign(aimStick.x) != Mathf.Sign(moveStick.x))
            //            skeletonAnimation.AnimationName = walkBackwardAnim;
            //        else
            //            skeletonAnimation.AnimationName = walkAnim;
            //    }
            //    else
            //    {
            //        skeletonAnimation.AnimationName = walkAnim;
            //    }

            //    break;
            case ActionState.RUN:
                skeletonAnimation.AnimationName = runAnim;
                break;
            case ActionState.JUMP:
                skeletonAnimation.AnimationName = jumpAnim;
                break;
            //case ActionState.FALL:
            //    skeletonAnimation.AnimationName = fallAnim;
            //    break;
        }
    }

    void ChaseMouse()
    {
        bool flip = false;

        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //h = h * speed * Time.deltaTime;
        //v = h * speed * Time.deltaTime;

        //transform.Translate(Vector3.right * h);
        //transform.Translate(Vector3.forward * h);
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Vector3 fixedPosition = new Vector3(0, 0, position.x);
        //transform.Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        skeletonAnimation.state.SetAnimation(1, shootAnim, false);
        
        //position = 

        float a = Mathf.Atan2(position.y, position.x) * Mathf.Rad2Deg;
        if (a < 0)
            a += 360;

        if (a < 270 && a > 90)
            flip = true;
        else
            flip = false;

        flipped = flip;

        float minAngle = -120;
        float maxAngle = 120;

        a = flip ? 180 + Mathf.Clamp(Mathf.DeltaAngle(0, a - 180), -maxAngle, -minAngle) : Mathf.Clamp(Mathf.DeltaAngle(0, a), minAngle, maxAngle);

        aimPivotBone.transform.localRotation = Quaternion.RotateTowards(aimPivotBone.transform.localRotation, Quaternion.AngleAxis(flip ? 180 - a : a, Vector3.forward), 300 * Time.deltaTime);

    }
    // Update is called once per frame
    void Update()
    {
        Move();
        
        ChaseMouse();
        UpdateAnim();
    }
}
