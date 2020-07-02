using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Siege_MonsterControl : MonoBehaviour
{
    private Siege_EAcidPot _acidPot = null;
    
    void Start()
    {
        _acidPot = this.transform.parent.gameObject.GetComponent<Siege_EAcidPot>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null &&  other.gameObject.tag == "Siege_Player") {
            _acidPot.isCollide = true;
            // _acidPot.statement = 2;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null &&  other.gameObject.tag == "Siege_Player") {
            _acidPot.isCollide = true;
            // _acidPot.statement = 2;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 애시드팟꺼
        if (_acidPot != null &&  other.gameObject.tag == "Siege_Player") {  // 트리거에서 player가 벗어나면
            _acidPot.isCollide = false;
        }
    }

    // // 휠구꺼
    // void AfterHit()
    // {
    //     _wheelGoo.AfterHit();
    // }

    // void AfterDeath()
    // {
    //     _wheelGoo.AfterDeath();
    // }

    // // 밤시드꺼
    // void AfterDeath_bombSeed()
    // {
    //     _bombSeed.AfterDeath();
    // }

}
