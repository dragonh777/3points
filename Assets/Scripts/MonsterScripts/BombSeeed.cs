using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

// !!!!!!!!!!!!!!!!!!!! Hit모션이 선딜있어서 애니메이션 수정하던지 스켈레톤데이터 새로 받는게 나을듯 !!!!!!!!!!!!!!!!!!!!!!!1
public class BombSeeed : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation; // skeletonAnimation 스크립트
    public AnimationReferenceAsset[] AnimClip;
    public Transform player;    // 플레이어 위치받기
    public Image HPBar; // HP바 받기
    //public GameObject hpBar;

    private Transform transform;
    private Rigidbody2D rigid;
    private CircleCollider2D coll;

    // 애니메이션 관련
    public enum AnimState { idle, hit, die };
    private AnimState _AnimState;
    private string currentAnimation;
    private bool isDead = false; // 체력이 다 닳으면 true, 죽는모션 후 오브젝트 destroy

    // 기능관련
    public float Speed = 150f;
    public static float HP = 100.0f;    // 체력, hp바에서 참조함(MonsterHPCtrl.cs)
    private float currentHP;
    private int statement = 0;  // 0: Idle, 1: Chase, 2: Attack, 3: Hit, 4: Die
    private int udstate = 0;    // 0: UP, 1: DOWN
    private float setDirY;
    private bool canbomb = true;    // true: can bomb, false: can't bomb

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        coll = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        setDirY = transform.position.y;// 기본좌표따기
        StartCoroutine("UDChange"); // Idle상태일 때 위아래 상태변경

        // 애니메이션 첫 설정(idle)
        _AnimState = AnimState.idle;
        SetCurrentAnimation(_AnimState);

        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        //if (coll.enabled == false) { // 두1질때 콜라이더 꺼지니까 그거 체크해서 스크립트 끄기
        //    rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        //    gameObject.GetComponent<BombSeed>().enabled = false;
        //}
        if (HP <= 0) {   // HP값이 0밑으로 떨어지면 Dead
            isDead = true;
            Attack();
        }
        if (HP != currentHP) {   // 총 HP와 현재 HP값이 차이가 난다면(맞았을 때)
            HPBar.fillAmount = HP / 100f;   // hp바 조정
            statement = 3;
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            HP -= 10;
        }

    }

    void FixedUpdate()
    {
        if (statement == 0 && canbomb) {    // statement 0:idle, 1:chase, 2:attack, 3:hit, 4:die
            Move_Idle();
        }
        else if (statement == 1 && canbomb) {
            Move_Chase();
        }
        else if (statement == 2 && canbomb) {
            Attack();
        }
        else if(statement == 3 && canbomb) {
            Hit();
        }
    }

    void OnDestroy()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {// 트리거에 Player가 닿으면
            statement = 1;  // Chase상태
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 발판에서 벗어나면 다시 collider를 트리거상태 false
        if (collision.gameObject.tag == "Floor") {
            coll.isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {// 콜라이더에 Player닿으면
            statement = 2;  // Attack상태
        }
        // 총알에 닿으면(맞으면) Chase상태로 변경
        if (collision.gameObject.tag == "Bullet") {
            statement = 1;
        }
        // 발판에 닿으면 통과하게 trigger상태로 바꿔줌
        if (collision.gameObject.tag == "Floor") {
            coll.isTrigger = true;
        }
    }


    void Move_Idle()    // 코루틴 말고 다른 방법(y좌표 정해줘서 그 범위 벗어나면 udstate변경)으로 수정해보기
    {
        float moveAmount = 0;
        float dirY = setDirY - transform.position.y;

        if (udstate == 0) {
            moveAmount = Speed * Time.deltaTime;
        }
        else if (udstate == 1) {
            moveAmount = -Speed * 0.3f * Time.deltaTime;
        }

        if (dirY <= 0) {  // 원래 있던 위치보다 높이 올라가려고 하면 올라가지않게하기
            return;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, moveAmount);
    }
    IEnumerator UDChange()  // UpDown Change
    {
        if (statement != 0) {
            yield break;
        }

        if (udstate == 1) { // udstate == 1 -> Down
            udstate = 0;
        }
        else if (udstate == 0) {    // udstate == 0 -> Up
            udstate = 1;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("UDChange");
    }

    private void Move_Chase()
    {
        float dirX = player.position.x - transform.position.x;
        float dirY = player.position.y - transform.position.y + 1f;
        float moveAmountX = 0, moveAmountY = 0;
        if (dirX > 0) {
            moveAmountX = Speed * Time.deltaTime;
        }
        else if (dirX < 0) {
            moveAmountX = -Speed * Time.deltaTime;
        }

        if (dirY > 0) {
            moveAmountY = Speed * Time.deltaTime;
        }
        if (dirY < 0) {
            moveAmountY = -Speed * Time.deltaTime;
        }

        rigid.velocity = new Vector2(moveAmountX, moveAmountY);
    }

    // 죽었을 때, 공격할 때 모션 같으므로 같은 Attack()호출했음
    void Attack()
    {
        canbomb = false;
        // 제자리고정
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        // 애니메이션
        _AnimState = AnimState.die;
        SetCurrentAnimation(_AnimState);

        Debug.Log("Attack");
        Invoke("AttackDestroy", 0.85f);
    }
    // 공격 후 destroy
    void AttackDestroy()
    {
        //if(!isDead) { // HP가 다 닳아서 죽은게 아니라면 데미지넣기
        
        //}

        Destroy(gameObject);
        //Destroy(hpBar);
    }

    void Hit()
    {
        currentHP = HP; // currentHP값 다시 맞춰주고
        // 애니메이션 변경(캐릭터가 쏘는 방향에 따라 좌우반전 나중에 추가할것!!!!)
        skeletonAnimation.state.SetAnimation(0, "bomb_hit", false).TimeScale = 1f;
        skeletonAnimation.loop = false;
        skeletonAnimation.timeScale = 1f;

        statement = 1;  // Chase상태로 변경
    }


    private void SetCurrentAnimation(AnimState _state)
    {
        _AsyncAnimation(AnimClip[(int)_state], true, 1f);
    }
    private void _AsyncAnimation(AnimationReferenceAsset animCip, bool loop, float timeScale)
    {
        //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 x
        if (animCip.name.Equals(currentAnimation))
            return;

        //해당 애니메이션으로 변경한다.
        skeletonAnimation.state.SetAnimation(0, animCip, loop).TimeScale = timeScale;
        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //현재 재생되고 있는 애니메이션 값을 변경
        currentAnimation = animCip.name;
    }
}
