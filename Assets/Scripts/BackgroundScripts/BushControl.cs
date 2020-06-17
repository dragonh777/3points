using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushControl : MonoBehaviour
{
    public float xScrollSpeed = 0.5f;
    public float yScrollSpeed = 0.01f;

    private Transform campos;
    private Transform transform;
    private float trX;
    private float trY;

    private Vector3 previousCamPos;

    private float OffsetX;
    private float OffsetY;


    // Start is called before the first frame update
    void Awake()
    {
        // Setting up the reference shortcut.
        campos = Camera.main.transform;
        transform = GetComponent<Transform>();
    }

    void Start()
    {
        trX = campos.position.x;
        trY = campos.position.y - 750f;
        previousCamPos = campos.position;
    }

    // Update is called once per frame
    void Update()
    {
        float paralX = (previousCamPos.x - campos.position.x) * xScrollSpeed;
        float paralY = (previousCamPos.y - campos.position.y) * yScrollSpeed;

        if (campos.position.x > trX)
        {
            trX = campos.position.x;
            OffsetX = transform.position.x + paralX * Time.deltaTime;
        }
        else if (campos.position.x < trX)
        {
            trX = campos.position.x;
            OffsetX = transform.position.x + paralX * Time.deltaTime;
        }

        if (campos.position.y > trY)
        {
            trY = campos.position.y;
            OffsetY = transform.position.y + paralY * Time.deltaTime;
        }
        else if (campos.position.y < trY)
        {
            trY = campos.position.y;
            OffsetY = transform.position.y + paralY * Time.deltaTime;
        }

        Vector3 Offset = new Vector3(OffsetX, OffsetY, transform.position.z);

        transform.position = Offset;

        previousCamPos = campos.position;
    }
}
