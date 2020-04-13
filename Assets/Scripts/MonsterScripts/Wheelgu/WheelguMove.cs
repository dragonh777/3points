using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WheelguMove : MonoBehaviour
{
    public float WheelguSpeed = 1f;
    public float forceGravity = 1f;

    public Transform transform;
    public Transform target;

    public Transform Wheelgu_Pos;
    public float radius = 1f;

    public CircleCollider2D collider;

    private Rigidbody2D rigid;
    private Vector3 moveAmount;

    bool rangeOn;

    private bool isMove = false;

    float dir;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        rangeOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        WheelMove();
        rangeOn = Physics2D.OverlapCircle(Wheelgu_Pos.position, radius, 1 << LayerMask.NameToLayer("Player"));
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (dir > 0)
                dir = 1f;
            else
                dir = -1f;
        }
    }
    void WheelMove()
    {
        if (isMove)
        {
            dir = target.position.x - transform.position.x;
            if (dir < 0)
            {
                moveAmount = WheelguSpeed * Vector3.left * Time.deltaTime;
            }
            else if (dir > 0)
            {
                moveAmount = WheelguSpeed * Vector3.right * Time.deltaTime;
            }
            transform.Translate(moveAmount);
        }
    }
}
