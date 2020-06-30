using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    Golem golemScript;

    void Start()
    {
        golemScript = GameObject.Find("Golem").GetComponent<Golem>();
    }

    // 스킬버튼함수
    public void Golem_HandCrash()
    {
        golemScript.HandCrash();
    }

    public void Golem_RollingThunder()
    {
        golemScript.RollingThunder();
    }

    public void Golem_RocketPunch()
    {
        golemScript.RocketPunch();
    }

    // 소환버튼 함수
    

    // 골렘함수
    void EndAttackMotion()
    {
        golemScript.EndAttackMotion();
    }

    void RollingMode()
    {
        golemScript.RollingMode();
    }

    void EndRollindEndMotion()
    {
        golemScript.EndRollindEndMotion();
    }

    void ShootRocketPunch()
    {
        golemScript.ShootRocketPunch();
    }

    void CrashBoundAtive()
    {
        golemScript.CrashBoundAtive();
    }

}
