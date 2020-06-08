using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PB_charater : MonoBehaviour
{
    [SerializeField]
    public float speed;
    protected Vector2 direction;
    // Start is called before the first frame update
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
    }
    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
