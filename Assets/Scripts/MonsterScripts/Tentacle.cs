using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

public class Tentacle : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation; // SkeletonAnimation 스크립트
    public AnimationReferenceAsset[] AnimClip;
    public Transform player;    // 플레이어 위치받기위함

    private GameObject tentacle; // 스파인오브젝트 받기

    // 애니메이션 관련 선언
    public enum AnimState { appear, idle, attack_left, attack_right, hit, die }
    private AnimState _AnimState;
    private string currentAnimation;
    private bool isAppear = false;  // appear 애니메이션 1회 제한 위함, true면 다른 애니메이션 실행가능

    // 기능관련 선언
    private bool isCollide = false; // 캐릭터와 충돌시 true 아니면 false
    private bool canStun = true;   // true시 최초 1회 스턴가능, 스턴 한번 하면(false시) 공격모드로 바뀜

    void Awake()
    {
        tentacle = GameObject.Find("TentacleGFX");
    }

    // Start is called before the first frame update
    void Start()
    {
        // 처음 Tentacle 소환시 appear의 씨앗모양 유지 위해 루프끄고 타임스케일 0으로
        skeletonAnimation.state.SetAnimation(0, "vine_appear", false).TimeScale = 0f;
        skeletonAnimation.loop = false;
        skeletonAnimation.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollide && canStun) {
            Appear();
        }
        // 애니메이션 적용
        if (isAppear) {
            SetCurrentAnimation(_AnimState);
        }
        if(isAppear && isCollide) {
            Attack();
        }
        else if(isAppear && !isCollide) {
            _AnimState = AnimState.idle;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            isCollide = true;
            Debug.Log("collide T");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {
            isCollide = false;
            Debug.Log("collide F");
        }
    }

    void Appear()   // 텐타클 범위에 플레이어 들어오면 Appear
    {
        canStun = false;

        //해당 애니메이션으로 변경한다.
        skeletonAnimation.state.SetAnimation(0, "vine_appear", false).TimeScale = 1f;
        skeletonAnimation.loop = false;
        skeletonAnimation.timeScale = 1f;

        Invoke("TransformSetting", 0.5f);    // 씨앗과 본체 크기 너무 달라서 조절하는 함수 삽입
        Invoke("Stun", 0.5f);   // 범위에 들어온 후 0.5초뒤 스턴
        Invoke("Idle", 1f);
    }

    void Stun()
    {
        if(!isCollide) {    // 0.5초 뒤 스턴 하는데 범위 안에 없으면 스턴 안걸리고 범위 안에 있으면 스턴
            return;
        }

        Debug.Log("stun");
        Player.canMove = false;
        Invoke("StunExit", 1f); // 1초뒤 스턴해제
    }
    void StunExit()
    {
        Debug.Log("stunExit");
        Player.canMove = true;
    }
    void TransformSetting()
    {
        tentacle.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.23f, 0f);
    }
    void Idle()   // 어택모드로 바뀌어서 공격대기상태
    {
        _AnimState = AnimState.idle;
        isAppear = true;
        Debug.Log("AttackMode");
    }
    void Attack()
    {
        if(transform.position.x < player.position.x) {  // 플레이어가 오른쪽에 있으면
            _AnimState = AnimState.attack_right;
        }
        else if(transform.position.x > player.position.x) { // 플레이어가 왼쪽에 있으면
            _AnimState = AnimState.attack_left;
        }
        Debug.Log("Attack");
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
