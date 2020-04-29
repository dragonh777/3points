using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 처음 씨앗모양 -> 닿으면 튀어나와서 스턴 -> 스턴2초후 1초뒤에 체력바 나오고 공격모드로 바뀜
public class ThornTentacle : MonoBehaviour
{
    public float timeLeft = 1.0f;

    private float nextTime = 0.0f;
    private bool canStun = true;    // true: can stun, false: can't stun
    private bool isCollide = false; // true: do stun, false: stop stun


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(isCollide && canStun) {
            Stun();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isCollide = true;
        }
    }

    void Stun()
    {
        Debug.Log("stun");
        Player.canMove = false;
        Invoke("StunExit", 2f);
    }
    void StunExit()
    {
        Debug.Log("stunExit");
        Player.canMove = true;
        isCollide = false;
        canStun = false;

        Invoke("Attack", 1.5f);
    }

    void Attack()   // 공격하는 애니메이션, 값 넣기
    {
        Debug.Log("AttackMode");
    }
}
