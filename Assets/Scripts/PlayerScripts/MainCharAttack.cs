using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이 스크립트는 메인 캐릭터 공격에 대해 구현할 파일임
// 현재 구현된 기능
// 마우스 방향으로 총알날리기

// 추가할 기능
// 정해진 스킬 사용, 쿨타임 등 적용예정
// 개인 노트북에서는 다른 키 입력시 마우스클릭이 씹힘(가만있어야만 공격됨), 공격키가 키보드면 상관없음 이거 고치기

public class MainCharAttack : MonoBehaviour
{

    public GameObject bulletPrefab; // 총알 프리팹

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetButtonDown("BasicAttack")) {  // BasicAttack = mouse 0, left ctrl
            Instantiate(bulletPrefab, transform.position, transform.rotation);  // 현재 캐릭터에서 총알 생성
        }
    }

}
