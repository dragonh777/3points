using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcidPott : MonoBehaviour
{
    public Transform _playerTransform;
    public Image HPBar;
    public Canvas HPCanvas;
    public GameObject bullet;
    
    private Animator _animator;
    private BoxCollider2D _boxColl;
    private CapsuleCollider2D _capColl;
    private Rigidbody2D _rigid;

    private float moveSpeed = 1f;
    public int statement = 0;  // 0: idle, 1: walk, 2: attack, 3: hit, 4: die
    private int walkState = 0;  // 0: left walk, 1: right walk
    public bool isCollide = false; // t: collide, f: not collide
    private bool hitState = false;  // t: 맞고있을때, 무적, f: 평상시, 맞을수 있음

    public static bool attackPosition = false;    // t: 왼쪽공격, f: 오른쪽공격(총알에서 얻어옴)

    public float HP = 100.0f;
    private float currentHP;
    private float maxHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
        maxHP = HP;
        if (gameObject.tag == "Siege_Enemy") {   // 공성전 적 애시드팟일 때
            _playerTransform = GameObject.Find("Golem").GetComponent<Transform>();
        }
        else {
            _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        }
        _capColl = GetComponent<CapsuleCollider2D>();
        _boxColl = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.Play("idle");
    }

    // Update is called once per frame
    void Update()
    {
        // 맞을 때
        if(currentHP != HP) {
            currentHP = HP;
            HPBar.fillAmount = HP / maxHP;
            statement = 3;
        }
        // 죽을 때
        if(HP <= 0) {
            statement = 4;
        }

        // if(!hitState && Input.GetKeyDown(KeyCode.X)) {
        //     hitState = true;
        //     Hit();
        // }

    }

    void FixedUpdate() 
    {    
        if(statement == 0 && !isCollide && !hitState) {
            Idle();
        }
        else if(statement == 1 && !isCollide && !hitState) {
            Walk();
        }
        else if(statement == 2 && !hitState) {
            Attack();
        }
        else if(statement == 3 && !hitState) {
            hitState = true;
            Hit();
        }
        else if(statement == 4) {
            _animator.Play("die");
            _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            _capColl.enabled = false;
            _boxColl.enabled = false;
        }

    }

    void Idle()
    {
        Vector3 currentVector = transform.localScale;
        Vector3 currentCanvasVector = HPCanvas.transform.localScale;
        transform.localScale = currentVector;
        HPCanvas.transform.localScale = currentCanvasVector;
        _animator.Play("idle");
    }
    void Walk()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(walkState == 0) {    // 왼쪽으로 걸어갈 때
            moveVelocity = Vector3.left;
            transform.localScale = new Vector3(0.2f, 0.2f);
            HPCanvas.transform.localScale = new Vector3(0.0463f, 0.0463f);
        }
        else if(walkState == 1) {   // 오른쪽으로 걸어갈 때
            moveVelocity = Vector3.right;
            transform.localScale = new Vector3(-0.2f, 0.2f);
            HPCanvas.transform.localScale = new Vector3(-0.0463f, 0.0463f);
        }
        _animator.Play("walk");
        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }
    void Attack()
    {
       float dirX = _playerTransform.position.x - transform.position.x;

        if(dirX > 0) {  // 플레이어가 왼쪽이면
            transform.localScale = new Vector3(-0.2f, 0.2f);    // 왼쪽보고
            HPCanvas.transform.localScale = new Vector3(-0.0463f, 0.0463f);
            attackPosition = true;
        }
        else if(dirX < 0) { // 플레이어가 오른쪽이면
            transform.localScale = new Vector3(0.2f, 0.2f);   // 오른쪽보고
            HPCanvas.transform.localScale = new Vector3(0.0463f, 0.0463f);
            attackPosition = false;
        }
        _animator.Play("attack");
    }

    void Shoot()
    {
        GameObject acidBullet = Instantiate(bullet);
        //acidBullet.transform.parent = gameObject.transform;
        if(attackPosition) {
            acidBullet.transform.localPosition = new Vector3(transform.position.x + 0.92f, transform.position.y + 1.194f);  
            acidBullet.transform.localScale = new Vector3(0.2f, 0.2f);
        }
        else if(!attackPosition) {
            acidBullet.transform.localPosition = new Vector3(transform.position.x - 0.92f, transform.position.y + 1.194f);
            acidBullet.transform.localScale = new Vector3(-0.2f, 0.2f);
        }
    }

    void Hit()
    {
        _animator.Play("hit");
        HP -= 10f;
    }
    void AfterHit()
    {
        hitState = false;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void IdleWalkStateChange()  // idle, walk상태 결정하기
    {
        statement = Random.Range(0, 2); // 0~1사이 랜덤값 statement로 지정
    }
    void WalkStateChange()  // left, rightwalk 결정하기
    {
        walkState = Random.Range(0, 2);
    }

    void StatementChange(int index)
    {
        statement = index;
        hitState = false;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            statement = 3;
        }
    }
}
