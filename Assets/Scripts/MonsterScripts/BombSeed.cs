using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombSeed : MonoBehaviour
{
    public float Speed = 10f;

    public Transform player;

    private Transform transform;
    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float dirX = player.position.x - transform.position.x;
        float dirY = player.position.y - transform.position.y + 2f;
        float moveAmountX = 0, moveAmountY = 0;
        if (dirX > 0)
        {
            moveAmountX = Speed * Time.deltaTime;
        }
        else if (dirX < 0)
        {
            moveAmountX = -Speed * Time.deltaTime;
        }

        if (dirY > 0)
        {
            moveAmountY = Speed * Time.deltaTime;
        }
        if (dirY < 0)
        {
            moveAmountY = -Speed * Time.deltaTime;
        }

        rigid.velocity = new Vector2(moveAmountX, moveAmountY);
    }
}
