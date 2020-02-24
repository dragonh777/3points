using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //Move변수
    public float moveSpeed = 6f;
    public static bool pLeft;

    //Jump변수
    public float shortJumpPower = 10f;
    public float jumpPower = 14f;
    public float checkRaius = 0.3f;
    public int jumpMax = 2;
    public Transform pos;

    private int jumpCnt;
    private float jumpTimeCounter;
    private bool isGround;

    //Skill변수
    public GameObject bullet;
    public float bulletPos = 1f;
    private float bulletP;

    //Hp변수
    public Slider healthBarSlider;
    public static int Hp;
    public int hpMax = 3;

    public GameObject bossPanel;
    public GameObject startPanel;
    public LayerMask isLayer;

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
        bulletP = bulletPos;
        Hp = hpMax;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(bullet, transform.position + new Vector3(bulletP, 0, 0), Quaternion.identity);
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
            pLeft = true;
            bulletP = -bulletPos;
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveVelocity = Vector3.right;
            renderer.flipX = false;
            anim.SetBool("isWalk", true);
            pLeft = false;
            bulletP = bulletPos;
        }

        else if (Input.GetAxisRaw("Horizontal") == 0)
        {
            anim.SetBool("isWalk", false);
        }

        transform.position += moveVelocity * moveSpeed * Time.deltaTime;
    }

    void Jump()
    {
        isGround = Physics2D.OverlapCircle(pos.position, checkRaius, isLayer);
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
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
                    rigid.velocity = Vector2.zero;
                    rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
                }
                rigid.velocity = Vector2.zero;
                rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
            }
        }
        if (isGround)
        {
            jumpCnt = jumpMax;
            anim.SetBool("isJump", false);
        }
    }
}
