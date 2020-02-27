using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //Move변수
    public float moveSpeed = 6f;
    public float forceGravity = 1f;
    public static bool pLeft;

    //Jump변수
    public float shortJumpPower = 10f;
    public float jumpPower = 14f;
    public float checkRaius = 0.3f;
    public int jumpMax = 2;
    public Transform pos;
    int layerMask;
    private float rayAmount = 0.4f;
    public float rayAmount2 = 0.3f;
    public float rayLength = 0.1f;
    public float rayParameter = 0.3f;
    public float rayParameter2 = 0.3f;

    private int jumpCnt;

    //Skill변수
    public GameObject bullet;
    public float bulletPos = 1f;
    private float bulletP;

    //Hp변수
    public Slider healthBarSlider;
    public static int Hp;
    public int hpMax = 3;

    public GameObject currentPanel;
    public GameObject nextPanel;
    public LayerMask isFloor;
    public LayerMask Hill;

    private Rigidbody2D rigid;
    RaycastHit2D HitL, HitR, fHitL, fHitR;
    SpriteRenderer renderer;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Floor");
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
        if (collision.gameObject.tag == "RightWall")
        {
            currentPanel.gameObject.SetActive(false);
            nextPanel.gameObject.SetActive(true);
        }
    }

    void Move()
    {
        fHitL = Physics2D.Raycast(transform.position - new Vector3(rayParameter2, rayAmount2, 0), Vector3.left, rayLength);
        fHitR = Physics2D.Raycast(transform.position - new Vector3(-rayParameter2, rayAmount2, 0), Vector3.right, rayLength);

        Debug.DrawRay(transform.position - new Vector3(rayParameter2, rayAmount2, 0), Vector3.left * rayLength, Color.red, 0.1f);
        Debug.DrawRay(transform.position - new Vector3(-rayParameter2, rayAmount2, 0), Vector3.right * rayLength, Color.red, 0.1f);

        float moves = Input.GetAxis("Horizontal");

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
        if (fHitL.collider == null && fHitR.collider == null)
        {
            transform.position += moveVelocity * moveSpeed * Time.deltaTime;
        }
        if (fHitL.collider != null || fHitR.collider != null)
        {
            rigid.velocity = new Vector2(moves * moveSpeed, rigid.velocity.y);
        }

    }

    void Jump()
    {
        HitL = Physics2D.Raycast(transform.position - new Vector3(rayParameter, rayAmount, 0), Vector3.down, rayLength);
        HitR = Physics2D.Raycast(transform.position - new Vector3(-rayParameter, rayAmount, 0), Vector3.down, rayLength);
        Debug.DrawRay(transform.position - new Vector3(rayParameter, rayAmount, 0), Vector3.down * rayLength, Color.red, 0.1f);
        Debug.DrawRay(transform.position - new Vector3(-rayParameter, rayAmount, 0), Vector3.down * rayLength, Color.red, 0.1f);
        Vector2 jumpVelocity = new Vector2(0, jumpPower);
        if (HitL.collider == null|| HitR.collider == null)
        {
            Debug.Log("aa");
            anim.SetBool("isJump", true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            jumpCnt--;
            if (jumpCnt > 0)
            {
                if (HitL.collider == null|| HitR.collider == null)
                {
                    jumpCnt--;
                    rigid.velocity = Vector2.zero;
                    rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
                }
                rigid.velocity = Vector2.zero;
                rigid.AddForce(jumpVelocity, ForceMode2D.Impulse);
            }
        }
        if (HitL.collider != null|| HitR.collider != null)
        {
            jumpCnt = jumpMax;
            anim.SetBool("isJump", false);
        }
    }
}
