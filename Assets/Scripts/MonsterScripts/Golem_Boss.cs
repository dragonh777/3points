using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

public class Golem_Boss : MonoBehaviour
{
    [Serializable]
    public class Information {
        public string name;
        public Vector3 position;
        private float HP = 200f;
    }
    
    public AnimState _AnimState;
    public enum AnimState {
        Idle, Walk, Hit, Crash, OnRoll, Roll, EndRoll, Punch, Die
    }

    public AnimationReferenceAsset[] AnimClip;

    public SkeletonAnimation skeletonAnimation;
    private string currentAnimation;

    // Start is called before the first frame update
    void Start()
    {
        _AnimState = AnimState.Idle;
        SetCurrentAnimation(_AnimState);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void _AsyncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        //동일한 애니메이션을 재생하려고 한다면 아래 코드 구문 실행 x
        if (animClip.name.Equals(currentAnimation))
            return;

        //해당 애니메이션으로 변경한다.
        skeletonAnimation.state.SetAnimation(0, animClip, loop).TimeScale = timeScale;

        skeletonAnimation.loop = loop;
        skeletonAnimation.timeScale = timeScale;

        //현재 재생되고 있는 애니메이션 값을 변경
        currentAnimation = animClip.name;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        _AsyncAnimation(AnimClip[(int)_state], true, 1f);
    }

    private void SetCurrentAnimation(AnimState _state, bool loop)
    {
        _AsyncAnimation(AnimClip[(int)_state], loop, 1f);
    }
}
