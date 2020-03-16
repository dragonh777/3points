using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    //public float EmoveSpeed;
    public float eBulletPosX;
    public float eBulletPosY;
    public float bulletDelay;
    public float coolTime;
    public Slider healthBarSlider;

    public int EHpMax;

    public GameObject Bullet;

    public static int Hp;

    private float bulletP;
    private float bSpeed;

    private float TimeLeft;
    private float nextTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        TimeLeft = bulletDelay;
        bulletP = eBulletPosX;
        Hp = EHpMax;
    }

    // Update is called once per frame
    void BulletLauncher ()
    {
        bSpeed = Random.Range(-eBulletPosY, eBulletPosY);
        Instantiate(Bullet, new Vector3(bulletP, bSpeed, 0), Quaternion.identity);
    }
    void Update()
    {

        if (Time.time > nextTime)
        {
            nextTime = Time.time + TimeLeft;
            BulletLauncher();
        }
        if (Hp <= 0)
        {
            Destroy(gameObject);
            EnemySkill_1.isClicked = false;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet"&&healthBarSlider.value>0)
        {
            healthBarSlider.value -= BulletControl.bDamage;
            Debug.Log("Hit");
        }
    }
}
