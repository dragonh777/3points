using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    private int cnt = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Return))
        {
            if (cnt == 0)
            {
                Players.isDie = false;
                SceneManager.LoadScene("Tutorial_Operation");
            }
            if (cnt == 1)
            {
                Players.isDie = false;
                SceneManager.LoadScene("Tutorial_Siege");
            }
            if (cnt == 3)
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            cnt++;
            if (cnt > 3)
            {
                transform.position = new Vector3(0, 0, 0);
                cnt = 0;
            }
            else 
                transform.position = new Vector3(0, transform.position.y - 0.825f, 0);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            cnt--;
            if (cnt < 0)
            {
                transform.position = new Vector3(0, -2.475f, 0);
                cnt = 3;
            }
            else
                transform.position = new Vector3(0, transform.position.y + 0.825f, 0);
        }
    }
}
