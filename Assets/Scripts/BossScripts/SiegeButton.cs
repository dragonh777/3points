using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiegeButton : MonoBehaviour
{
    public GameObject[] monster = new GameObject[3];
    private Golem golemScript;
    private ButtonCoolTime[] coolTimeScript = new ButtonCoolTime[6];
    public bool isAttack = false;

    void Start()
    {
        golemScript = GameObject.Find("Golem").GetComponent<Golem>();

        for(int i = 0; i < 6; i++) {
            if(gameObject.name != "GameManager") {
                break;
            }

            if (i < 3) {
                coolTimeScript[i] = GameObject.Find("SkillBtn" + i.ToString()).GetComponent<ButtonCoolTime>();
            }
            else {
                coolTimeScript[i] = GameObject.Find("SpawnBtn" + (i - 3).ToString()).GetComponent<ButtonCoolTime>();
            }

        }
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Mouse0)) {  // 좌클릭, 내려찍기
        //    Golem_HandCrash(20f);
        //}
        //else if (Input.GetKeyDown(KeyCode.Mouse1)) {   // 우클릭, 구르기
        //    Golem_RollingThunder(30f);
        //}
        //else if (Input.GetKeyDown(KeyCode.E)) {  // E, 펀치
        //    Golem_RocketPunch(50f);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha1)) {  // 1, 소환1
        //    Spawn(0);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2)) {  // 2, 소환2
        //    Spawn(1);
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha3)) {  // 3, 소환3
        //    Spawn(2);
        //}

    }

    // 스킬버튼함수
    public void Golem_HandCrash(float cost)
    {
        if(isAttack || golemScript.currentSP < cost) {
            return;
        }
        golemScript.HandCrash(cost);
        coolTimeScript[0].StartCoolTime(true);
    }

    public void Golem_RollingThunder(float cost)
    {
        if(isAttack || golemScript.currentSP < cost) {
            return;
        }
        golemScript.RollingThunder(cost);
        coolTimeScript[1].StartCoolTime(true);
    }

    public void Golem_RocketPunch(float cost)
    {
        if (isAttack || golemScript.currentSP < cost) {
            return;
        }
        golemScript.RocketPunch(cost);
        coolTimeScript[2].StartCoolTime(true);
    }

    // 소환버튼 함수
    public void Spawn(int index)
    {
        float cost;
        Vector3 spawnPosition;
        GameObject _player = GameObject.Find("Player");
        GameObject _golem = GameObject.Find("Golem");
        GameObject _monster = monster[index];
        float dirX = _player.transform.position.x - _golem.transform.position.x;

        if(index == 0) {    // 휠구
            if(dirX >= 0) {  // 적이 오른쪽에 있을 때
                spawnPosition = new Vector3(_golem.transform.position.x + 3f, -12f);
            }
            else {
                spawnPosition = new Vector3(_golem.transform.position.x - 3f, -12f);
            }
            cost = 30f;
        }
        else if(index == 1) {   // 애시드팟
            if (dirX >= 0) {  // 적이 오른쪽에 있을 때
                spawnPosition = new Vector3(_golem.transform.position.x + 3f, -12f);
            }
            else {
                spawnPosition = new Vector3(_golem.transform.position.x - 3f, -12f);
            }
            cost = 40f;
        }
        else {  //밤시드
            spawnPosition = new Vector3(_golem.transform.position.x - 11f, -12f);
            cost = 30f;
        }

        if(golemScript.currentSP < cost) {
            return;
        }

        _monster.transform.position = spawnPosition;
        Instantiate(_monster);

        golemScript.currentSP -= cost;
        coolTimeScript[index + 3].StartCoolTime(false);
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

    void EndRollingEndMotion()
    {
        golemScript.EndRollingEndMotion();
    }

    void ShootRocketPunch()
    {
        golemScript.ShootRocketPunch();
    }

    void CrashBoundActive()
    {
        golemScript.CrashBoundActive();
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
