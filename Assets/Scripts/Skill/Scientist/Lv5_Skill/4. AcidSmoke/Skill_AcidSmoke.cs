using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 과학자 산성 연막탄 : 연막탄을 던져 데미지를 주면서 동시에 적의 공격이 빗나갈 확률을 높인다.
// 1체인 : 실명 확률 50% 데미지 5 X 3
// 2체인 : 실명 확률 66% 데미지 5 X 6
// 3체인 : 실명 확률 75% 데미지 5 X 12

public class Skill_AcidSmoke : Skill {


    public int damage;
    public AcidSmoke _acidSmoke;

    public override void Activation(int chainNum)
    {
        AcidSmoke acidSmoke = Instantiate(_acidSmoke,
            GameManager.instance.playerList[1].transform.position, // 과학자 위치
            Quaternion.identity);

        acidSmoke.Damage = damage;

        acidSmoke.ChainNum = chainNum;

        acidSmoke.transform.parent = GameManager.instance.EffectHolder.transform;
    }
}
