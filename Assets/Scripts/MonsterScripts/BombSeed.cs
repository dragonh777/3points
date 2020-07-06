using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombSeed : MonoBehaviour
{
    public Image HPBar;
    public Canvas HPCanvas;

    private Transform _playerTransform;
    private Rigidbody2D _rigid;
    private CircleCollider2D _cirColl;
    private Animator _animator;
    private GameObject effect;

    private bool isPop = false;
    public int statement = 0;  // 0: idle, 1: chase, 2: hit, 3: pop, 4: die
    public float HP = 100.0f;
    private float currentHP;
    private float maxHP;
    private bool hitFlag = false;   // t: 쳐맞음, f: 안맞음

    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
        maxHP = HP;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _rigid = GetComponent<Rigidbody2D>();
        _cirColl = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        effect = gameObject.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // 맞을 때
        if (currentHP != HP)
        {
            currentHP = HP;
            HPBar.fillAmount = HP / maxHP;
            statement = 2;
        }
        // 죽을 때
        if (HP <= 0)
        {
            statement = 4;
        }
    }

    void FixedUpdate()
    {
        if (statement == 0 && !hitFlag)  // idle
        {
            _animator.Play("bomb_idle");
        }
        else if (statement == 1 && !hitFlag) // chase
        {
            Chase();
        }
        else if (statement == 2 && !hitFlag) // hit
        {
            hitFlag = true;
            Hit();
        }
        else if (statement == 3) // pop
        {
            Pop();
        }
        else if (statement == 4) // die 
        {
            Die();
        }
    }

    void Chase()
    {
        Vector3 target = new Vector3(_playerTransform.position.x, _playerTransform.position.y);
        transform.position = Vector3.MoveTowards(transform.position, target, 0.1f);

        _animator.Play("bomb_idle");
    }

    void Hit()
    {
        _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        _animator.Play("bomb_hit");
        HP -= 10f;
    }

    void AfterHit()
    {
        _rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        hitFlag = false;
        statement = 1;
    }

    void Pop()
    {
        _rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        _cirColl.enabled = false;
        isPop = true;
        _animator.Play("bomb_pop");
        Invoke("AfterPop", 0.38f);
    }

    void AfterPop()
    {
        effect.SetActive(true);
    }

    void Die()
    {
        _animator.Play("bomb_die");
        _cirColl.enabled = false;
    }

    public void AfterDeath()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {

        if (collision.gameObject.tag == "Bullet" && !isPop)   // 총알에 맞으면
        {
            statement = 2;  // hit
        }
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Siege_Enemy")    // 캐릭터와 충돌하면
        {
            statement = 3;  // pop
        }
    }

}
