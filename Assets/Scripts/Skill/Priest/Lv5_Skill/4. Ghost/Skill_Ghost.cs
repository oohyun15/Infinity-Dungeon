using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//문제점 : 부딪힌 적들에게 순서대로 디버프를 적용하다보니까 endTime이 계속 증가해서 디버프가 끝나지 않는다.
//  일단 연속으로 이 스킬에 부딪혀도 속박시간이 덮어쓰기가 안되도록 해놓음 // 수정 필요


// 사제 지박령 소환 : 지박령과 부딪힌 적에게 피해를 주고 움직임을 멈춘다.
// 1체인 : 데미지 20  속박 시간 1초
// 2체인 : 데미지 30  속박 시간 2초
// 3체인 : 데미지 40  속박 시간 3초

public class Skill_Ghost : Skill
{
    public float speed;
    public int damage;
    public float time;

    public Ghost _ghost;
    

    public override void Activation(int chainNum)
    {
        Ghost ghost = Instantiate(_ghost, GameManager.instance.playerList[2].transform.position + Vector3.down * 0.8f,
            Quaternion.identity);

        ghost.Speed = speed;
        ghost.Damage = damage;
        ghost.ChainNum = chainNum;
        ghost.timer = time;

        ghost.transform.parent = GameManager.instance.EffectHolder.transform;
    }
}

