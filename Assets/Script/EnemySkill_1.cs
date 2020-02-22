using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySkill_1 : MonoBehaviour
{
    public float BulletSpeed;

    public GameObject Skill;
    public Image image;
    public Button skillButton;
    public float coolTime = 10.0f;
    public bool isClicked = false;
    float leftTime = 10.0f;
    float speed = 5.0f;

    private Transform transform;

    Vector3 moveAmount;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        moveAmount = BulletSpeed * Vector3.left * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveAmount);

        if (isClicked)
        {
            if(leftTime > 0)
            {
                leftTime -= Time.deltaTime * speed;
                if(leftTime < 0)
                {
                    leftTime = 0;
                    if (skillButton)
                        skillButton.enabled = true;
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
        Instantiate(Skill, new Vector3(6, 0, 0), Quaternion.identity);
        leftTime = coolTime;
        isClicked = true;
        if (skillButton)
            skillButton.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Hit");
            Destroy(gameObject);
            PlayerMove.Hp--;
        }
    }
}
