using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

public class Golem_Boss : MonoBehaviour
{
    // 골렘 정보
    public float HP = 200f;
    private float currentHP;
    private float maxHP;
    
    // 애니메이션 관련
    public AnimState _AnimState;
    public enum AnimState {
        Idle, Walk, Hit, Crash, OnRoll, Roll, EndRoll, Punch, Die
    }
    public AnimationReferenceAsset[] AnimClip;  // 스켈레톤 애니메이션
    public SkeletonAnimation skeletonAnimation; // 스크립트
    private string currentAnimation;    // 애니메이션 중복실행 체크용

    // 받아오는 정보
    public Image HPBar;
    public GameObject _Punch;
    private GameObject _player;
    private GameObject _GFX;
    private GameObject _rollSprite;
    private GameObject _crashBound;

    // 제어용 변수들
    public int state = 0;  // 1: walk, 2, 3: attack
    public bool isRoll = false;
    public bool isAttack = false;
    public bool onAttackBound = false;   // 어택바운드 내에 들어오면 t
    public bool isHit = false;


    // Start is called before the first frame update
    void Start()
    {
        maxHP = currentHP = HP;
        _player = GameObject.Find("Player");
        _GFX = transform.GetChild(0).gameObject;
        _rollSprite = transform.GetChild(1).gameObject;
        _crashBound = transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.fillAmount = currentHP / maxHP;
        if(currentHP <= 0) {
            SetAnimation("set", "golem_die", false);
            state = 100;
            isAttack = true;
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        else if(currentHP != HP) {
            currentHP = HP;
            if(!isAttack) {
                isHit = true;
                state = 100;
                SetAnimation("set", "golem_hit", false);
                Invoke("EndHit", 0.5f);
            }
        }

        if(DistanceCheck().x >- 7f && DistanceCheck().x < 7f) {
            onAttackBound = true;
        }
        else {
            onAttackBound = false;
        }

        if(DistanceCheck().x > 0) { // 오른쪽으로 움직일 때
            transform.localScale = new Vector3(1f, 1f);
            }
        else {  // 왼쪽으로 움직일 때
            transform.localScale = new Vector3(-1f, 1f);
        }  

        if(onAttackBound && !isAttack && !isRoll && !isHit) {
            if(currentHP <= (maxHP/2)) {
                state = UnityEngine.Random.Range(3, 5);
            }
            else {
                state = 3;
            }
        }
        else if(!isAttack && !isRoll && !onAttackBound && !isHit){
            state = 1;  // walk
        }

    }
    void EndHit()
    {
        isHit = false;
    }

    void FixedUpdate() 
    {
        if(state == 1) {
            SetAnimation("set", "golem_walk_Full", true);
            MoveToPlayer(0.03f);
        }
        else if(state == 2) {
            if(isAttack) {
                return;
            }
            isAttack = true;

            Crash();
        }
        else if(state == 3) {
            MoveToPlayer(0.1f);

            if(isAttack) {
                return;
            }
            isAttack = true;
            OnRolling();
        }
        else if(state == 4){
            if(isAttack){
                return;
            }
            isAttack = true;
            RocketPunch();
        }
    }

    // 플레이어와 골렘 거리 체크
    Vector3 DistanceCheck()
    {
        Vector3 distance = new Vector3(_player.transform.position.x - transform.position.x, transform.position.y);
        return distance;
    }

    void MoveToPlayer(float moveSpeed)
    {
        Vector3 target = new Vector3(_player.transform.position.x, transform.position.y);
        
        if(!isRoll && !isAttack) {
            if(DistanceCheck().x > 0) { // 오른쪽으로 움직일 때
                transform.localScale = new Vector3(1f, 1f);
            }
            else {  // 왼쪽으로 움직일 때
                transform.localScale = new Vector3(-1f, 1f);
            }       
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed); 
        }
        else if(isRoll) {
            if(transform.localScale.x > 0) {
                _rollSprite.transform.Rotate(new Vector3(0, 0, -5));
            }
            else {
                _rollSprite.transform.Rotate(new Vector3(0, 0, -5));
            }
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed);
        }
    }

    // 기본공격
    void Crash()
    {
        SetAnimation("set", "golem_attack", false);
        Invoke("AcviteBound", 0.6f);
    }
    void AcviteBound()
    {
        _crashBound.SetActive(true);
        Invoke("EndCrash", 0.7333f);
    }
    void EndCrash()
    {
        _crashBound.SetActive(false);
        isAttack = false;
        state = 1;
    }

    // 롤링 스킬
    void OnRolling()
    {
        if(isRoll) {
            return;
        }
        isRoll = true;

        SetAnimation("set", "golem_circle_1", false);
        Invoke("Rolling", 0.2667f);
    }
    void Rolling()
    {
        _GFX.GetComponent<MeshRenderer>().enabled = false;
        _rollSprite.SetActive(true);

        Invoke("EndRolling", 1f);
    }
    void EndRolling()
    {
        _GFX.GetComponent<MeshRenderer>().enabled = true;
        _rollSprite.SetActive(false);
        _rollSprite.transform.localRotation = new Quaternion(0f, 0f, 0f, 0f);
        SetAnimation("set", "golem_circle_2", false);
        // isAttack = false;
        isRoll = false;
        Crash();
    }

    // 로켓펀치 스킬
    void RocketPunch()
    {
        SetAnimation("set", "golem_punch", false);
        Invoke("ShootRocketPunch", 0.2f);
        Invoke("EndPunch", 1.4f);
    }
    void ShootRocketPunch()
    {
        if(!isAttack) {
            return;
        }
        GameObject punch = Instantiate(_Punch);
        punch.tag = "Enemy";
    }
    void EndPunch()
    {
        isAttack = false;
        state = 1;
    }

    

    void SetAnimation(string setORadd, string AnimName, bool loop)  // 사용법: SetAnimation("set"OR"add", "애니메이션이름", trueORfalse)
    {
        if(AnimName.Equals(currentAnimation)) {
            return;
        }

        if(setORadd.Equals("set")) {
            skeletonAnimation.state.SetAnimation(0, AnimName, loop);    // 애니메이션 실행(직전 애니메이션 끊고 바로 실행)
        }
        else if(setORadd.Equals("add")) {
            skeletonAnimation.state.AddAnimation(0, AnimName, loop, 0f);    // 애니메이션 추가(직전 애니메이션 끝난 뒤 실행)
        }
        else {
            Debug.Log("SetAnimation Error!");
        }

        currentAnimation = AnimName;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // if(other.gameObject.tag == "Player") {
        //     onAttackBound = true;
        // }
        if(other.gameObject.tag == "Bullet" && !isHit && !isRoll) {
            HP -= 10f;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        // if(other.gameObject.tag == "Player") {
        //     onAttackBound = true;
        // }    
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        // if(other.gameObject.tag == "Player") {
        //     onAttackBound = false;
        // }    
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
