using UnityEngine;
/// <summary>
/// 사제 스킬: 파이어 볼
/// 1체인: 20 데미지
/// 2체인: 40 데미지
/// 3체인: 80 데미지
/// </summary>



public class Lv5_Skill_Priest : Skill
{
    public int damage;
    public float speed;

    public Fire _Fire;

    public override void Activation(int chainNum)
    {
        Fire fire = Instantiate(_Fire,
            GameManager.instance.playerList[2].transform.position, // 사제 위치
            Quaternion.identity);

    //    fire.transform.parent = GameManager.instance.playerList[2].transform;

        fire.Damage = damage;
        fire.ChainNum = chainNum;
        fire.speed = speed;
        fire.transform.parent = GameManager.instance.EffectHolder.transform;


    }
}