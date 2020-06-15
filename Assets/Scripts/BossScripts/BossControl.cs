using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
    public TextMeshProUGUI spText;
    public Slider spS;

    public static int sp = 100;

    public int SP;

    public float regenPerSecond = 2f;

    private float regen = 0f;
    private int maxSp;

    //public float moveDelay = 1.5f;
    //public float bossSpeed = 5f;
    //public int bossStep = 3;

    //public float radius = 1f;

    //public Transform bossPos;

    //private int bStep;

    //private float timeLeft;
    //private float nextTime = 0.0f;

    //private float fTime = 0.0f;
    //private float firstTime = 0.0f;

    //private Transform transform;

    //public Transform target;
    //public BoxCollider2D collider;
    //SpriteRenderer renderer;

    

    //bool rangeOn;

    //private bool isMove = false;

    //Vector3 moveAmount;

    // Start is called before the first frame update
    void Start()
    {
        sp = SP;
        maxSp = SP;
        spS.maxValue = (float)SP;
        spS.value = (float)SP;
        //bStep = Random.Range(2, 5);
        //transform = GetComponent<Transform>();
        //renderer = GetComponent<SpriteRenderer>();
        //timeLeft = moveDelay;
        //rangeOn = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //BossMove();
    }

    void Update()
    {
        regen += regenPerSecond * Time.deltaTime;
        spS.value += regenPerSecond * Time.deltaTime;
        spText.text = sp + "/" + maxSp;
        if (regen > 1.0f)
        {
            if (sp < maxSp)
                sp += 1;
            regen = 0f;
        }

        //BossStep();
        //rangeOn = Physics2D.OverlapCircle(bossPos.position, radius, 1 << LayerMask.NameToLayer("Player"));
    }

    //void BossMove()
    //{
    //    if (isMove)
    //    {
    //        float dir = target.position.x - transform.position.x;
    //        if (dir < 0)
    //        {
    //            moveAmount = bossSpeed * Vector3.left * Time.deltaTime;
    //            renderer.flipX = true;
    //        }
    //        else if (dir > 0)
    //        {
    //            moveAmount = bossSpeed * Vector3.right * Time.deltaTime;
    //            renderer.flipX = false;
    //        }
    //        transform.Translate(moveAmount);
    //    }
    //}

    //void BossStep()
    //{
    //    fTime += Time.deltaTime;
    //    if (rangeOn)
    //    {
    //        bStep = 0;
    //        isMove = false;
    //        Debug.Log("rangeOn");
    //    }
    //    if (Time.time > nextTime && bStep > 0)
    //    {
    //        nextTime = Time.time + timeLeft;
    //        isMove = true;
    //        //Debug.Log("moveon");
    //        //Debug.Log(fTime);
    //        if (fTime >= 2)
    //        {
    //            isMove = false;
    //            fTime = 0.0f;
    //            //Debug.Log("moveoff");
    //            bStep--;
    //        }
    //    }
    //}

    //void Stomp()
    //{

    //}
}
