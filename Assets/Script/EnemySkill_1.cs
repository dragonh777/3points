using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill_1 : MonoBehaviour
{
    public float BulletSpeed;

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
