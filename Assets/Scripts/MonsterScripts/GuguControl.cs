using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class GuguControl : MonoBehaviour
{
    public enum AnimState
    {
        IDLE, HIT, DES
    }

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    public float HP;

    private AnimState _AnimState;

    private string CurrentAnimation;

    private float desTime = 0f;
    private float hitTime = 0f;
    private bool isHit = false;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Bullet")
    //    {
    //        HP -= 10f;

    //    }
    //}

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && HP > 0)
        {
            HP -= 10f;
            _AnimState = AnimState.HIT;
            SetCurrentAnimation(_AnimState, false);
            isHit = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            hitTime += Time.deltaTime;
            if (hitTime > 0.15f)
            {
                hitTime = 0f;
                isHit = false;
                _AnimState = AnimState.IDLE;
                SetCurrentAnimation(_AnimState, false);
            }
        }
        if (HP <= 0)
        {
            desTime += Time.deltaTime;
            _AnimState = AnimState.DES;
            SetCurrentAnimation(_AnimState, false);
        }


        if (desTime > 1.2f)
        {
            Destroy(gameObject);
        }

        
    }
}
