using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveTest : MonoBehaviour
{

    public float movePower = 6f;    // 좌우 움직이는 힘
    public float jumpPower = 14f;    // 점프 할 때 주는 힘
    public float dashPower = 1f;    // 대쉬 할 때 주는 힘
    public float maxDashTime = 1.0f;    // 대쉬하는시간
    public float dashStoppingTime = 0.1f;   // 대쉬 멈추는 속도

    public int jumpCount = 2;   // 점프카운트(몇단점프인지)

    private float currentDashTime;  // 현재 대쉬시간

    bool isFloor = false;   // true: 땅에 닿음, false: 공중에 떠있음
    bool isJump = false;    // 점프 확인용 true: 점프중, false: 점프안하는중
    bool isDash = false;    // 대쉬 확인용 true: 대쉬중, false: 대쉬안하는중
    bool dashCheck = true; // 대쉬 반복금지true: 대쉬가능, false: 대쉬금지

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
        if (Input.GetKeyDown(KeyCode.W)) {   // 점프키 W키 누르면 isJump 참
            isJump = true;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {   // 대쉬키 SpaceBar
            if (dashCheck) {
                isDash = true;
            }
        }
        if (isFloor) {  // 땅에 닿으면 대쉬 가능
            dashCheck = true;
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
            isFloor = true; // Floor태그 가진놈 닿으면 true
            jumpCount = 2;  // Floor에 발 닿으면 점프카운트 2로 초기화
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    void Move()
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

            isFloor = false;
            jumpCount--;    // 점프카운트 감소
        }
    }

    //IEnumerator CoroutineName()
    //{
    //    //할일
    //    yield return new WaitForSeconds(2f);
    //}

    //StartCoroutine("CoroutineName");

    void Dash()
    {
        rigid.velocity = Vector2.zero;
        currentDashTime = 0.0f;
        dashCheck = false;

        while (currentDashTime < maxDashTime) {
            if (Input.GetAxisRaw("Horizontal") < 0) {
                Vector2 dashVelocity = new Vector2(-dashPower, 0);
                rigid.AddForce(dashVelocity, ForceMode2D.Impulse);
                currentDashTime += dashStoppingTime;
            }
            else if (Input.GetAxisRaw("Horizontal") > 0) {
                Vector2 dashVelocity = new Vector2(dashPower, 0);
                rigid.AddForce(dashVelocity, ForceMode2D.Impulse);
                currentDashTime += dashStoppingTime;
            }
            else {
                break;
            }
        }
    }

}
