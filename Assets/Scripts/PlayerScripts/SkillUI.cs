using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [Header("Summon Button & Cool Button")]
    public Button[] Btn;
    public GameObject[] backBtn;
    public Image[] image;
    public SpriteRenderer[] iconRenderer;

    [Header("CoolTime(sec) & SpCost")]
    public float[] coolTime;

    float leftTime = 0.0f;
    float leftTime1 = 0.0f;
    bool isClicked1 = false;
    bool isClicked2 = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("BasicAttack") && leftTime <= 0 && !Players.isDie)
        {
            backBtn[0].SetActive(true);
            leftTime = coolTime[0];
            isClicked1 = true;
            if (Btn[0])
                Btn[0].enabled = false;
            
        }

        if (isClicked1)
        {
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                if (leftTime < 0)
                {

                    leftTime = 0;
                    if (Btn[0])
                        Btn[0].enabled = true;

                    isClicked1 = true;
                }
                float ratio = 0f + (leftTime / coolTime[0]);
                float ratio1 = 1f - (leftTime / coolTime[0]);
                iconRenderer[0].color = new Color(ratio1, ratio1, ratio1);
                if (image[0])
                {
                    image[0].fillAmount = ratio;

                }

                if (image[0].fillAmount == 0)
                    backBtn[0].SetActive(false);
            }
        }

        if (Input.GetButtonDown("Dash") && leftTime1 <= 0 && !Players.isDie)
        {
            backBtn[1].SetActive(true);
            leftTime1 = coolTime[1];
            isClicked2 = true;
            if (Btn[1])
                Btn[1].enabled = false;

        }

        

        if (isClicked2)
        {
            if (leftTime1 > 0)
            {
                leftTime1 -= Time.deltaTime;
                if (leftTime1 < 0)
                {

                    leftTime1 = 0;
                    if (Btn[1])
                        Btn[1].enabled = true;

                    isClicked2 = true;
                }
                float ratio = 0f + (leftTime1 / coolTime[1]);
                float ratio1 = 1f - (leftTime1 / coolTime[1]);
                iconRenderer[1].color = new Color(ratio1, ratio1, ratio1);
                if (image[1])
                {
                    image[1].fillAmount = ratio;

                }

                if (image[1].fillAmount == 0)
                    backBtn[1].SetActive(false);
            }
        }
    }
}
