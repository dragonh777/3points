using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    public GameObject currentKey;
    public GameObject nextKey;
    public static bool isMove = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isMove = true;
            this.gameObject.SetActive(false);
            currentKey.SetActive(false);
            nextKey.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
