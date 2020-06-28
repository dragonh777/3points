using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private AcidPott _acidPot;    // 애시드팟 스크립트
    private WheelGoo _wheelGoo;

    void Start()
    {
        _acidPot = this.transform.parent.gameObject.GetComponent<AcidPott>();
        _wheelGoo = this.transform.parent.gameObject.GetComponent<WheelGoo>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player") {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player") {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {  // 트리거에서 player가 벗어나면
            _acidPot.isCollide = false;
        }
    }

    // 휠구꺼
    void AfterHit()
    {
        _wheelGoo.AfterHit();
    }

    void AfterDeath()
    {
        _wheelGoo.AfterDeath();
    }
}
