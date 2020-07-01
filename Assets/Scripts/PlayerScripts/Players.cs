using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Players : MonoBehaviour
{

    public enum AnimState
    {
        IDLE, RUB, RUN, JUMP, JUMPD, DASH, FALL, ILSD, DOWN, GHIT, AHIT, LAND, DIE
    }

    private AnimState _AnimState;

    private string CurrentAnimation;


    [Header("Speeds & Timings")]
    public float moveSpeed = 6f;
    public float backmoveSpeed = 3f;
    public float jumpSpeed = 1f;
    public float dashSpeed = 1f;
    public float dashCooltime = 1.5f;
    public float aimDuration = 1f;
    public float jumpDuration = 0.5f;
    public float attackDelay = 0.5f;
    public int hpMax = 6;
    [Range(1, 3)]
    public int maxJumps = 1;


    public float xx = 0f;

    public static int HP = 0;
    public static bool isHit = false;
    public static bool isDie = false;
    public static bool isGround = false;
    public static bool isMove = false;
    public static bool isJump = false;
    public static bool isLand = false;

    [Header("Animations")]
    [SpineAnimation]
    public string idleAnim;
    [SpineAnimation]
    public string shootAnim;



    [Header("References")]
    public SkeletonUtilityBone aimPivotBone;
    public GameObject bulletEffect;
    public GameObject bulletPrefab;
    public GameObject dashBurstEffect;
    public GameObject dashWindEffect;
    public GameObject dustEffect;
    public GameObject gameOver;
    public Transform aimPivot;
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

    private Plane mouseCastPlane;

    private Vector2 target, mouse;
    private Vector2 aimStick = Vector2.zero;

    private Vector3 moveAmount;
    private Vector3 right;
    private Vector3 left;
    private Vector3 dir;

    private Rigidbody2D rb;




    private bool dashCheck = true;
    private bool flipped;
    private bool flip = false;
    private bool aiming = false;

    private float dir1;
    private float aTime = 0f;
    private float atTime;
    private float dashTime = 0f;
    private float movetmp = 0f;
    private float moveTime = 0f;
    private float a = 0f;
    private float b = 0f;
    private float minAngle = -180;
    private float maxAngle = 180;
    private float angle;
    private float airTime = 0f;

    private int jmpcount;
    private Quaternion vec;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = transform.position;
        right = new Vector3(1, 1, 1);
        left = new Vector3(-1, 1, 1);
        HP = hpMax;

        skeletonAnimation.skeleton.SetColor(new Color32(235, 235, 255, 255));

        jmpcount = maxJumps;

        movetmp = moveSpeed;

        atTime = attackDelay;

        mouseCastPlane = new Plane(Vector3.forward, transform.position);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Enemy" && !isHit)
        //{
        //    HP--;

        //    if (transform.localScale.x > 0)
        //        rb.AddForce(new Vector2(-8, 5), ForceMode2D.Impulse);

        //    else if (transform.localScale.x < 0)
        //        rb.AddForce(new Vector2(8, 5), ForceMode2D.Impulse);

        //    if (HP > 1)
        //    {
        //        isHit = true;
        //        StartCoroutine("invincibility");
        //    }
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Bottom")
        {
            jmpcount = maxJumps;
            if (rb.velocity.y < -10f && rb.velocity.y > -25f)
            {
                isLand = true;
                airTime = 0f;
                Instantiate(dustEffect, transform.position, Quaternion.identity);
            }
            else if (rb.velocity.y < -25f)
            {
                isLand = true;
                DustEffect.isLLand = true;
                airTime = 0f;
                Instantiate(dustEffect, transform.position, Quaternion.identity);
                Debug.Log("LLAND");
            }
        }

        if (collision.gameObject.tag == "Enemy" && !isHit && !dashCheck)
        {
            if (collision.gameObject.name == "BombSeed")
            {
                return;
            }

            HP--;

            if (transform.localScale.x > 0 && HP > 0)
                rb.AddForce(new Vector2(-8, 5), ForceMode2D.Impulse);

            else if (transform.localScale.x < 0 && HP > 0)
                rb.AddForce(new Vector2(8, 5), ForceMode2D.Impulse);

            if (HP > 1)
            {
                isHit = true;
                StartCoroutine("invincibility");
            }
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
            isGround = false;
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


    void Move()
    {


        dir = Vector3.zero;


        if (dir1 == 0f)                         //방향이 0이면(움직이지 않으면)
        {
            isMove = false;
            moveTime = 0f;
            if (isGround)                       //애니메이션 재생
            {
                if (isHit)
                {
                    _AnimState = AnimState.GHIT;
                    SetCurrentAnimation(_AnimState, false);
                }
                else if (aiming)
                    _AnimState = AnimState.ILSD;
                //else if (Input.GetKey(KeyCode.S))
                //    _AnimState = AnimState.DOWN;
                else
                    _AnimState = AnimState.IDLE;
            }

            else if (rb.velocity.y > 0)
            {
                if (isHit)
                    _AnimState = AnimState.AHIT;
                else if (jmpcount == 0)
                {
                    skeletonAnimation.state.SetAnimation(1, idleAnim, false);
                    _AnimState = AnimState.JUMPD;
                }
                else
                    _AnimState = AnimState.JUMP;
            }
            else
            {
                if (isHit)
                    _AnimState = AnimState.AHIT;
                else if (!isGround)
                    _AnimState = AnimState.FALL;

            }
        }
        else
        {
            isMove = true;
            moveTime += Time.deltaTime;
            if (isGround)
            {
                if (moveTime > 0.5f)
                {
                    moveTime = 0f;
                    Instantiate(dustEffect, transform.position, Quaternion.identity);
                }
                if (aiming)
                {
                    if (isHit)
                    {
                        _AnimState = AnimState.GHIT;
                        SetCurrentAnimation(_AnimState, false);
                    }
                    else if ((aimStick.x > 0 && dir1 > 0) || (aimStick.x < 0 && dir1 < 0))
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
                    //if (Input.GetKey(KeyCode.S))
                    //    _AnimState = AnimState.DOWN;
                    //else
                    if (isHit)
                    {
                        _AnimState = AnimState.GHIT;
                        SetCurrentAnimation(_AnimState, false);
                    }
                    else
                        _AnimState = AnimState.RUN;
                }


            }


            else if (rb.velocity.y > 0)
            {
                if (isHit)
                    _AnimState = AnimState.AHIT;
                else if (jmpcount == 0)
                {
                    skeletonAnimation.state.SetAnimation(1, idleAnim, false);
                    _AnimState = AnimState.JUMPD;
                }
                else
                    _AnimState = AnimState.JUMP;
            }
            else
            {
                if (isHit)
                    _AnimState = AnimState.AHIT;
                else if (!isGround)
                    _AnimState = AnimState.FALL;

            }
            //방향에 좌우 따라 맞춤
            dir = new Vector3(dir1, 0, 0);
            transform.localScale = new Vector2(dir1, 1);
        }

        moveAmount = dir * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);

    }

    void Jump()
    {
        if (jmpcount > 0)
        {
            if (!isGround)
                jmpcount = 1;

            rb.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpSpeed);
            rb.AddForce(jumpVelocity, ForceMode2D.Impulse);

            jmpcount--;
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

        aimPivotBone.transform.localRotation = Quaternion.RotateTowards(aimPivotBone.transform.localRotation, Quaternion.AngleAxis(flip ? 180 - a : a, Vector3.forward), 1000 * Time.deltaTime);
        vec = Quaternion.RotateTowards(vec, Quaternion.AngleAxis(flip ? 180 + b : b, Vector3.forward), 10000 * Time.deltaTime);
    }

    void ChaseMouse()
    {
        if (flipped)
            this.transform.localScale = left;

        else
            this.transform.localScale = right;
    }

    void dash()
    {

        if ((Input.GetButtonDown("Dash") && !dashCheck) && (dashCooltime < dashTime))
        {
            if (transform.localScale.x > 0)
                rb.velocity = new Vector2(dashSpeed, 0);

            else if (transform.localScale.x < 0)
                rb.velocity = new Vector2(-dashSpeed, 0);

            skeletonAnimation.state.SetAnimation(1, idleAnim, false);
            Instantiate(dashBurstEffect, transform.position, Quaternion.identity);
            Instantiate(dashWindEffect, transform.position, Quaternion.identity);
            rb.gravityScale = 0f;
            dashTime = 0f;
            dashCheck = true;
            _AnimState = AnimState.DASH;
            SetCurrentAnimation(_AnimState);
            //moveAmount = dir * jumpSpeed * Time.deltaTime;
            //transform.Translate(moveAmount);
        }
    }

    IEnumerator invincibility()
    {
        int cnt = 0;

        while (cnt < 2)
        {
            if (cnt % 2 == 0)
                skeletonAnimation.skeleton.SetColor(new Color32(150, 150, 150, 255));
            else
                skeletonAnimation.skeleton.SetColor(new Color32(235, 235, 255, 255));

            yield return new WaitForSeconds(0.2f);

            cnt++;
        }

        skeletonAnimation.skeleton.SetColor(new Color32(235, 235, 255, 255));

        isHit = false;

        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isDie)
            dash();


        dir1 = Input.GetAxisRaw("Horizontal");

        if (!isDie)
            fire();

        if (Input.GetButtonDown("Jump"))
            isJump = true;

        //if ((Input.GetKeyDown(KeyCode.X) || !isHit) && HP > 0)
        //{

        //}

        if (Input.GetButtonDown("BasicAttack") && !dashCheck && !isDie)
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

        if (isLand && !isMove)
        {

            _AnimState = AnimState.LAND;
            SetCurrentAnimation(_AnimState, false);
            if (airTime > 0.3f)
            {
                isLand = false;
            }

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

        if (dashTime > jumpDuration)
        {
            dashCheck = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
            rb.gravityScale = 1f;
        }

        if (HP <= 0)
        {
            _AnimState = AnimState.DIE;
            SetCurrentAnimation(_AnimState, false);
            isDie = true;
            gameOver.SetActive(true);
        }

        //Debug.Log(Time.time);
        //ChaseMouse();
        atTime += Time.deltaTime;
        dashTime += Time.deltaTime;
        airTime += Time.deltaTime;


        SetCurrentAnimation(_AnimState);
    }

    private void FixedUpdate()
    {
        if (isJump && !dashCheck && !isDie)
        {
            isJump = false;
            Jump();
        }

        if (!dashCheck && !isDie)
            Move();
    }
}
