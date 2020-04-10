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

    private Rigidbody2D rigid;              //리지드바디 변수
    RaycastHit2D HitL, HitR, fHitL, fHitR;  //레이캐스트
    SpriteRenderer renderer;                //스프라이트렌더러
    //Animator anim;

    private Vector3 moveAmount;             //움직인 양
    

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Floor");    //레이어변수에 바닥 넣음
        rigid = GetComponent<Rigidbody2D>();                //자기자신 리지드바디
        renderer = GetComponent<SpriteRenderer>();          //자기자신 렌더러
        //anim = GetComponent<Animator>();                  //자기자신 애니메이터
        jumpCnt = jumpMax;                                  //점프횟수 = 점프최대횟수
        bulletP = bulletPos;                                //총알위치 초기화
        Hp = hpMax;                                         //체력 = 최대체력
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();                                             //물리 관련 함수는 FixedUpdate에, Move(움직임)함수 실행
    }
    private void Update()
    {
        Jump();                                             //점프 함수
        if (Input.GetKeyDown(KeyCode.X))                    //x키를 눌렀을 때
        {
            Instantiate(bullet, transform.position + new Vector3(bulletP, 0, 0), Quaternion.identity);      //Instantiate소환함수(생성할 총알오브젝트, 플레이어의 위치 + 캐릭터 가운데서 소환되지 않게 초기 총알생성위치 정해줌, 한번만 실행)
        }
        if (Hp == 0)                                        //Hp가 0이면
        {
            Destroy(gameObject);                            //현재 오브젝트 파괴
        }
    }

    void OnCollisionEnter2D(Collision2D collision)          //현재 오브젝트의 콜라이더가 다른 콜라이더와 충돌 시 (collision에 다른 콜라이더 반환)
    {
        if (collision.gameObject.tag == "Bullet" && healthBarSlider.value > 0)          //충돌한 콜라이더.(의) 게임오브젝트.(의) 태그가 Bullet이고, 체력슬라이더.(의) 값이 0보다 크면)
        {
            healthBarSlider.value--;                        //체력값을 1 뺴줌
            Debug.Log("Hit");                               //Hit 로그 표시
        }
        if (collision.gameObject.tag == "RightWall")        //충돌한 오브젝트의 태그가 RightWall(오른쪽 벽)이면
        {
            currentPanel.gameObject.SetActive(false);       //현재 패널을 끄고
            nextPanel.gameObject.SetActive(true);           //다음 패널을 킴 (이 기능은 이제 필요없)
        }
    }

    void Move()
    {
        //fHitL = Physics2D.Raycast(transform.position - new Vector3(rayParameter2, rayAmount2, 0), Vector3.left, rayLength);
        //fHitR = Physics2D.Raycast(transform.position - new Vector3(-rayParameter2, rayAmount2, 0), Vector3.right, rayLength);

        //Debug.DrawRay(transform.position - new Vector3(rayParameter2, rayAmount2, 0), Vector3.left * rayLength, Color.red, 0.1f);
        //Debug.DrawRay(transform.position - new Vector3(-rayParameter2, rayAmount2, 0), Vector3.right * rayLength, Color.red, 0.1f);

        //float moves = Input.GetAxis("Horizontal");          //moves 변수에 GetAxis("Horizontal"(왼쪽 방향일때 -1, 오른쪽일때 1 반환))를 통하여 -1 또는 1로 방향 나타냄

        Vector3 dir = Vector3.zero;                         //방향을 넣을 dir을 넣고 0으로 초기화

        if (Input.GetAxisRaw("Horizontal") < 0)             //방향입력이 0보다 작을때(-1일때, 즉 왼쪽일때)
        {
            dir = Vector3.left;                             //왼쪽 방향을 넣어줌
            renderer.flipX = false;                         //flipX를 꺼서 방향은 그대로
            //anim.SetBool("isWalk", true);
            pLeft = true;                                   //플레이어가 왼쪽인지 판별하는 bool 함수에 true 넣어줌
            bulletP = -bulletPos;                           //총알위치를 -로 넣어 왼쪽으로 향하게 함
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)        //방향입력이 0보다 클때
        {
            dir = Vector3.right;                            //오른쪽 방향
            renderer.flipX = true;                          //flipX를 켜서 보는 방향을 바꿈
            //anim.SetBool("isWalk", true);
            pLeft = false;                                  //오른쪽 판별을 위해 끔
            bulletP = bulletPos;                            //총알 위치 그대로하여 오른쪽으로 향하게 함
        }

        else if (Input.GetAxisRaw("Horizontal") == 0)       //방향 입력이 없을 때
        {
            //anim.SetBool("isWalk", false);
        }
        
        moveAmount = dir * moveSpeed * Time.deltaTime;      //움직이는양 = 방향 * 속도 * 시간
        //if (fHitL.collider == null && fHitR.collider == null)
        //{
        //transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        transform.Translate(moveAmount);                    //플레이어의 위치(transform) = 위치(transform) + 움직이는 양(moveAmount)
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
