using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCtrl : MonoBehaviour
{
    public float xScrollSpeed = 0.5f;
    public float yScrollSpeed = 0.01f;
    public Transform transform;
    Material material;

    private float trX;
    private float trY;

    private Vector3 previousCamPos;

    private float OffsetX;
    private float OffsetY;
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        trX = transform.position.x;
        trY = transform.position.y;
        previousCamPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        float paralX = (previousCamPos.x - transform.position.x) * -xScrollSpeed;
        float paralY = (previousCamPos.y - transform.position.y) * -yScrollSpeed;

        if (transform.position.x > trX)
        {
            trX = transform.position.x;
            OffsetX = material.mainTextureOffset.x + paralX * Time.deltaTime;
        }
        else if (transform.position.x < trX)
        {
            trX = transform.position.x;
            OffsetX = material.mainTextureOffset.x + paralX * Time.deltaTime;
        }

        if (transform.position.y > trY)
        {
            trY = transform.position.y;
            OffsetY = material.mainTextureOffset.y + paralY * Time.deltaTime;
        }
        else if (transform.position.y < trY)
        {
            trY = transform.position.y;
            OffsetY = material.mainTextureOffset.y + paralY * Time.deltaTime;
        }

        Vector2 Offset = new Vector2(OffsetX, OffsetY);

        material.mainTextureOffset = Offset;

        previousCamPos = transform.position;
        
    }
}
