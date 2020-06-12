using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour
{
    public Button Btn;
    public GameObject Monster;
    public Image image;
    public TextMeshProUGUI spText;
    public Slider spS;

    public float regenPerSecond = 2f;

    public float coolTime = 10.0f;
    float leftTime = 10.0f;
    bool isClicked = false;

    float regen = 0f;

    int sp = 100;
    int maxSp = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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

        if (isClicked && sp > 0)
        {
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                if (leftTime < 0)
                {
                    leftTime = 0;
                    if (Btn)
                        Btn.enabled = true;
                    isClicked = true;
                }
                float ratio = 1.0f - (leftTime / coolTime);
                if (image)
                    image.fillAmount = ratio;
            }
        }
    }

    public void Summon1()
    {
        if (sp >= 10)
        {
            Instantiate(Monster, new Vector3(0, 0, 0), Quaternion.identity);
            leftTime = coolTime;
            isClicked = true;
            sp -= 10;
            spS.value -= 10f;
            
            if (Btn)
                Btn.enabled = false;
        }
    }
}
