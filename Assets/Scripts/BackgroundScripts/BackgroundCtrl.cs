using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCtrl : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public Transform transform;
    Material material;

    private float dir;
    private float tr1;

    private Vector3 previousCamPos;

    private float OffsetX;
    void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start()
    {
        tr1 = transform.position.x;
        previousCamPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        float paral = (previousCamPos.x - transform.position.x) * -scrollSpeed;

        dir = Input.GetAxisRaw("Horizontal");
        if (transform.position.x > tr1)
        {
            tr1 = transform.position.x;
            OffsetX = material.mainTextureOffset.x + paral * Time.deltaTime;
        }
        else if (transform.position.x < tr1)
        {
            tr1 = transform.position.x;
            OffsetX = material.mainTextureOffset.x + paral * Time.deltaTime;
        }

        Vector2 Offset = new Vector2(OffsetX, 0);

        material.mainTextureOffset = Offset;

        previousCamPos = transform.position;
        
    }
}
