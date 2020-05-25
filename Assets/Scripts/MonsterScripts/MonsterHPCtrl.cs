using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 스크립터블 오브젝트 사용해보기(지금은 HP바 조절을 각 몬스터의 Update()에서 조절하고있음
public class MonsterHPCtrl : MonoBehaviour
{
    public Image hpbar;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(monster.position.x, monster.position.y + 1f, monster.position.z);

        BombSeed_HPControl();
    }

    void BombSeed_HPControl()
    {
        hpbar.fillAmount = BombSeeed.HP / 100f;
    }
}
