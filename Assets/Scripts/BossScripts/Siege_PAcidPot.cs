using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Siege_PAcidPot : MonoBehaviour
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
    
    // Start is called before the first frame update
    void Start()
    {
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _capColl = GetComponent<CapsuleCollider2D>();
        _boxColl = GetComponent<BoxCollider2D>();
        _rigid = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        Invoke("SpawnDestroy", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if(statement != 4) {
            Attack();
        }
        else if(statement == 4) {
            _animator.Play("die");
            _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
            _capColl.enabled = false;
            _boxColl.enabled = false;
        }

        if (!_playerTransform.gameObject.activeSelf) {
            statement = 4;
        }
    }

    void SpawnDestroy()
    {
        statement = 4;
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
        acidBullet.gameObject.tag = "Siege_Bullet";
        acidBullet.transform.parent = gameObject.transform;
        //if(attackPosition) {
        //    acidBullet.transform.localPosition = new Vector3(transform.position.x + 0.92f, transform.position.y + 1.194f);  
        //    acidBullet.transform.localScale = new Vector3(0.2f, 0.2f);
        //}
        //else if(!attackPosition) {
        //    acidBullet.transform.localPosition = new Vector3(transform.position.x - 0.92f, transform.position.y + 1.194f);
        //    acidBullet.transform.localScale = new Vector3(-0.2f, 0.2f);
        //}
        acidBullet.transform.localPosition = new Vector3(-5f, 5.39f);
        acidBullet.transform.localScale = new Vector3(-1f, 1f);
    }
    void Die()
    {
        Destroy(this.gameObject);
    }


    void Idle()
    {
    }
    void Walk()
    {
    }
    void Hit()
    {
    }
    void AfterHit()
    {
    }
    void IdleWalkStateChange()  // idle, walk상태 결정하기
    {
    }
    void WalkStateChange()  // left, rightwalk 결정하기
    {
    }
    void StatementChange(int index)
    {
    }
}
