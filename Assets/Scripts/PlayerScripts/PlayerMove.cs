using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //Move변수
    public float moveSpeed = 6f;            //이동속도
    public float forceGravity = 1f;         //중력크기
    public static bool pLeft;               //플레이어좌우판별

    //Jump변수
    public float shortJumpPower = 10f;      //숏점프크기
    public float jumpPower = 14f;           //점프크기
    public float checkRaius = 0.3f;         //충돌지름크기
    public int jumpMax = 2;                 //점프횟수
    public Transform pos;                   //위치
    int layerMask;                          //레이어변수
    public float rayAmount = 0.45f;         //레이캐스트 레이저 크기
    public float rayAmount2 = 0.45f;
    public float rayLength = 0.1f;          //레이캐스트 레이저 나타나는 시간
    public float rayParameter = 0.35f;      //레이캐스트 레이저 길이
    public float rayParameter2 = 0.35f;

    private int jumpCnt;                    //점프횟수 초기화변수

    //Skill변수
    public GameObject bullet;               //총알  
    public float bulletPos = 1f;            //총알위치
    private float bulletP;                  //총알위치 초기화변수

    //Hp변수
    public Slider healthBarSlider;          //체력바
    public static int Hp;                   //체력
    public int hpMax = 3;                   //최대체력

    public GameObject currentPanel;         //현재 패널
    public GameObject nextPanel;            //다음 패널
    public LayerMask isFloor;               //바닥
    public LayerMask Hill;                  //언덕

    private Rigidbody2D rigid;              //
    RaycastHit2D HitL, HitR, fHitL, fHitR;
    SpriteRenderer renderer;
    //Animator anim;

    private Vector3 moveAmount;
    

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Floor");
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        //anim = GetComponent<Animator>();
        jumpCnt = jumpMax;
        bulletP = bulletPos;
        Hp = hpMax;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        Jump();
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(bullet, transform.position + new Vector3(bulletP, 0, 0), Quaternion.identity);
        }
        if (Hp == 0)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && healthBarSlider.value > 0)
        {
            healthBarSlider.value--;
            Debug.Log("Hit");
        }
        if (collision.gameObject.tag == "RightWall")
        {
            currentPanel.gameObject.SetActive(false);
            nextPanel.gameObject.SetActive(true);
        }
    }

    void Move()
    {
        //fHitL = Physics2D.Raycast(transform.position - new Vector3(rayParameter2, rayAmount2, 0), Vector3.left, rayLength);
        //fHitR = Physics2D.Raycast(transform.position - new Vector3(-rayParameter2, rayAmount2, 0), Vector3.right, rayLength);

        //Debug.DrawRay(transform.position - new Vector3(rayParameter2, rayAmount2, 0), Vector3.left * rayLength, Color.red, 0.1f);
        //Debug.DrawRay(transform.position - new Vector3(-rayParameter2, rayAmount2, 0), Vector3.right * rayLength, Color.red, 0.1f);

        float moves = Input.GetAxis("Horizontal");

        Vector3 dir = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            dir = Vector3.left;
            renderer.flipX = false;
            //anim.SetBool("isWalk", true);
            pLeft = true;
            bulletP = -bulletPos;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            dir = Vector3.right;
            renderer.flipX = true;
            //anim.SetBool("isWalk", true);
            pLeft = false;
            bulletP = bulletPos;
        }

        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            //anim.SetBool("isWalk", false);
        }
        
        moveAmount = dir * moveSpeed * Time.deltaTime;
        //if (fHitL.collider == null && fHitR.collider == null)
        //{
        //transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);
        //}
        //else if (fHitL.collider != null || fHitR.collider != null)
        //{
        //    rigid.velocity = new Vector2(moves * moveSpeed, rigid.velocity.y);
        //}

    }

    void Jump()
    {
        HitL = Physics2D.Raycast(transform.position - new Vector3(rayParameter, rayAmount, 0), Vector3.down, rayLength);
        HitR = Physics2D.Raycast(transform.position - new Vector3(-rayParameter, rayAmount, 0), Vector3.down, rayLength);
        Debug.DrawRay(transform.position - new Vector3(rayParameter, rayAmount, 0), Vector3.down * rayLength, Color.red, 0.1f);
        Debug.DrawRay(transform.position - new Vector3(-rayParameter, rayAmount, 0), Vector3.down * rayLength, Color.red, 0.1f);
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        if (HitL.collider == null|| HitR.collider == null)
        {
            Debug.Log("aa");
            //anim.SetBool("isJump", true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            jumpCnt--;
            if (jumpCnt > 0)
            {
                if (HitL.collider == null|| HitR.collider == null)
                {
                    jumpCnt--;
                    rigid.velocity = Vector2.zero;
                    rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
                }
                rigid.velocity = Vector2.zero;
                rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
            }
        }
        if (HitL.collider != null|| HitR.collider != null)
        {
            jumpCnt = jumpMax;
            //anim.SetBool("isJump", false);
        }
    }
}
