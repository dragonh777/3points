using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 12f;

    private Transform transform;
    private float speed;

    Vector3 moveAmount;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        speed = Random.Range(1, bulletSpeed);
        moveAmount = speed * Vector3.left * Time.deltaTime;
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
