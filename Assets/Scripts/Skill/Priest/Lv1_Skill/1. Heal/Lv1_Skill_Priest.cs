using UnityEngine;
using System.Collections;
/// <summary>
/// 사제 스킬: 체력 회복
/// 1체인: 20 회복
/// 2체인: 60 회복
/// 3체인: 180 회복
/// </summary>


public class Lv1_Skill_Priest : Skill
{
    public int heal;
    public static float EndTime;
    public GameObject EffectPrefab;

    public override void Activation(int chainNum)
    {
        for (int i = 0; i < GameManager.instance.playerList.Length; i++)
        {
            Player player = GameManager.instance.playerList[i];

            GameObject effectPrefab = Instantiate(EffectPrefab,
                new Vector3(player.transform.position.x, 3.75f, 0), Quaternion.Euler(-90f, 0f, 0f));

            effectPrefab.transform.parent = GameManager.instance.EffectHolder.transform;

            if (player.Hp > 0
                && player.Hp < player.InitHp)
            {
                EndTime = 1.0f + Time.fixedTime;

                player.Hp += heal * (int)Mathf.Pow(2, chainNum);

                if (player.Hp > player.InitHp) player.Hp = player.InitHp;

                player.StartCoroutine(HealHUD(player, 1.0f, EndTime));

                player.HpBarUpdate();
            }
        }
    }

    IEnumerator HealHUD(Character target, float time, float endTime)
    {
        HUDController HUDcontroller = target.HpHUD.GetComponent<HUDController>();

        HUDcontroller.Status[0].SetActive(true);

        yield return new WaitForSeconds(time);

        if (endTime - EndTime + 0.1f < Mathf.Epsilon) yield break;

        HUDcontroller.Status[0].SetActive(false);
    }
}
