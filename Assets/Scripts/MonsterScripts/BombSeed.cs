using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BombSeed : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    public float Speed = 150f;

    public Transform player;
    public GameObject hpBar;

    private Transform transform;
    private Rigidbody2D rigid;
    private CircleCollider2D coll;

    private int statement = 0;  // 0: Idle, 1: Chase, 2: Attack
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
    }

    // Update is called once per frame
    void Update()
    {
        if(coll.enabled == false) { // 두1질때 콜라이더 꺼지니까 그거 체크해서 스크립트 끄기
            rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            gameObject.GetComponent<BombSeed>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if(statement == 0 && canbomb) {
            Move_Idle();
        }
        else if(statement == 1 && canbomb) {
            Move_Chase();
        }
        else if(statement == 2 && canbomb) {
            Attack();
        }
    }

    void OnDestroy()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.tag == "Player") {// 트리거에 Player가 닿으면
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
        if(collision.gameObject.tag == "Player") {// 콜라이더에 Player닿으면
            statement = 2;  // Attack상태
        }
        // 총알에 닿으면(맞으면) Chase상태로 변경
        if(collision.gameObject.tag == "Bullet") {
            statement = 1;  
        }
        // 발판에 닿으면 통과하게 trigger상태로 바꿔줌
        if(collision.gameObject.tag == "Floor") {
            coll.isTrigger = true;
        }
    }


    void Move_Idle()    // 코루틴 말고 다른 방법(y좌표 정해줘서 그 범위 벗어나면 udstate변경)으로 수정해보기
    {
        float moveAmount = 0;
        float dirY = setDirY - transform.position.y;

        if(udstate == 0) {
            moveAmount = Speed * Time.deltaTime;
        }
        else if(udstate == 1) {
            moveAmount = -Speed * 0.3f * Time.deltaTime;
        }

        if(dirY <= 0) {  // 원래 있던 위치보다 높이 올라가려고 하면 올라가지않게하기
            return;
        }
        rigid.velocity = new Vector2(rigid.velocity.x, moveAmount);
    }
    IEnumerator UDChange()
    {
        if(statement != 0) {
            yield break;
        }

        if (udstate == 1) {
            udstate = 0;
        }
        else if(udstate == 0) {
            udstate = 1;
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("UDChange");
    }

    private void Move_Chase()
    {
        float dirX = player.position.x - transform.position.x;
        float dirY = player.position.y - transform.position.y + 2f;
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

// 애니메이션, 데미지입히기 넣기
    void Attack()
    {
        canbomb = false;
        // 제자리고정
        rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        Debug.Log("Attack");
        Invoke("AttackDestroy", 2);
    }
    // 공격 후 destroy
    void AttackDestroy()
    {
        Destroy(gameObject);
        Destroy(hpBar);
    }
}
