using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 이 스크립트는 공통적으로 캐릭터에 들어가야하는 것들 구현해놓은 파일임
// 현재 구현된 기능
// 1. a: 왼쪽움직임, d: 오른쪽움직임   Move()
// 2. w: 점프, 2단점프 가능    Jump()
// 3. SpaceBar: 대쉬, 0.6초 딜레이(DashCoroutine()에서 수정가능)    Dash()

// 추가 할 기능
// 1. 애니메이션: Idls, Move, Jump, Dash 관련, 마우스따라 바라보기 애니메이션 추가(현재 단일스프라이트로 좌우 플립만 가능)
// 2. 아이템 사용: 퀵슬롯
// 3. 상태: 체력

public class PlayerMoveTest : MonoBehaviour
{

    public float movePower = 6f;    // 좌우 움직이는 힘
    public float jumpPower = 14f;    // 점프 할 때 주는 힘
    public float dashPower = 1f;    // 대쉬 할 때 주는 힘
    public float maxDashTime = 1.0f;    // 대쉬하는시간
    public float dashStoppingTime = 0.1f;   // 대쉬 멈추는 속도

    public int jumpCount = 2;   // 점프카운트(몇단점프인지)

    private float currentDashTime;  // 현재 대쉬시간

    //bool isFloor = false;   // true: 땅에 닿음, false: 공중에 떠있음
    bool isJump = false;    // 점프 확인용 true: 점프함수실행가능, false: 점프함수실행불가
    bool isDash = false;    // 대쉬 확인용 true: 대쉬함수실행가능, false: 대쉬함수실행불가
    bool dashCheck = true; // 대쉬 딜레이체크용 true: 대쉬가능, false: 대쉬금지

    Rigidbody2D rigid;
    SpriteRenderer renderer;

    Vector3 movement;   // 좌우이동시 Vector3

    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();

        jumpCount = 0;  // 점프카운트 초기화
        currentDashTime = 0.0f; // 현재 대쉬시간 초기화
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) {   // 점프키 W키 누르면 isJump 참
            isJump = true;
        }

        if (Input.GetButtonDown("Dash") && dashCheck) {   // 대쉬키 SpaceBar
            isDash = true;
        }
    }

    void FixedUpdate()
    {
        Move();

        if (isJump) {   // isJump값 참이면 Jump함수 실행
            isJump = false; // 이거 안하면 3번눌렀을때 2단점프 하고 자동으로 점프함
            Jump();
        }

        if (isDash) {
            isDash = false;
            Dash();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // boxCollider2D 트리거 충돌(플레이어 발바닥 확인)
    {
        if (collision.gameObject.tag == "Floor") {
            //isFloor = true; // Floor태그 가진놈 닿으면 true
            jumpCount = 2;  // Floor에 발 닿으면 점프카운트 2로 초기화
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void Move() // transform.position으로 미끄러지지않고 부드럽게 이동하게 하기
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0) {    // 왼쪽 이동
            moveVelocity = Vector3.left;
            renderer.flipX = false; // 스프라이트 플립
        }
        else if (Input.GetAxisRaw("Horizontal") > 0) {   // 오른쪽 이동
            moveVelocity = Vector3.right;
            renderer.flipX = true;  // 스프라이트 플립
        }

        transform.position += moveVelocity * movePower * Time.deltaTime;    // 이동 힘주기
    }

    void Jump()
    {
        if (jumpCount > 0) { // 점프카운트 있을때
            rigid.velocity = Vector2.zero;

            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);

            //isFloor = false;
            jumpCount--;    // 점프카운트 감소
        }
    }

    void Dash() // 대쉬는 AddForce로 팍 밀기
    {
        rigid.velocity = Vector2.zero;
        currentDashTime = 0.0f;
        dashCheck = false;  // 대쉬 딜레이주기위함

        while (currentDashTime < maxDashTime) { // 정해진 시간값만큼 현재시간과 계산해 대쉬시간 적용
            if (Input.GetAxisRaw("Horizontal") < 0) {   // 왼쪽이동시
                Vector2 dashVelocity = new Vector2(-dashPower, 0);
                rigid.AddForce(dashVelocity, ForceMode2D.Impulse);
                currentDashTime += dashStoppingTime;    // 현재시간에 정해진 시간양만큼 계속 더해 대쉬지속
            }
            else if (Input.GetAxisRaw("Horizontal") > 0) {  // 오른쪽이동시
                Vector2 dashVelocity = new Vector2(dashPower, 0);
                rigid.AddForce(dashVelocity, ForceMode2D.Impulse);
                currentDashTime += dashStoppingTime;
            }
            else {  // 가만히있을때 대쉬 누르면 아무일도 없게한다, 없으면 무한반복돼서 ㅈ됨
                break;
            }
        }
        StartCoroutine("DashCoroutine");    // 딜레이적용
    }
    IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(0.6f);  // 현재 0.6초딜레이
        dashCheck = true;
    }

}
