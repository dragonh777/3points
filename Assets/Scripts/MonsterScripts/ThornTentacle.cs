﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThornTentacle : MonoBehaviour
{
    public Transform _playerTransform;
    public Image HPBar;
    public Canvas HPCanvas;

    private Rigidbody2D _rigid;
    private BoxCollider2D _boxColl; // 애니메이션 있는 오브젝트의 콜라이더(씨앗)
    private CapsuleCollider2D _capColl; // 애니메이션 있는 오브젝트 콜라이더(텐타클)
    private Animator _animator;
    private GameObject attackBound;
    public int statement = 0;  // 0: seed, 1: appear, 2: idle, 3: attack, 4: hit, 5: die
    private bool appearFlag = false;    // t: appear, f: seed
    public bool isCollide = false; // t: 공격범위 안에 플레이어
    private bool attackFlag = false;    // t: 공격모션중, f: 공격모션끝(공격모션 끝나야 다음공격 방향 정해짐)
    private bool hitFlag = true;   // t:맞는모션중, f: 안맞는중

    public static float HP = 100.0f;
    private float currentHP;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
        _playerTransform = GameObject.Find("Player").GetComponent<Transform>();
        _rigid = GetComponent<Rigidbody2D>();
        _boxColl = GetComponent<BoxCollider2D>();
        _capColl = GetComponent<CapsuleCollider2D>();
        _animator = GetComponent<Animator>();
        attackBound = gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // 맞을 때
        if(currentHP != HP) {
            currentHP = HP;
            HPBar.fillAmount = HP / 100f;
        }
        // 죽을 때
        if(HP <= 0) {
            statement = 5;
        }

        if(appearFlag) {
            appearFlag = false;
            statement = 1;
        }
    }

    void FixedUpdate() 
    {
        if(statement == 1) {    // appear
            _animator.SetBool("isCollide", true);
        }
        else if(statement == 2 && !isCollide) {   // idle
            _animator.Play("vine_idle");
        }
        else if(statement == 3 && !attackFlag) {   // attack
            Attack();
        }
        else if(statement == 4) {   // hit
            _animator.Play("vine_hit");
        }
        else if(statement == 5) {   // die
            _animator.Play("vine_die");
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player" && statement == 0) {
            appearFlag = true;
        }
        else if(other.gameObject.tag == "Bullet" && statement != 0) {
            Hit();
        }
    }

    // 애니메이션 이벤트
    void Appear() 
    {
        HPCanvas.gameObject.SetActive(true);
        _boxColl.enabled = false;
        _capColl.enabled = true;
        attackBound.SetActive(true);
        hitFlag = false;
    }

    void Attack()
    {
        attackFlag = true;
        float dirX = _playerTransform.position.x - transform.position.x;

        if(dirX > 0) {  // 플레이어가 오른쪽이면
            _animator.Play("vine_attack_right");
        }
        else if(dirX < 0) { // 플레이어가 왼쪽이면
            _animator.Play("vine_attack_left");
        }
    }
    void AttackFlagSetFalse()   // 애니메이션이벤트에서 사용
    {
        attackFlag = false;
    }

    void Hit()
    {
        _animator.SetBool("hitEnd", false);
        hitFlag = true;
        statement = 4;
        HP -= 10f;
    }

    void Die()
    {
        Destroy(gameObject);
    }

    void StatementChange(int index)     // 애니메이션이벤트에서 statement바꾸기용
    {
        hitFlag = false;
        _animator.SetBool("hitEnd", true);
        statement = index;
    }
}