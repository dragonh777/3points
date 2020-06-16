using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    [Header("Summon Button & Cool Button")]
    public Button[] Btn;
    public GameObject[] backBtn;
    public Image[] image;

    [Header("Summon Monster & image")]
    public GameObject[] monster;
    public SpriteRenderer[] monsterRenderer;

    [Header("CoolTime(sec) & SpCost")]
    public float[] coolTime;
    public int[] cost;
    public Slider spS;

    float leftTime = 0.0f;
    float leftTime1 = 0.0f;
    float leftTime2 = 0.0f;
    bool isClicked1 = false;
    bool isClicked2 = false;
    bool isClicked3 = false;
    int cnt = 0;

    //float regen = 0f;

    //int sp = 100;
    //int maxSp = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //regen += regenPerSecond * Time.deltaTime;
        //spS.value += regenPerSecond * Time.deltaTime;
        //spText.text = sp + "/" + maxSp;
        //if (regen > 1.0f)
        //{
        //    if (sp < maxSp)
        //        sp += 1;
        //    regen = 0f;
        //}


        if (isClicked1 && BossControl.sp > 0)
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
                monsterRenderer[0].color = new Color(ratio1, ratio1, ratio1);
                if (image[0])
                {
                    image[0].fillAmount = ratio;
                    
                }
                    
                if (image[0].fillAmount == 0)
                    backBtn[0].SetActive(false);
            }
        }

        if (isClicked2 && BossControl.sp > 0)
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
                monsterRenderer[1].color = new Color(ratio1, ratio1, ratio1);
                if (image[1])
                {
                    image[1].fillAmount = ratio;

                }

                if (image[1].fillAmount == 0)
                    backBtn[1].SetActive(false);
            }
        }

        if (isClicked3 && BossControl.sp > 0)
        {
            if (leftTime2 > 0)
            {
                leftTime2 -= Time.deltaTime;
                if (leftTime2 < 0)
                {

                    leftTime2 = 0;
                    if (Btn[2])
                        Btn[2].enabled = true;

                    isClicked3 = true;
                }
                float ratio = 0f + (leftTime2 / coolTime[2]);
                float ratio1 = 1f - (leftTime2 / coolTime[2]);
                monsterRenderer[2].color = new Color(ratio1, ratio1, ratio1);
                if (image[2])
                {
                    image[2].fillAmount = ratio;

                }

                if (image[2].fillAmount == 0)
                    backBtn[2].SetActive(false);
            }
        }

    }

    public void Summon1()
    {
        if (BossControl.sp >= cost[0])
        {
            if (cnt == 0)
                Instantiate(monster[0], new Vector3(7, -7, 0), Quaternion.identity);
            backBtn[0].SetActive(true);
            leftTime = coolTime[0];
            isClicked1 = true;
            BossControl.sp -= cost[0];
            spS.value -= (float)cost[0];
            cnt++;
            if (Btn[0])
                Btn[0].enabled = false;
        }

    }

    public void Skill1()
    {
        if (BossControl.sp >= cost[0])
        {
            //Instantiate(monster[0], new Vector3(7, -7, 0), Quaternion.identity);
            backBtn[0].SetActive(true);
            leftTime = coolTime[0];
            isClicked1 = true;
            BossControl.sp -= cost[0];
            spS.value -= (float)cost[0];

            if (Btn[0])
                Btn[0].enabled = false;
        }
    }

    public void Summon2()
    {
        if (BossControl.sp >= cost[1])
        {
            //Instantiate(monster[1], new Vector3(0, 0, 0), Quaternion.identity);
            backBtn[1].SetActive(true);
            leftTime1 = coolTime[1];
            isClicked2 = true;
            BossControl.sp -= cost[1];
            spS.value -= (float)cost[1];

            if (Btn[1])
                Btn[1].enabled = false;
        }
    }

    public void Summon3()
    {
        if (BossControl.sp >= cost[2])
        {
            //Instantiate(monster[2], new Vector3(0, 0, 0), Quaternion.identity);
            Debug.Log("a");
            backBtn[2].SetActive(true);
            leftTime2 = coolTime[2];
            isClicked3 = true;
            BossControl.sp -= cost[2];
            spS.value -= (float)cost[2];

            if (Btn[2])
                Btn[2].enabled = false;
        }
    }
}
