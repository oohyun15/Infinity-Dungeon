using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 에이전트 아이언 슈트 : 방어력 증가 (받는 피해 감소)
// 1체인 : 3초간 받는 피해 30% 감소
// 2체인 : 3초간 받는 피해 60% 감소
// 3체인 : 3초간 받는 피해 90% 감소

public class IronSuit : Skill
{
    private float timer = 3.0f;
    private static float EndTime;
    public GameObject EffectPrefab;

    public override void Activation(int chainNum)
    {
        Player player = GameManager.instance.playerList[0];

        GameObject effectPrefab = Instantiate(EffectPrefab, player.transform.position, Quaternion.identity);

        effectPrefab.transform.parent = GameManager.instance.EffectHolder.transform;

        float time = timer + Time.fixedTime;

        if (time - EndTime > Mathf.Epsilon)
        {
            EndTime = time;

            if (player.Hp > 0) GameManager.instance.StartCoroutine(DefenceUp(player, EndTime, chainNum));
        }
    }

    public IEnumerator DefenceUp(Player player, float endTime, int ChainNum)
    {
        HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

        player.DefensiveRatio = player.InitDefensiveRatio - (0.3f * ChainNum);

        HUDcontroller.Status[6].SetActive(true);

        yield return new WaitForSeconds(timer);

        if (endTime < EndTime) yield break;

        player.DefensiveRatio = player.InitDefensiveRatio;

        HUDcontroller.Status[6].SetActive(false);
    }

}