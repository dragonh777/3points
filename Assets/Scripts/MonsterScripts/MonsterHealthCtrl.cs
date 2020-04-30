using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHealthCtrl : MonoBehaviour
{
    public GameObject monsterObject;
    public Transform monster;
    public Slider healthBar;
    public CircleCollider2D coll;
    public Rigidbody2D rigid;

    private Transform transform;

    private bool isDead = false;  // true: dead, false: alive
    
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
        transform.position = new Vector3(monster.position.x, monster.position.y + 1f, monster.position.z);

        if (Input.GetKeyDown(KeyCode.X) && healthBar.value > 0)
        {
            healthBar.value -= 10;
        }

        if (healthBar.value <= 0 && isDead == false) {
            isDead = true;
            Death();
        }
    }

    void Death()
    {
        rigid.constraints = RigidbodyConstraints2D.None;    // 두1지기전에 프리즈포지션, 로테이션 다 해제

        rigid.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);  // 튀어오르기
        coll.enabled = false;   // 해당 몬스터의 콜라이더 삭제

        Invoke("AfterDeath", 1.5f); // 1.5초후 죽은 후처리
    }
    void AfterDeath()
    {
        Destroy(monsterObject); // 해당 몬스터 삭제
        Destroy(gameObject);    // 해당 몬스터의 체력바 삭제
    }
    
}
