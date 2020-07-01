using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Spine;
using Spine.Unity;
using UnityEngine;

public class DustEffect : MonoBehaviour
{

    public enum AnimState
    {
        BREAK, SLAND, MLAND, LLAND
    }

    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    private AnimState _AnimState;

    private string CurrentAnimation;

    private GameObject pdir;
    private float desTime = 0f;

    private bool isBreak = false;
    private bool isSLand = false;
    private bool isMLand = false;
    public static bool isLLand = false;

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

    // Start is called before the first frame update
    void Start()
    {

        pdir = GameObject.Find("Player");
        transform.position = pdir.transform.position;

        if (pdir.transform.localScale.x > 0f)
        {
            if (Players.isGround && Players.isMove)
                transform.localScale = new Vector3(-0.5f, 0.5f, 1);
            else
                transform.localScale = new Vector3(0.5f, 0.5f, 1);

        }
        else if (pdir.transform.localScale.x < 0f)
        {
            if (Players.isGround && Players.isMove)
                transform.localScale = new Vector3(0.5f, 0.5f, 1);
            else
                transform.localScale = new Vector3(-0.5f, 0.5f, 1);

        }
    }

    // Update is called once per frame
    void Main()
    {
        if (Players.isGround && Players.isMove)
        {
            _AnimState = AnimState.BREAK;
            isBreak = true;
        }

        else if (isLLand)
            _AnimState = AnimState.LLAND;

        else if (Players.isLand && Players.isMove && !isLLand)
        {
            _AnimState = AnimState.SLAND;
            isSLand = true;
        }

        else if (Players.isLand && !Players.isMove && !isLLand)
        {
            _AnimState = AnimState.MLAND;
            isMLand = true;
        }



        SetCurrentAnimation(_AnimState, false);



        if (isBreak || isSLand)
            Destroy(gameObject, 0.3f);
        else if (isMLand)
            Destroy(gameObject, 0.5f);
        else if (isLLand)
        {
            Destroy(gameObject, 1.3f);
        }

        isLLand = false;
    }
}
