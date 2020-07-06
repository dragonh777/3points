using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class TextControl : MonoBehaviour
{
    public GameObject currentKey;
    public GameObject nextKey;
    public GameObject dialog;
    public GameObject[] marker;
    public static bool isMove = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isMove = true;
            this.gameObject.SetActive(false);
            if (currentKey.name == "ADKey")
            {
                dialog.transform.localPosition = new Vector3(0, 80, 0);
            }
            if (currentKey.name == "SpaceKey")
            {
                marker[0].SetActive(true);
                dialog.transform.localPosition = new Vector3(850, -300, 0);
            }
            if (currentKey.name == "ShiftKey")
            {
                marker[0].SetActive(false) ;
                marker[1].SetActive(true);
                dialog.transform.localPosition = new Vector3(730, -300, 0);
            }
            if (currentKey.name == "AttackKey")
            {
                marker[0].SetActive(false);
            }
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
