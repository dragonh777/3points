using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fTime = 0.4f;

    private float leftTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        leftTime = fTime;
    }

    // Update is called once per frame
    void Update()
    {
        leftTime -= Time.deltaTime;
        float ratio = 0f + (leftTime / fTime);
        byte ratiob = (byte)((ratio * 100) + 155);
        GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, ratiob);
        if (ratiob <= 1)
        {
            Destroy(gameObject);
        }
        //if (ratio > 0)
        //{
        //    ratio--;
        //    
        //}
    }
}
