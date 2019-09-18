using UnityEngine;
/// <summary>
/// 과학자 스킬: 독극물 투척
/// 1체인: 10 데미지 X 3번
/// 2체인: 10 데미지 X 6번
/// 3체인: 10 데미지 X 12번
/// </summary>
public class Lv5_Skill_Scientist : Skill {

    public int damage;
    public Poison _Poison;

    public override void Activation(int chainNum)
    {
        Poison poison = Instantiate(_Poison,
            GameManager.instance.playerList[1].transform.position, // 과학자 위치
            Quaternion.identity);

        poison.Damage = damage;
        poison.ChainNum = chainNum;


        poison.transform.parent = GameManager.instance.EffectHolder.transform;
    }
}
