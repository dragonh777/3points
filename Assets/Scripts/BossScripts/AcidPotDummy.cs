using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidPotDummy : MonoBehaviour
{
    private AcidPott _acidPot = null;

    // Start is called before the first frame update
    void Start()
    {
        _acidPot = this.transform.parent.gameObject.GetComponent<AcidPott>();
    }

     private void OnTriggerEnter2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null && other.gameObject.tag == "Golem") {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
        
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        // 애시드팟꺼
        if (_acidPot != null && other.gameObject.tag == "Golem") {
            _acidPot.isCollide = true;
            _acidPot.statement = 2;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 애시드팟꺼
        if (_acidPot != null && other.gameObject.tag == "Golem") {  // 트리거에서 player가 벗어나면
            _acidPot.isCollide = false;
        }
    }
}
