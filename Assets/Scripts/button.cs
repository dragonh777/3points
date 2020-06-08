using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class button : MonoBehaviour
{
    [SerializeField]
    private Stat sp;

    public GameObject Monster;
    //public GameObject Skill;
    public int cost;
    public Image image;
    private Button bbt;
    public float coolTime = 10.0f;
    public static bool isClicked = false;
    float leftTime = 10.0f;
    float speed = 5.0f;

    // Start is called before the first frame update

    void Start()
    {
        bbt = GetComponent<Button>();
        // sp.Initiallize(Play_boss.initsp, Play_boss.initsp);
    }

    // Update is called once per frame
    void Update()
    {

        if (isClicked)
        {
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime * speed;
                if (leftTime < 0)
                {
                    leftTime = 0;
                    if (bbt)
                        bbt.enabled = true;
                    isClicked = true;
                }
                float ratio = 1.0f - (leftTime / coolTime);
                if (image)
                    image.fillAmount = ratio;
            }
        }

    }

    public void StartCoolTime()
    {
        leftTime = coolTime;
        isClicked = true;
        if (bbt)
            bbt.enabled = false; // 버튼 기능을 해지함.
    }

    public void button_active1()
    {
        if (Play_boss.initsp >= cost)
        {
            Instantiate(Monster, new Vector3(0, 0, 0), Quaternion.identity);
            Play_boss.initsp -= cost;
            leftTime = coolTime;
            isClicked = true;
            if (bbt)
                bbt.enabled = false;
        }

    }
    public void button_active2()
    {
        if (Play_boss.initsp >= cost)
        {
            Instantiate(Monster, new Vector3(0, 0, 0), Quaternion.identity);
            Play_boss.initsp -= cost;
            leftTime = coolTime;
            isClicked = true;
            if (bbt)
                bbt.enabled = false;
        }

    }
    public void button_active3()
    {
        if (Play_boss.initsp >= cost)
        {
            Instantiate(Monster, new Vector3(0, 0, 0), Quaternion.identity);
            Play_boss.initsp -= cost;
            leftTime = coolTime;
            isClicked = true;
            if (bbt)
                bbt.enabled = false;
        }

    }
}

