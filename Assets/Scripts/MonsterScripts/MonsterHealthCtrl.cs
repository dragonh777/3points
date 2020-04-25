using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthCtrl : MonoBehaviour
{
    public Transform monster;
    public Slider healthBar;
    public CircleCollider2D coll;
    public Rigidbody2D rigid;

    private Transform transform;
    private bool isDead = false;
    private int cnt = 0;
    private void Awake()
    {
        transform = GetComponent<Transform>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && healthBar.value > 0)
        {
            healthBar.value -= 10;
        }

        if (healthBar.value <= 0 && !isDead)
        {
            if(cnt == 0)
                isDead = true;
            coll.isTrigger = true;
        }
        transform.position = new Vector3(monster.position.x, monster.position.y + 1f, monster.position.z);

        if (isDead)
        {
            rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
            isDead = false;
            cnt++;
        }

    }
}
