using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private AcidPott _acidPot = null;
    private WheelGoo _wheelGoo = null;
    private BombSeed _bombSeed = null;

    void Start()
    {
        _acidPot = this.transform.parent.gameObject.GetComponent<AcidPott>();
        _wheelGoo = this.transform.parent.gameObject.GetComponent<WheelGoo>();
        _bombSeed = this.transform.parent.gameObject.GetComponent<BombSeed>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (other.gameObject.tag == "Player" && _acidPot != null) {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
        // 밤시드꺼
        else if(other.gameObject.tag == "Player" && _bombSeed != null)
        {
            _bombSeed.statement = 1;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (other.gameObject.tag == "Player" && _acidPot != null) {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 애시드팟꺼
        if (collision.gameObject.tag == "Player" && _acidPot != null) {  // 트리거에서 player가 벗어나면
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

    // 밤시드꺼
    void AfterDeath_bombSeed()
    {
        _bombSeed.AfterDeath();
    }
}
