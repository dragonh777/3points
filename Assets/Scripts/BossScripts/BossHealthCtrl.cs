using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthCtrl : MonoBehaviour
{
    public Slider healthBar;
    public SpriteRenderer leftEye;
    public SpriteRenderer rightEye;

    private Color tempC;

    // Start is called before the first frame update
    void Start()
    {
        tempC = leftEye.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && healthBar.value > 0){
            healthBar.value -= 10;
            tempC.a -= 0.1f;

            leftEye.color = tempC;
            rightEye.color = tempC;
        }

    }
}
