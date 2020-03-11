using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
    public float moveDelay = 0.5f;
    public float bossSpeed = 1f;
    public int bossStep = 3;

    private int bStep;

    private float timeLeft;
    private float nextTime = 0.0f;
    private Transform transform;

    private bool isMove = false;

    Vector3 moveAmount;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        timeLeft = moveDelay;
        moveAmount = bossSpeed * Vector3.left * Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isMove)
        {
            BossMove();
        }
    }

    void Update()
    {
        if (Time.time > nextTime)
        {
            nextTime = Time.time + timeLeft;
            bStep -= bossStep;
            Debug.Log("move");
            isMove = true;
        }
    }

    void BossMove()
    {
        transform.Translate(moveAmount);
    }
    
}
