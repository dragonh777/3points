using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FogControl : MonoBehaviour
{
    public float fogSpeed;

    private Transform currentTransform;
    private Transform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTransform = transform;
        currentTransform.position = new Vector3(currentTransform.position.x + fogSpeed * Time.deltaTime, currentTransform.position.y, currentTransform.position.z);
        transform = currentTransform;
    }
}
