using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private Golem golemScript;

    public GameObject[] monster;

    void Start()
    {
        golemScript = GameObject.Find("Golem").GetComponent<Golem>();
    }

    // 스킬버튼함수
    public void Golem_HandCrash(float cost)
    {
        golemScript.HandCrash(cost);
    }

    public void Golem_RollingThunder(float cost)
    {
        golemScript.RollingThunder(cost);
    }

    public void Golem_RocketPunch(float cost)
    {
        golemScript.RocketPunch(cost);
    }

    // 소환버튼 함수
    public void Spawn(int index)
    {
        float cost;

        if(index == 0) {
            cost = 30f;
        }
        else if(index == 1) {
            cost = 40f;
        }
        else{
            cost = 30f;
        }

        if(golemScript.currentSP < cost) {
            return;
        }
        GameObject _camera = GameObject.Find("Main Camera");
        Vector3 spawnPosition = new Vector3(_camera.transform.position.x - 11f, -7f);

        GameObject _monster = Instantiate(monster[index]);
        _monster.transform.position = spawnPosition;

        golemScript.currentSP -= cost;
    }
    

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

    void HitEnd()
    {
        golemScript.HitEnd();
    }

    void AfterDeath()
    {
        golemScript.AfterDeath();
    }

}
