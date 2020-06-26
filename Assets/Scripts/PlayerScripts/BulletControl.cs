using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour
{
    public enum AnimState
    {
        AIR, DEL, HIT
    }

    public float bulletSpeed = 30f;
    public int bulletDamage = 35;

    public static int bDamage;

    [Header("References")]
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    private GameObject pdir;
    private float hitTime = 0f;
    private float delTime = 0f;
    private float fTime = 0f;


    private AnimState _AnimState;
    private string CurrentAnimation;

    bool left = false;
    bool isHit = false;
    bool isDel = false;

    // Start is called before the first frame update
    void Start()    // Bullet프리팹 생성시 초기화
    {
        fTime = 0f;
        bDamage = bulletDamage;
        pdir = GameObject.Find("Player");
        if (pdir.transform.localScale.x > 0f)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
            left = false;
        }
        else if (pdir.transform.localScale.x < 0f)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
            left = true;
        }
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

    private void SetCurrentAnimation(AnimState _state, bool loop)
    {
        _AsyncAnimation(AnimClip[(int)_state], loop, 1f);
    }

    private void Update()
    {
        if (fTime > 0.5f)
        {
            isDel = true;
        }
        if (!isHit && isDel)
        {
            delTime = 0f;
            _AnimState = AnimState.DEL;
            SetCurrentAnimation(_AnimState, false);

            Destroy(gameObject, 0.7f);
        }

        if (isHit && hitTime > 0.7f)
        {
            Destroy(gameObject);
        }

        Destroy(gameObject, 1.5f);
        hitTime += Time.deltaTime;
        delTime += Time.deltaTime;
        fTime += Time.deltaTime;
    }
    // Update is called once per frame
    void FixedUpdate()  // Bullet프리팹 생성 후 날아가게 하기
    {
        if (left)
        {
            GetComponent<Transform>().transform.Translate(new Vector3(-1, 0, 0) * bulletSpeed * Time.deltaTime);
        }
        else if (!left)
        {
            GetComponent<Transform>().transform.Translate(new Vector3(1, 0, 0) * bulletSpeed * Time.deltaTime);
        }

        //Destroy(gameObject, 5); // 5초 후 자동 Destroy
    }

    void OnCollisionEnter2D(Collision2D collision)  // 날아가다 충돌시
    {
        if (collision.gameObject.tag == "Wall"|| collision.gameObject.tag == "Floor")   // 태그 Wall이나 Floor에닿으면 Destroy
        {
            bulletSpeed = 0f;
            _AnimState = AnimState.HIT;
            SetCurrentAnimation(_AnimState, false);
            isHit = true;
            hitTime = 0f;
        }
        else if (collision.gameObject.tag == "Enemy")   // 태그 Enemy에 닿으면
        {
            bulletSpeed = 0f;
            Debug.Log("Hit");
            _AnimState = AnimState.HIT;
            SetCurrentAnimation(_AnimState, false);
            isHit = true;
            hitTime = 0f;
            EnemyMove.Hp -= bDamage;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")   // 태그 Wall이나 Floor에닿으면 Destroy
        {
            bulletSpeed = 0f;
            _AnimState = AnimState.HIT;
            SetCurrentAnimation(_AnimState, false);
            isHit = true;
            hitTime = 0f;
            //Destroy(gameObject, 0.7f);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")   // 태그 Wall이나 Floor에닿으면 Destroy
        {
            bulletSpeed = 0f;
            _AnimState = AnimState.HIT;
            SetCurrentAnimation(_AnimState, false);
            isHit = true;
            hitTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            bulletSpeed = 0f;
            _AnimState = AnimState.HIT;
            SetCurrentAnimation(_AnimState, false);
            isHit = true;
            hitTime = 0f;
        }
    }
}
