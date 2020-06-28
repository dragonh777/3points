using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    public float fTime = 3f;

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
        float ratio1 = 1f - (leftTime / fTime);

        //if (ratio > 0)
        //{
        //    ratio--;
        //    GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, ratio);
        //}
    }
}
