using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

public class PlayerHp : MonoBehaviour
{

    [Header("Animations")]
    [SpineAnimation]
    public string left;
    [SpineAnimation]
    public string leftDamage;
    [SpineAnimation]
    public string leftRestr;
    [SpineAnimation]
    public string right;
    [SpineAnimation]
    public string rightDamage;
    [SpineAnimation]
    public string rightRestr;

    public SkeletonAnimation[] skeletonAnimation;

    private int cnt = 5;

    public static bool pHit = false;
    public static bool pHit2 = false;

    private bool restr = false;

    private float resTime = 0f;
    private float regen = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (pHit)
        {
            pHit = false;
            cnt = Players.HP;
        }
        if (pHit2)
        {
            cnt++;
            pHit2 = false;
            if (cnt % 2 == 0)
            {
                skeletonAnimation[cnt].state.SetAnimation(0, rightDamage, false);
                skeletonAnimation[cnt].gameObject.GetComponent<MeshRenderer>().sortingOrder = 6;
            }

            else
            {
                skeletonAnimation[cnt].state.SetAnimation(0, leftDamage, false);
                skeletonAnimation[cnt].gameObject.GetComponent<MeshRenderer>().sortingOrder = 6;
            }
            cnt--;
        }
        if (Players.HP == cnt && cnt >= 0)
        {
            if (cnt <= -1)
                cnt = 0;
            if (cnt%2 == 0)
            {
                skeletonAnimation[cnt].state.SetAnimation(0, leftDamage, false);
                skeletonAnimation[cnt].gameObject.GetComponent<MeshRenderer>().sortingOrder = 6;
            }

            else
            {
                skeletonAnimation[cnt].state.SetAnimation(0, rightDamage, false);
                skeletonAnimation[cnt].gameObject.GetComponent<MeshRenderer>().sortingOrder = 6;
            }
            
            cnt--;
        }
        
        if (Players.isHit)
        {
            regen = 0f;
        }
        if (regen > 3.0f && Players.HP > 0)
        {
            regen = 0f;
            if (cnt < 5)
            {
                Players.HP++;
                cnt++;
                if (cnt % 2 == 0)
                {
                    skeletonAnimation[cnt].state.SetAnimation(0, leftRestr, false);
                    resTime = 0f;
                }

                else
                {
                    skeletonAnimation[cnt].state.SetAnimation(0, rightRestr, false);
                    resTime = 0f;
                }
            }
        }
        

        if (resTime > 0.4f && cnt > -1)
        {
            skeletonAnimation[cnt].gameObject.GetComponent<MeshRenderer>().sortingOrder = 4;
        }
        

        resTime += Time.deltaTime;
        regen += Time.deltaTime;
    }
}
