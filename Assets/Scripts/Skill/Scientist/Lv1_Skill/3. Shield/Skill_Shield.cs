using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 과학자 비상 방벽 작동 : 비상 방벽을 내려 적들이 더이상 접근하지 못하게 하고 파괴될 때 적들에게 데미지를 준다.
// 1체인 지속 시간 1초 폭발데미지 20
// 2체인 지속 시간 2초 폭발데미지 40
// 3체인 지속 시간 3초 폭발데미지 60

public class Skill_Shield : Skill {

    public Shield shield;

    public override void Activation(int chainNum)
    {

        int PositionX = Random.Range(5, 9);

        Shield _shield = Instantiate(shield, new Vector3(PositionX, 4.35f, 0.0f), Quaternion.identity);

        _shield.ChainNum = chainNum;
        _shield.transform.parent = GameManager.instance.EffectHolder.transform;    
    }
            
        
   


   
}
