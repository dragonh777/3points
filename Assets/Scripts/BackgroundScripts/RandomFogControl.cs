using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFogControl : MonoBehaviour
{
    public float maxFogSpeed = 0.6f;

    private float speed = 0f;
    Vector3 moveAmount;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(1, maxFogSpeed);

        moveAmount = speed * Vector3.left * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.Translate(moveAmount);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WorldOut")
        {
            Destroy(gameObject);
        }
    }

}
