using System.Collections;
using UnityEngine;
/// <summary>
/// 요원 스킬: 공격 속도 증가
/// 1체인: 2배, 2초
/// 2체인: 2배, 4초
/// 3체인: 2배, 8초
/// </summary>



public class Lv5_Skill_Agent : Skill
{
    public GameObject EffectPrefab;

    private static float EndTime;

    public override void Activation(int chainNum)
    {
        Player player = GameManager.instance.playerList[0];

        GameObject effectPrefab = Instantiate(EffectPrefab,
            new Vector3(player.transform.position.x, 3.75f, 0), Quaternion.Euler(90f, 0f, 0f));

        effectPrefab.transform.parent = GameManager.instance.EffectHolder.transform;

        // float time = Mathf.Pow(2.0f, chainNum);

        float time = 4f;

        if (time + Time.fixedTime - EndTime > Mathf.Epsilon)
        {
            EndTime = time + Time.fixedTime;

            if (player.Hp > 0) GameManager.instance.StartCoroutine(AttackSpeed(player, chainNum, time, EndTime));
        }
    }
    IEnumerator AttackSpeed(Player player, int ChainNum, float time, float endTime)
    {
        HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

        switch (ChainNum)
        {
            case 1:

                player.AttackDelay = player.InitAttackDelay * 0.5f;  // 공격 속도 2배

                player.animator.SetFloat("AttackSpeed", 2f);

                break;

            case 2:

                player.AttackDelay = player.InitAttackDelay * 0.33f;  // 공격 속도 2배

                player.animator.SetFloat("AttackSpeed", 3f);

                break;

            case 3:

                player.AttackDelay = player.InitAttackDelay * 0.25f;  // 공격 속도 2배

                player.animator.SetFloat("AttackSpeed", 4f);

                break;
        }

        HUDcontroller.Status[4].SetActive(true);

        yield return new WaitForSeconds(time);

        if (endTime < EndTime) yield break;

        player.AttackDelay = player.InitAttackDelay;  // 공격 속도 원상태

        player.animator.SetFloat("AttackSpeed", 1f);

        HUDcontroller.Status[4].SetActive(false);
    }
}