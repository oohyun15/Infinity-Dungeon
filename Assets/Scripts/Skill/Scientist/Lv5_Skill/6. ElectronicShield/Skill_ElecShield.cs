using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 과학자 고전압 방벽 : 고전압 방벽을 소환해 부딪힌 적에게 데미지를 주고 느려지게한다.
// 1체인 : 데미지 30 속도 감소 : 30% 지속 시간 2초
// 2체인 : 데미지 30 속도 감소 : 60% 지속 시간 4초
// 3체인 : 데미지 30 속도 감소 : 90% 지속 시간 6초

public class Skill_ElecShield : Skill {

    public ElectronicShield elShield;
    public int damage;
    public float DBtime;
    public float DownSpeed;
    
    public override void Activation(int chainNum)
    {

        float PositionX = GameManager.instance.playerList[1].transform.position.x;

        ElectronicShield _Elshield = Instantiate(elShield, new Vector3(PositionX, 2.98f, 0.0f), Quaternion.identity);

        _Elshield.ChainNum = chainNum;
        _Elshield.Damage = damage;
        _Elshield.timer = DBtime;
        _Elshield.DownSpeed = DownSpeed;
        _Elshield.transform.parent = GameManager.instance.EffectHolder.transform;
    }

    


    
}
