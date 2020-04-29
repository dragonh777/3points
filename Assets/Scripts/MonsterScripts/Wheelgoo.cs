using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelgoo : MonoBehaviour
{
    public float Speed = 100f;
    public Transform player;    // 플레이어 위치받기

    private Rigidbody2D rigid;
    private Transform transform;    // 자신 위치받기

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
        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveAmount = 0;
        float dirX = player.position.x - transform.position.x;


        if(dirX > 0) {
            moveAmount = Speed * Time.deltaTime;
        }
        else if(dirX < 0) {
            moveAmount = -Speed * Time.deltaTime;
        }

        rigid.velocity = new Vector2(moveAmount, rigid.velocity.y);
    }
}
