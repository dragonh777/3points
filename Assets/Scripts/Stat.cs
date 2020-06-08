using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour
{
    private Image content;

    [SerializeField]
    private Text statText;

    [SerializeField]
    private float lerpSpeed;

    private float currentFill;
    public float MyMaxValue { get; set; }

    public Play_boss initsp;

    public float MyCurrentValue
    {
        get
        {
            return currentValue;
        }
        set
        {
            if (value > MyMaxValue)
                currentValue = MyMaxValue;
            else if (value < 0)
                currentValue = 0;
            else
                currentValue = value;

            currentFill = currentValue / MyMaxValue;
            statText.text = currentValue + "/" + MyMaxValue;
        }
    }
    // Start is called before the first frame update
    public float currentValue;
    void Start()
    {
        content = GetComponent<Image>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Player_spbar();
        //sp 값 변경시
        if (currentFill != content.fillAmount)
        {
            //Mathf.Lerp(시작값, 끝값, 기준) 부드럽게 값을 변경 가능
            content.fillAmount = Mathf.Lerp(content.fillAmount, currentFill, Time.deltaTime * lerpSpeed);
        }
    }
    //sp값 셋팅(현재 값, 최대 값)
    public void Initiallize(float currentValue, float maxValue)
    {
        MyMaxValue = maxValue;
        MyCurrentValue = currentValue;
    }
    /* public void Player_spbar()
     {
         float sp=
     }*/
}
