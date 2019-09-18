using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 사제 무적 : 지속시간 동안 무적상태로 만들어 피해를 받지 않는다.
// 1체인 : 1.5초 무적
// 2체인 : 3초 무적
// 3체인 : 4.5초 무적
public class Invincibility : Skill
{

    private float timer = 1.5f;
    public static float EndTime;
    private GameObject Angel;

    public override void Activation(int chainNum)
    {
        Angel = GameManager.instance.Angel;

        timer = timer * chainNum;

        float time = timer + Time.fixedTime;

        if (time - EndTime > Mathf.Epsilon)
        {
            EndTime = time;

            GameManager.instance.StartCoroutine(NoDamage(time));
        }
    }

    private IEnumerator NoDamage(float time)
    {
        Angel.SetActive(true);

        for (int i = 0; i < GameManager.instance.playerList.Length; i++)
        {
            Player player = GameManager.instance.playerList[i];

            //   HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

            player.DefensiveRatio = 0.0f;

            // 무적 HUD 미구현
            // HUDcontroller.Status[2].SetActive(true);
        }

        yield return new WaitForSeconds(timer);

        if (time < EndTime) yield break;

        for (int i = 0; i < GameManager.instance.playerList.Length; i++)
        {
            Player player = GameManager.instance.playerList[i];

            //   HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

            player.DefensiveRatio = player.InitDefensiveRatio;

            // HUDcontroller.Status[2].SetActive(false);
        }
        Angel.SetActive(false);
    }
}
