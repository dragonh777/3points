using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonCoolTime : MonoBehaviour
{
    public float coolTime;

    private SiegeButton buttonScript;
    private Button _button;
    private Image _image;

    private float leftTime;
    private float speed = 1.0f;
    public bool isClicked = false;

    // Start is called before the first frame update
    void Start()
    {
        buttonScript = GameObject.Find("GameManager").GetComponent<SiegeButton>();
        _button = GetComponent<Button>();
        _image = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isClicked) {
            if(leftTime > 0) {  // 시간 남았을 때
                leftTime -= Time.deltaTime * speed;

                if (leftTime < 0) {    // 쿨타임 끝났을 때
                    leftTime = 0;
                    _image.gameObject.SetActive(false);
                    isClicked = false;
                    _button.enabled = true;
                }
            }

            float ratio = (leftTime / coolTime);
            if (_image) {
                _image.fillAmount = ratio;
            }
        }
    }

    public void StartCoolTime(bool isSkill)
    {
        if(isSkill) {
        buttonScript.isAttack = true;
        }
        _image.gameObject.SetActive(true);

        leftTime = coolTime;
        isClicked = true;
        _button.enabled = false;
    }
}
