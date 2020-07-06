using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private AcidPott _acidPot = null;
    private WheelGoo _wheelGoo = null;
    private BombSeed _bombSeed = null;
    private ThornTentacle _tentacle = null;
    private Golem_Boss _golem = null;
    
    void Start()
    {
        _acidPot = this.transform.parent.gameObject.GetComponent<AcidPott>();
        _wheelGoo = this.transform.parent.gameObject.GetComponent<WheelGoo>();
        _bombSeed = this.transform.parent.gameObject.GetComponent<BombSeed>();
        _tentacle = this.transform.parent.gameObject.GetComponent<ThornTentacle>();
        _golem = this.transform.parent.gameObject.GetComponent<Golem_Boss>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null && (other.gameObject.tag == "Player" || other.gameObject.tag == "Siege_Player")) {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
        // 밤시드꺼
        else if(_bombSeed != null && (other.gameObject.tag == "Player" || other.gameObject.tag == "Siege_Enemy")) {
            _bombSeed.statement = 1;
        }
        // 텐타클꺼
        if(_tentacle != null && _tentacle.statement == 0 && other.gameObject.tag == "Player") {
            _tentacle.statement = 1;
        }
        else if(_tentacle != null && (_tentacle.statement == 2 || _tentacle.statement == 3) && other.gameObject.tag == "Player") {
            _tentacle.isCollide = true;
        }

        // 보스 골렘꺼
        if(_golem != null && other.gameObject.tag == "Bullet") {
            _golem.HP -= 10f;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null && (other.gameObject.tag == "Player" || other.gameObject.tag == "Siege_Player")) {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
        // 텐타클꺼
        else if(_tentacle != null && (_tentacle.statement == 2 || _tentacle.statement == 3) && other.gameObject.tag == "Player") {
            _tentacle.isCollide = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        // 애시드팟꺼
        if (_acidPot != null && (other.gameObject.tag == "Player" || other.gameObject.tag == "Siege_Player")) {  // 트리거에서 player가 벗어나면
            _acidPot.isCollide = false;
        }
        // 텐타클꺼
        else if(_tentacle != null && other.gameObject.tag == "Player") {
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
