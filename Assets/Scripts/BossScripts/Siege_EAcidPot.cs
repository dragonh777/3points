using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Siege_EAcidPot : MonoBehaviour
{
    public Transform _playerTransform;
    public Image HPBar;
    public Canvas HPCanvas;
    public GameObject bullet;
    //public GameObject gameover;
    
    private Animator _animator;
    private BoxCollider2D _boxColl;
    private CapsuleCollider2D _capColl;
    private Rigidbody2D _rigid;

    private float moveSpeed = 1f;
    public int statement = 0;  // 0: idle, 1: walk, 2: attack, 3: hit, 4: die
    private int walkState = 0;  // 0: left walk, 1: right walk
    public bool isCollide = false; // t: collide, f: not collide
    private bool hitState = false;  // t: 맞고있을때, 무적, f: 평상시, 맞을수 있음

    public bool attackPosition = false;    // t: 왼쪽공격, f: 오른쪽공격(총알에서 얻어옴)

    public float HP = 100.0f;
    private float currentHP;
    private float maxHP;

    public bool miniAcid = false;  // t: 작은애들
    
    // Start is called before the first frame update
    void Start()
    {
        maxHP = currentHP = HP;
        _playerTransform = GameObject.Find("Golem").GetComponent<Transform>();
        _capColl = GetComponent<CapsuleCollider2D>();
        _boxColl = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.Play("idle");
        
        if(gameObject.name.Equals("EnemyAcid")) {
            miniAcid = true;
            maxHP = currentHP = HP = 30f;
        }
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
        // if(statement == 0 && !isCollide && !hitState) {
        //     Idle();
        // }
        if( !isCollide && !hitState) {
            Walk();
        }
        else if(isCollide && !hitState) {
            Attack();
        }
        else if(statement == 3) {
            _animator.Play("hit");
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
        float dirX = _playerTransform.position.x - transform.position.x;

        if(dirX < 0) {    // 왼쪽으로 걸어갈 때
            moveVelocity = Vector3.left;
            if(miniAcid) {
                transform.localScale = new Vector3(0.2f, 0.2f);
            }
            else {
                transform.localScale = new Vector3(0.4f, 0.4f);
            }
            HPCanvas.transform.localScale = new Vector3(0.0463f, 0.0463f);
        }
        else if(dirX > 0) {   // 오른쪽으로 걸어갈 때
            moveVelocity = Vector3.right;
            if(miniAcid) {
                transform.localScale = new Vector3(-0.2f, 0.2f);
            }
            else {
                transform.localScale = new Vector3(-0.4f, 0.4f);
            }
            HPCanvas.transform.localScale = new Vector3(-0.0463f, 0.0463f);
        }
        _animator.Play("walk");
        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }
    void Attack()
    {
        float dirX = _playerTransform.position.x - transform.position.x;

        if(dirX > 0) {  // 플레이어가 왼쪽이면
            if(miniAcid) {
                transform.localScale = new Vector3(-0.2f, 0.2f);    // 왼쪽보고
            }
            else {
                transform.localScale = new Vector3(-0.4f, 0.4f);    // 왼쪽보고
            }
            HPCanvas.transform.localScale = new Vector3(-0.0463f, 0.0463f);
            attackPosition = true;
        }
        else if(dirX < 0) { // 플레이어가 오른쪽이면
            if (miniAcid) {
                transform.localScale = new Vector3(0.2f, 0.2f);   // 오른쪽보고
            }
            else {
                transform.localScale = new Vector3(0.4f, 0.4f);   // 오른쪽보고
            }
            HPCanvas.transform.localScale = new Vector3(0.0463f, 0.0463f);
            attackPosition = false;
        }
        _animator.Play("attack");
    }

    void Shoot()
    {
        GameObject acidBullet = Instantiate(bullet);
        acidBullet.transform.parent = gameObject.transform;
        //if(attackPosition) {
        //    acidBullet.transform.localPosition = new Vector3(transform.position.x + 1.75f, transform.position.y + 2.2f); 
        //    if(miniAcid) {
        //        acidBullet.transform.localScale = new Vector3(0.2f, 0.2f);
        //    }
        //    else {
        //        acidBullet.transform.localScale = new Vector3(0.4f, 0.4f);
        //    }
        //}
        //else if(!attackPosition) {
        //    if (miniAcid) {
        //        acidBullet.transform.localScale = new Vector3(-0.2f, 0.2f);
        //    }
        //    else {
        //        acidBullet.transform.localScale = new Vector3(-0.4f, 0.4f);
        //    }
        //    acidBullet.transform.localPosition = new Vector3(transform.position.x - 1.75f, transform.position.y + 2.2f);
        //}

        //-6.3  5.39
        //if(miniAcid) {
        //    if (attackPosition) {
        //        acidBullet.transform.localPosition = new Vector3(-5.77f, 5.27f);
        //        acidBullet.transform.localScale = new Vector3(-1f, 1f);
        //    }
        //    else {
        //        acidBullet.transform.localPosition = new Vector3(-5.77f, 5.27f);
        //        acidBullet.transform.localScale = new Vector3(-1f, 1f);
        //    }
        //}
        //else {
            //if (attackPosition) {
            //    acidBullet.transform.localPosition = new Vector3(-5f, 5.39f);
            //    acidBullet.transform.localScale = new Vector3(-1f, 1f);
            //}
            //else {
                acidBullet.transform.localPosition = new Vector3(-5f, 5.39f);
                acidBullet.transform.localScale = new Vector3(-1f, 1f);
            //}
        //}
        
    }

    void Hit()
    {
        hitState = true;
        HP -= 10f;

        Invoke("AfterHit", 0.15f);
    }
    void AfterHit()
    {

    }

    void Die()
    {
        //Destroy(this.gameObject);
        //gameover.SetActive(true);
        gameObject.SetActive(false);
    }

    void StatementChange(int index)
    {
        statement = index;
        hitState = false;
    }

    void IdleWalkStateChange()  // idle, walk상태 결정하기
    {
    }
    void WalkStateChange()  // left, rightwalk 결정하기
    {
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Siege_Bullet") {
            Hit();
        }
    }
}
