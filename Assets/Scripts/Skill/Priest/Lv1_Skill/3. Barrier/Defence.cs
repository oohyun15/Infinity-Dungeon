using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 사제 보호 : 아군의 방어력을 증가시켜 받는 피해를 줄인다.
// 1체인 : 받는 피해가 20% 감소한다.
// 2체인 : 받는 피해가 40% 감소한다.
// 3체인 : 받는 피해가 60% 감소한다.

public class Defence : Skill
{

    private static float EndTime;
    public GameObject EffectPrefab;

    public static GameObject effectPrefab;

    public override void Activation(int chainNum)
    {
        float time = Mathf.Pow(2.0f, chainNum);

        if (time + Time.fixedTime - EndTime > Mathf.Epsilon)
        {
            EndTime = time + Time.fixedTime;

            for (int i = 0; i < GameManager.instance.playerList.Length; i++)
            {
                Player player = GameManager.instance.playerList[i];

                // 중복 허용을 안되게 해놔서 가장 앞에 있는 플레이어만 걸리게 된다.
                if (player.Hp > 0)
                {
                    GameManager.instance.StartCoroutine(Defending(player, time, EndTime, chainNum));

                    break;
                }
            }
        }
    }

    IEnumerator Defending(Player player, float time, float endTime, int chainNum)
    {
        HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

        if (effectPrefab == null)
        {
            effectPrefab = Instantiate(EffectPrefab, player.transform.position, Quaternion.identity);

            effectPrefab.transform.parent = GameManager.instance.EffectHolder.transform;
        }
        HUDcontroller.Status[6].SetActive(true);

        player.DefensiveRatio = player.InitDefensiveRatio - (0.2f * chainNum);

        yield return new WaitForSeconds(time);

        if (endTime < EndTime) yield break;

        HUDcontroller.Status[6].SetActive(false);

        Destroy(effectPrefab);

        player.DefensiveRatio = player.InitDefensiveRatio;

    }
}
