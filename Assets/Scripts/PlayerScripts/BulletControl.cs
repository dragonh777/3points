using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletControl : MonoBehaviour
{
    // Bullet프리팹 스크립트
    // 구현된 기능
    //마우스 방향으로 날아감
    // 데미지, 속도 가지고 있음

    // 구현해야할 기능
    // 날아가는 방향으로 총알 회전시키기

    public float bulletSpeed = 30f;
    public int bulletDamage = 35;

    public static int bDamage;

    Vector2 dir;

    // Start is called before the first frame update
    void Start()    // Bullet프리팹 생성시 초기화
    {
        bDamage = bulletDamage;
        dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position);
    }

    // Update is called once per frame
    void FixedUpdate()  // Bullet프리팹 생성 후 날아가게 하기
    {
        GetComponent<Transform>().transform.Translate(dir.normalized * bulletSpeed * Time.deltaTime);

        Destroy(gameObject, 5); // 5초 후 자동 Destroy
    }

    void OnCollisionEnter2D(Collision2D collision)  // 날아가다 충돌시
    {
        if (collision.gameObject.tag == "Wall"|| collision.gameObject.tag == "Floor")   // 태그 Wall이나 Floor에닿으면 Destroy
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.tag == "Enemy")   // 태그 Enemy에 닿으면
        {
            Debug.Log("Hit");
            Destroy(gameObject);
            EnemyMove.Hp -= bDamage;
        }
    }
}
