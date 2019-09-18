using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steroid : Skill {

    // 에이전트 스테로이드 : 공격력을 극대화하는 대신 그만큼 체력이 감소한다.
    // 1체인  에이전트의 현재 체력 12% 감소, 공격력 2배 증가
    // 2체인  에이전트의 현재 체력 24% 감소, 공격력 4배 증가
    // 3체인  에이전트의 현재 체력 36% 감소, 공격력 6배 증가

    private static float EndTime;
    private GameObject Devil;

    public override void Activation(int chainNum)
    {
        Devil = GameManager.instance.Devil;

        float time = 4.0f;

        if (time + Time.fixedTime - EndTime > Mathf.Epsilon)
        {
            EndTime = time + Time.fixedTime;

            Player player = GameManager.instance.playerList[0];
                if (player.Hp > 0)
                {
                    // 어차피 for문에 들어온 순간 EndTime이나 Poison 클래스의 time과 같은 맥락으로 사용 가능. 굳이 _time = time + Time.FixedTime을 할당시켜 _time을 쓸 필요는 없음
                    GameManager.instance.StartCoroutine(AttackUp(player, time, EndTime, chainNum));
                }
            
        }
    }

    IEnumerator AttackUp(Player player, float time, float endTime, int ChainNum)
    {
        Devil.SetActive(true);

        HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

        player.Hp = (int)(player.Hp * (0.95f - (0.05f * ChainNum)));

        player.HpBarUpdate();

        player.Damage = player.InitDamage * 3 * ChainNum;

        HUDcontroller.Status[2].SetActive(true);

        yield return new WaitForSeconds(time);

        if (endTime < EndTime) yield break;

        player.Damage = player.InitDamage;

        HUDcontroller.Status[2].SetActive(false);

        Devil.SetActive(false);
    }
}
