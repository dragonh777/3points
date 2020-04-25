using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheelgoo : MonoBehaviour
{
    public float Speed = 10f;


    private Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveAmount;

        moveAmount = Time.deltaTime * Speed;

        rigid.velocity = new Vector2(moveAmount, rigid.velocity.y);
    }
}
