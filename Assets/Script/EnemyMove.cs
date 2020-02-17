using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    //public float EmoveSpeed;
    public float eBulletPosX;
    public float eBulletPosY;
    public float eSkillPosX;
    public float eSkillPosY;
    public float bulletDelay;
    public Slider healthBarSlider;

    public int EHpMax;

    public GameObject Bullet;
    public GameObject Skill;

    public static int Hp;

    private float BulletP;
    private float Bspeed;

    private float TimeLeft;
    private float nextTime = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        TimeLeft = bulletDelay;
        BulletP = eBulletPosX;
        Hp = EHpMax;
    }

    // Update is called once per frame
    void BulletLauncher ()
    {
        Bspeed = Random.Range(-eBulletPosY, eBulletPosY);
        Instantiate(Bullet, new Vector3(BulletP, Bspeed, 0), Quaternion.identity);
    }
    public void Skill1Active()
    {
        Instantiate(Skill, new Vector3(eSkillPosX, 0, 0), Quaternion.identity);
    }
    IEnumerator skill1()
    {
        Instantiate(Skill, new Vector3(eSkillPosX, 0, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.0f);
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
