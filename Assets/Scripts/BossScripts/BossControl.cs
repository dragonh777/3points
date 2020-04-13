using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
    public float moveDelay = 1.5f;
    public float bossSpeed = 5f;
    public int bossStep = 3;

    public float radius = 1f;

    public Transform bossPos;

    private int bStep;

    private float timeLeft;
    private float nextTime = 0.0f;

    private float fTime = 0.0f;
    private float firstTime = 0.0f;

    private Transform transform;

    public Transform target;
    public BoxCollider2D collider;
    SpriteRenderer renderer;

    private float aa;

    bool rangeOn;

    private bool isMove = false;

    Vector3 moveAmount;

    // Start is called before the first frame update
    void Start()
    {
        bStep = Random.Range(2, 5);
        transform = GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        timeLeft = moveDelay;
        rangeOn = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        BossMove();

        
    }

    void Update()
    {
        BossStep();
        rangeOn = Physics2D.OverlapCircle(bossPos.position, radius, 1 << LayerMask.NameToLayer("Player"));
    }

    void BossMove()
    {
        if (isMove)
        {
            float dir = target.position.x - transform.position.x;
            if (dir < 0)
            {
                moveAmount = bossSpeed * Vector3.left * Time.deltaTime;
                renderer.flipX = true;
            }
            else if (dir > 0)
            {
                moveAmount = bossSpeed * Vector3.right * Time.deltaTime;
                renderer.flipX = false;
            }
            transform.Translate(moveAmount);
        }
    }

    void BossStep()
    {
        fTime += Time.deltaTime;
        if (rangeOn)
        {
            bStep = 0;
            isMove = false;
            Debug.Log("rangeOn");
        }
        if (Time.time > nextTime && bStep > 0)
        {
            nextTime = Time.time + timeLeft;
            isMove = true;
            //Debug.Log("moveon");
            //Debug.Log(fTime);
            if (fTime >= 2)
            {
                isMove = false;
                fTime = 0.0f;
                //Debug.Log("moveoff");
                bStep--;
            }
        }
    }

    void Stomp()
    {

    }
}
