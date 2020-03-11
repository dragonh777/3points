using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySkill_1 : MonoBehaviour
{
    public float bulletSpeed = 30f;

    public GameObject skill;
    public Image image;
    public Button skillButton;
    public float coolTime = 10.0f;
    public static bool isClicked = false;
    float leftTime = 10.0f;
    float speed = 5.0f;

    private Transform transform;

    Vector3 moveAmount;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        moveAmount = bulletSpeed * Vector3.left * Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(moveAmount);

        if (isClicked && EnemyMove.Hp > 0)
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
        if (EnemyMove.Hp > 0)
        {
            Instantiate(skill, new Vector3(6, 0, 0), Quaternion.identity);
            leftTime = coolTime;
            isClicked = true;
            if (skillButton)
                skillButton.enabled = false;
        }
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
