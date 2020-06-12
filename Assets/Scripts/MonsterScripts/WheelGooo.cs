using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

// Hit모션 딜레이(첫번째 때릴때 딜레이 있고 두번 째 때리면 딜레이없음)고치기
public class WheelGooo : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation; // skeletonAnimation 스크립트
    public AnimationReferenceAsset[] AnimClip;

    public float Speed = 100f;
    public Transform player;    // 플레이어 위치받기
    public GameObject wheelgooSprite;    // 휠구 스프라이트 받기
    public GameObject wheelgooGFX;  // 스켈레톤 데이터 받기
    public GameObject hitEffect;
    public Image HPBar;
    private Rigidbody2D rigid;
    private Transform transform;    // 자신 위치받기

    // 애니메이션
    public enum AnimState { hit, die };
    private AnimState _AnimState;
    private string currentAnimation;

    public static float HP = 100f;
    private float currentHP;
    private int statement = 0;  // 0: idle(GFX off Sprite On), 1: hit, 2: Die(GFX on Sprite Off)
    private bool doAnimation = false;

    bool hit = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        currentHP = HP;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0) {   // HP값이 0밑으로 떨어지면 Dead
            statement = 2;
            Dead();
        }
        if (HP != currentHP) {   // 총 HP와 현재 HP값이 차이가 난다면(맞았을 때)
            HPBar.fillAmount = HP / 100f;   // hp바 조정
            statement = 1;
        }

        if (hit && doAnimation == false) {
            HP -= 10;
            hit = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            Instantiate(hitEffect, transform.position + new Vector3(0, 1, 0), Quaternion.identity);

            hit = true;
        }
    }

    void FixedUpdate()
    {
        if (statement == 0) {   // idle
            Move();
        }
        else if(statement == 1) {   // hit
            Hit();
        }
        //else if(statement == 2) {   // die
        //    Dead();
        //}
    }

    void Move()
    {
        float moveAmount = 0;
        float dirX = player.position.x - transform.position.x;


        if (dirX > 0) {
            moveAmount = Speed * Time.deltaTime;
            wheelgooSprite.transform.Rotate(new Vector3(0, 0, -3)); // 오브젝트가 아닌 스프라이트 회전기키기 위함
        }
        else if (dirX < 0) {
            moveAmount = -Speed * Time.deltaTime;
            wheelgooSprite.transform.Rotate(new Vector3(0, 0, 3));
        }

        rigid.velocity = new Vector2(moveAmount, rigid.velocity.y);
    }

    void Hit()
    {
        if (doAnimation) {
            return;
        }
        doAnimation = true;
        currentHP = HP;
        //wheelgooSprite.transform.Rotate(new Vector3(0, 0, 0));  // 맞고 나서 어색해서 넣음

        wheelgooGFX.SetActive(true);
        wheelgooSprite.SetActive(false);

        //skeletonAnimation.state.SetAnimation(0, "vineball_hit", false).TimeScale = 1f;
        //skeletonAnimation.loop = false;
        //skeletonAnimation.timeScale = 1f;

        _AnimState = AnimState.hit;
        SetCurrentAnimation(_AnimState);

        rigid.constraints = RigidbodyConstraints2D.FreezeAll;
        
        Invoke("AfterHit", 0.5f);
    }
    void AfterHit()
    {
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        wheelgooGFX.SetActive(false);
        wheelgooSprite.SetActive(true);
        
        statement = 0;

        doAnimation = false;
    }

    void Dead()
    {
        _AnimState = AnimState.die;
        SetCurrentAnimation(_AnimState);

        rigid.constraints = RigidbodyConstraints2D.FreezeAll;

        Invoke("AfterDead", 0.5f);
    }
    void AfterDead()
    {
        Destroy(gameObject);    // 스크립트 들어있는 게임오브젝트삭제
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
