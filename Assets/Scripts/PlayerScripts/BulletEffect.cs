using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEffect : MonoBehaviour
{
    
    private GameObject pdir;
    // Start is called before the first frame update
    void Start()
    {
        pdir = GameObject.Find("Player");
        if (pdir.transform.localScale.x > 0f)
        {
            this.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (pdir.transform.localScale.x < 0f)
        {
            this.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetButtonDown("BasicAttack"))
        //{
        //    Ttime = 0f;
        //    this.gameObject.SetActive(true);
        //}

        //Ttime += Time.deltaTime;

        //if(Ttime > 3f)
        //{
        //    this.gameObject.SetActive(false);
        //}
        Destroy(gameObject, 0.3f);
    }
}
