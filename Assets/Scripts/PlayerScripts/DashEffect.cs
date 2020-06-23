using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public float scaleX = 0.25f;
    public float scaleY = 0.25f;
    public float offX = 0.8f;
    public float offY = 0.6f;

    private GameObject pdir;
    private Transform transform;
    private Vector3 Amount;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        pdir = GameObject.Find("Player");
        if (pdir.transform.localScale.x > 0f)
        {
            this.transform.localScale = new Vector3(scaleX, scaleY, 1);
            Amount = new Vector3(offX, offY, 0);
        }
        else if (pdir.transform.localScale.x < 0f)
        {
            this.transform.localScale = new Vector3(-scaleX, scaleY, 1);
            Amount = new Vector3(-offX, offY, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pdir.transform.position + Amount;
        Destroy(gameObject, 0.25f);
    }
}
