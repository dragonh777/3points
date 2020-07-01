using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogSummon : MonoBehaviour
{
    public GameObject Fog;
    public float xRange = 0f;
    public float minYRange = 0f;
    public float maxYRange = 1f;

    private float fTime = 0f;
    private float offsetY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        offsetY = Random.Range(minYRange, maxYRange);
        Instantiate(Fog, new Vector3(xRange, offsetY, 0), Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        offsetY = Random.Range(minYRange, maxYRange);
        fTime += Time.deltaTime;
        if (fTime > 10f)
        {
            Instantiate(Fog, new Vector3(xRange, offsetY, 0), Quaternion.identity);
            fTime = 0f;
        }
    }
}
