using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornTentacle : MonoBehaviour
{

    public float timeLeft = 1.0f;
    private float nextTime = 0.0f;

    private int cnt = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("aa");
            if (cnt == 0)
                Stun();
        }
    }

    void Stun()
    {
        Player.isMove = false;
        if (Time.time > nextTime)
        {
            nextTime = Time.time + timeLeft;
            Player.isMove = true;
        }
    }
}
