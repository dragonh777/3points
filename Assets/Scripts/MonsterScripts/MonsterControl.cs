using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    private AcidPott script;    // 애시드팟 스크립트

    void Start()
    {
        script = this.transform.parent.gameObject.GetComponent<AcidPott>();
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player") {
            script.isCollide = true;
            script.statement = 2;
        }
    }
    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player") {
            script.isCollide = true;
            script.statement = 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player") {  // 트리거에서 player가 벗어나면
            script.isCollide = false;
        }
    }
}
