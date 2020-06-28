using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelGoo : MonoBehaviour
{
    public Image HPBar; // HP바 이미지
    public Canvas HPCanvas; // HP바 캔버스

    public float moveSpeed = 120.0f;

    private Transform _playerTransform; // 플레이어 트랜스폼
    private GameObject _sprite; // 휠구 스프라이트
    private GameObject _GFX;    // 휠구 애니메이션 그래픽
    private Animator _animator; // 휠구 애니메이터
    private Rigidbody2D _rigid; // 휠구 리지드바디

    public static float HP = 100.0f;
    private float currentHP;
    private int statement = 0;  // 0: wheel, 1: hit, 2: die
    private bool hitFlag = false;   // t: 맞는중, f: 안맞는중
    
    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _sprite = gameObject.transform.GetChild(1).gameObject;
        _GFX = gameObject.transform.GetChild(0).gameObject;
        _animator = _GFX.GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();

        _sprite.transform.Rotate(new Vector3(0, 0, 1));
    }

    // Update is called once per frame
    void Update()
    {
        //// 맞을 때
        //if (currentHP != HP)
        //{
        //    currentHP = HP;
        //    HPBar.fillAmount = HP / 100f;
        //}
        //// 죽을 때
        //if (HP <= 0)
        //{
        //    statement = 2;
        //}
    }

    void FixedUpdate()
    {
        //if (statement == 0 && !hitFlag) {   // wheel
        //    Move();
        //}
        //else if(statement == 1 && !hitFlag) {   // hit
        //    hitFlag = true;

        //    Hit();
        //}
        //else if(statement == 2) {   // die
        //    Die();
        //}
    }

    void Move()
    {
        float moveAmount = 0;
        float dirX = _playerTransform.position.x - transform.position.x;


        if (dirX > 0)
        {
            moveAmount = moveSpeed * Time.deltaTime;
            _sprite.transform.Rotate(new Vector3(0, 0, -3)); // 오브젝트가 아닌 스프라이트 회전기키기 위함
        }
        else if (dirX < 0)
        {
            moveAmount = -moveSpeed * Time.deltaTime;
            _sprite.transform.Rotate(new Vector3(0, 0, 3));
        }

        _rigid.velocity = new Vector2(moveAmount, _rigid.velocity.y);
    }

    void Hit()
    {
        _GFX.SetActive(true);   // 애니메이션 켜고
        _sprite.SetActive(false);   // 스프라이트 끄고

        HP -= 10.0f;
        _animator.Play("vineball_hit");
    }

    public void AfterHit()
    {
        _sprite.SetActive(true);    // 스프라이트 켜고
        _GFX.SetActive(false);  // 애니메이션 끄고

        hitFlag = false;
        statement = 0;
    }

    void Die()
    {
        _GFX.SetActive(true);   // 애니메이션 켜고
        _sprite.SetActive(false);   // 스프라이트 끄고

        _animator.Play("vineball_die");
    }

    public void AfterDeath()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet") {
            statement = 1;
        }
    }
}
