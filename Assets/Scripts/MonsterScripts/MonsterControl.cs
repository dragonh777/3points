using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private AcidPott _acidPot = null;
    private WheelGoo _wheelGoo = null;
    private BombSeed _bombSeed = null;
    private ThornTentacle _tentacle = null;

    void Start()
    {
        _acidPot = this.transform.parent.gameObject.GetComponent<AcidPott>();
        _wheelGoo = this.transform.parent.gameObject.GetComponent<WheelGoo>();
        _bombSeed = this.transform.parent.gameObject.GetComponent<BombSeed>();
        _tentacle = this.transform.parent.gameObject.GetComponent<ThornTentacle>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null && other.gameObject.tag == "Player") {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
        // 밤시드꺼
        else if(_bombSeed != null && other.gameObject.tag == "Player") {
            _bombSeed.statement = 1;
        }
        // 텐타클꺼
        else if(_tentacle != null && other.gameObject.tag == "Player") {
            _tentacle.isCollide = true;
            _tentacle.statement = 3;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null && other.gameObject.tag == "Player") {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
        // 텐타클꺼
        else if(_tentacle != null && other.gameObject.tag == "Player") {
            _tentacle.isCollide = true;
            _tentacle.statement = 3;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        // 애시드팟꺼
        if (_acidPot != null && collision.gameObject.tag == "Player") {  // 트리거에서 player가 벗어나면
            _acidPot.isCollide = false;
        }
        // 텐타클꺼
        else if(_tentacle != null && collision.gameObject.tag == "Player") {
            _tentacle.isCollide = false;
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
