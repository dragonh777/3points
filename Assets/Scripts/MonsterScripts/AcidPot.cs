using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class AcidPot : MonoBehaviour
{

    public SkeletonAnimation skeletonAnimation; // skeletonAnimation 스크립트
    public AnimationReferenceAsset[] AnimClip;
    public Transform player;    // 플레이어 위치받기
    public Image HPBar; // HP바 받기
    public Canvas HPCanvas;
    private Rigidbody2D rigid;

    // 움직임 관련
    public float moveSpeed = 1f;
    public int statement = 0;  // 0: idle, 1: walk, 2: attack, 3: hit, 4: die
    private int walkState = 0;  // 0: Left Walk, 1: Right Walk;
    public bool isCollide = false;    // 공격 범위 내에 들어오면 true
    public bool isHit = false;  // 맞으면 true

    // 애니메이션 관련
    public enum AnimState { idle, walk, attack, hit, die };
    private AnimState _AnimState;
    private string CurrentAnimation;

    // 체력바 관련
    public static float HP = 100.0f;
    private float currentHP;

    private int statementChangeCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
        StatementChange();
        statementChangeCount++;
        rigid = GetComponent<Rigidbody2D>();
        HPCanvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 저장된 HP가 현재 HP와 다를 때(맞았을 때)
        if (currentHP != HP) {
            currentHP = HP; // 저장된 HP에 현재 HP넣고
            HPBar.fillAmount = HP / 100f;   // hp바 조정
            statement = 3;  // 상태 hit로 변경
        }
        // HP가 다 떨어지면(죽으면)
        if (HP <= 0) {
            statement = 4;  // 상태 die로 변경
        }
        if (!isHit && Input.GetKeyDown(KeyCode.X)) {    // 맞는모션중엔 무적
            HP -= 10;
        }
        Debug.Log("StatementCount: " + statementChangeCount);

        SetCurrentAnimation(_AnimState);
    }

    void FixedUpdate()
    {
        if (statement == 0) {    // idle
            Idle();
        }
        else if (statement == 1) {    // walk
            Walk();
        }
        else if (statement == 2) {   // attack
            Attack();
        }
        else if (statement == 3) {   // hit
            Hit();
        }
        else if (statement == 4) {   // die
            Die();
        }
    }

    // Idle, Walk 상태 결정 함수
    void StatementChange()
    {
        // idle, walk상태가 아니라면
        if(statement != 0 && statement != 1) {
            return; // 그냥 리턴
        }
        if(isCollide) {
            return;
        }

        statement = Random.Range(0, 2);   // 0~1사이 int값 랜덤 발생

        if(statement == 1) {    // statement가 walk라면
            walkState = Random.Range(0, 2); // left walk인지 right walk인지 결정
        }

        Invoke("StatementChange", 1.5f);    // 1.5초 후 함수 재실행
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {// 트리거에 Player가 닿으면
            isCollide = true;
            statement = 2;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
       if(collision.gameObject.tag == "Player") {
            isCollide = true;
            statement = 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && isCollide) {  // 트리거에서 player가 벗어나면
            isCollide = false;
            statement = 0;
            statementChangeCount++;
            StatementChange();
        }
    }

    void Idle()
    {
        _AnimState = AnimState.idle;
    }

    void Walk()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(walkState == 0) {    // 왼쪽으로 걸어갈 때
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(1, 1, 1);
            HPCanvas.transform.localScale = new Vector3(0.00926f, 0.00926f, 0.00926f);
            _AnimState = AnimState.walk;
        }
        else if(walkState == 1) {   // 오른쪽으로 걸어갈 때
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-1, 1, 1);
            HPCanvas.transform.localScale = new Vector3(-0.00926f, 0.00926f, 0.00926f);
            _AnimState = AnimState.walk;
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }

    void Attack()
    {
        float dirX = player.position.x - transform.position.x;

        if(dirX > 0) {  // 플레이어가 왼쪽이면
            transform.localScale = new Vector3(-1, 1, 1);    // 왼쪽보고
            HPCanvas.transform.localScale = new Vector3(-0.00926f, 0.00926f, 0.00926f);
        }
        else if(dirX < 0) { // 플레이어가 오른쪽이면
            transform.localScale = new Vector3(1, 1, 1);   // 오른쪽보고
            HPCanvas.transform.localScale = new Vector3(0.00926f, 0.00926f, 0.00926f);
        }

        _AnimState = AnimState.attack;  // 애니메이션 변경
    }

    void Hit()
    {
        isHit = true;
        _AnimState = AnimState.hit;
        Invoke("AfterHit", 0.5f);
    }
    void AfterHit()
    {
        if (isHit) {
            isHit = false;
            statement = 0;
            statementChangeCount++;
            StatementChange();
        }
    }

    void Die()
    {
        _AnimState = AnimState.die;
        Destroy(gameObject, 1.0f);
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

}
