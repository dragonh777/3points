using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed;
    public float ShortjumpPower;
    public float jumpPower;
    public float checkRaius;
    public float BulletPos;
    public Slider healthBarSlider;
    public GameObject bossPanel;
    public GameObject startPanel;

    public int jumpMax;
    public int HpMax;

    public GameObject Bullet;
    public Transform pos;
    public LayerMask isLayer;

    public static bool Pleft;
    public static int Hp;

    private int jumpCnt;
    private float BulletP;
    private float jumpTimeCounter;
    private bool isGround;
    //private bool isJump;
    //private bool jumpCancel;

    Rigidbody2D rigid;
    SpriteRenderer renderer;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        jumpCnt = jumpMax;
        BulletP = BulletPos;
        Hp = HpMax;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        isGround = Physics2D.OverlapCircle(pos.position, checkRaius, isLayer);
        if (!isGround)
        {
            anim.SetBool("isJump", true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            
            jumpCnt--;
            if (jumpCnt > 0)
            {
                if (!isGround)
                {
                    
                    jumpCnt--;
                    //isJump = true;
                    Jump();
                }
                //isJump = true;
                Jump();
            }
        }
        //if (Input.GetKeyUp(KeyCode.C) && !isGround)
        //{
        //    jumpCancel = true;
        //}
        if (isGround)
        {
            jumpCnt = jumpMax;
            anim.SetBool("isJump", false);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(Bullet, transform.position + new Vector3(BulletP, 0, 0), Quaternion.identity);
        }
        if (Hp == 0)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && healthBarSlider.value > 0)
        {
            healthBarSlider.value--;
            Debug.Log("Hit");
        }
        if (collision.gameObject.tag == "Portal")
        {
            startPanel.gameObject.SetActive(false);
            bossPanel.gameObject.SetActive(true);
        }
    }

    void Move()
    {
        Vector3 moveVelocity = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveVelocity = Vector3.left;
            renderer.flipX = true;
            anim.SetBool("isWalk", true);
            Pleft = true;
            BulletP = -BulletPos;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            renderer.flipX = false;
            anim.SetBool("isWalk", true);
            Pleft = false;
            BulletP = BulletPos;
        }

        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("isWalk", false);
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }
    
    void Jump()
    {
        //if (isJump)
        //{
            rigid.velocity = Vector2.zero;
            Vector2 jumpVelocity = new Vector2(0, jumpPower);
            rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        //    isJump = false;
        //}
        //if (jumpCancel)
        //{
        //    if (rigid.velocity.y > ShortjumpPower)
        //    {
        //        rigid.velocity = Vector2.zero;
        //        Vector2 jumpVelocity = new Vector2(0, ShortjumpPower);
        //        rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
        //        jumpCancel = false;
        //    }
        //}
        //if(jumpTimeCounter > 0)
        //{
        //    Debug.Log("jump");
        //    rigid.velocity = Vector2.up * jumpPower;
        //    jumpTimeCounter -= Time.deltaTime;
        //}
    }
}
