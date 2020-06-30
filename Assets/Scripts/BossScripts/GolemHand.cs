using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemHand : MonoBehaviour
{
    private GameObject golem;
    private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        golem = GameObject.Find("Golem");
        if(golem.transform.localScale.x > 0) {  // 오른쪽발사
            transform.position = new Vector3(golem.transform.position.x + 2.32f, golem.transform.position.y + 3.0f);
        }
        else {  // 왼쪽발사
            transform.position = new Vector3(golem.transform.position.x - 2.32f, golem.transform.position.y + 3.0f);
            transform.localScale = new Vector3(-1f, 1f);
        }

        Destroy(gameObject, 1f);
    
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVelocity = Vector3.zero;

        if(transform.localScale.x > 0) {    // 오른쪽발사
            moveVelocity = Vector3.right;
        }
        else {  // 왼쪽발사
            moveVelocity = Vector3.left;
        }

        transform.position += moveVelocity * speed * Time.deltaTime;
        
    }
}
