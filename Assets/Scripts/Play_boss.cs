using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play_boss : PB_charater
{
    [SerializeField]            //sp
    private Stat sp;

    public float maxSp = 100f;

    public static float initsp = 100;

    //private Vector2 direction;
    public static bool pLeft;
    SpriteRenderer renderer;                //스프라이트렌더러
    private Rigidbody2D rigid;
    private Vector3 moveAmount;

    [SerializeField]
    private GameObject[] MonsterPrefab;
    // Start is called before the first frame update
    protected override void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        sp.Initiallize(initsp, initsp);

        initsp = maxSp;

        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        GetInput();
        base.Update();
    }

    public void GetInput()
    {

        Vector3 dir = Vector3.zero;                         //방향을 넣을 dir을 넣고 0으로 초기화

        if (Input.GetAxisRaw("Horizontal") < 0)             //방향입력이 0보다 작을때(-1일때, 즉 왼쪽일때)
        {
            dir = Vector3.left;                             //왼쪽 방향을 넣어줌
            renderer.flipX = true;                         //flipX를 꺼서 방향은 그대로
            pLeft = true;                                   //플레이어가 왼쪽인지 판별하는 bool 함수에 true 넣어줌
        }

        else if (Input.GetAxisRaw("Horizontal") > 0)        //방향입력이 0보다 클때
        {
            dir = Vector3.right;                            //오른쪽 방향
            renderer.flipX = false;                          //flipX를 켜서 보는 방향을 바꿈
            pLeft = false;                                  //오른쪽 판별을 위해 끔
        }
        transform.Translate(moveAmount);
        moveAmount = dir * speed * Time.deltaTime;
    }

    public void Summon()
    {
        Instantiate(MonsterPrefab[0], transform.position, Quaternion.identity);
    }
}
