using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        TentacleHPbar();
    }

    void TentacleHPbar()
    {
        float HP = Tentacle.HP;    // EnemyMove에 설정된 HP값 받아옴
        hpbar.fillAmount = HP / 100f;
    }
}
