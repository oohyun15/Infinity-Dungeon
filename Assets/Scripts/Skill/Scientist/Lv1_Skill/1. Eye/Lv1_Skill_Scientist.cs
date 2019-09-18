using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 과학자 스킬: 데미지 2배 증가
/// 1체인: 2초, 2배
/// 2체인: 4초, 2배
/// 3체인: 8초, 2배
/// </summary>


public class Lv1_Skill_Scientist : Skill
{
    public GameObject EffectPrefab;

    private static float EndTime;

    public override void Activation(int chainNum)
    {
        Player player = GameManager.instance.playerList[1];
        
        // 이펙트는 스킬 발동 시 항상 생기지만 스킬이 중복되서 실행되진 않는다.
        GameObject effectPrefab = Instantiate(EffectPrefab, player.transform.position, Quaternion.Euler(180f, 0f, 0f));

        effectPrefab.transform.parent = GameManager.instance.EffectHolder.transform;

        float time = Mathf.Pow(2.0f, chainNum);

        if (time + Time.fixedTime - EndTime > Mathf.Epsilon)
        {
            EndTime = time + Time.fixedTime;

            if (player.Hp > 0) GameManager.instance.StartCoroutine(AttackDamage(player, time, EndTime));
        }
    }

    IEnumerator AttackDamage(Player player, float time, float endTime)
    {
        HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

        HUDcontroller.Status[2].SetActive(true);

        player.Damage = player.InitDamage * 2; // 공격력 2배

        yield return new WaitForSeconds(time);

        if (endTime < EndTime) yield break;

        HUDcontroller.Status[2].SetActive(false);

        player.Damage = player.InitDamage;
    }
}
