using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Bush : MonoBehaviour
{
    public enum AnimState
    {
        IDLE, TOUCH
    }

    private AnimState _AnimState;

    private string CurrentAnimation;

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    bool objectOn = false;
    float tchTime;

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
        if (collision.gameObject.tag == "Player")
        {
            objectOn = true;
            tchTime = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            objectOn = true;
            tchTime = 0f;

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tchTime += Time.deltaTime;
        if (objectOn)
        {
            if (tchTime < 0.7f)
            {
                _AnimState = AnimState.TOUCH;
                SetCurrentAnimation(_AnimState, false);
            }
            else if (tchTime > 0.7f)
            {
                objectOn = false;
                _AnimState = AnimState.IDLE;
                SetCurrentAnimation(_AnimState);
            }
        }
        else
            SetCurrentAnimation(_AnimState);
    }
}
