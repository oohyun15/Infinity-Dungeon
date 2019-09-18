using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 사제 스킬 기도 : 아군을 치료함과 동시에 범위 내 적들에게 피해를 준다.
// 1체인 : 초당 데미지 5, 초당 체력 회복 10, 지속시간  3초 (총 30 피해 60 회복)
// 2체인 : 초당 데미지 10, 초당 체력 회복 15, 지속시간  3초 (총 60 피해 90 회복)
// 3체인 : 초당 데미지 15, 초당 체력 회복 20, 지속시간  3초 (총 90 피해 120 회복)
// 초당 아님. 0.5초당임

public class Pray : Skill
{

    public int heal;
    public int damage;
    public static float EndTime;
    public float Range;
    [HideInInspector] public float positionX;
    public GameObject EffectPrefab;





    public override void Activation(int chainNum)
    {
        positionX = GameManager.instance.playerList[2].transform.position.x;

        int num = 6;

        float time = num * 0.5f + Time.fixedTime;

        damage = damage + (chainNum - 1) * 5;

        heal = heal + (chainNum - 1) * 5;

        for (int i = 0; i < EnemySpawner.EnemyArray.Count; i++)
        {
            Enemy enemy = EnemySpawner.EnemyArray[i];
            if (enemy.gameObject.transform.position.x - positionX < Range)
            {
                if (EndTime < time) EndTime = time;

                enemy.StartCoroutine(Poisoning(enemy, num, time));
                // 범위 내 적에게 3초간 데미지를 준다.
            }
        }
        for (int i = 0; i < GameManager.instance.playerList.Length; i++)
        {
            Player player = GameManager.instance.playerList[i];

            // 이펙트가 캐릭터 밑부터 시작되도록 포지션 값을 수동으로 설정해 놓음. 후에 플레이어 크기가 바뀔 경우 수정이 필요함.
            GameObject effectPrefab = Instantiate(EffectPrefab, player.transform.position +Vector3.down, Quaternion.Euler(-90f,0f,0f));

            effectPrefab.transform.parent = GameManager.instance.EffectHolder.transform;

            // 실제로 힐은 체력이 있을 때만 올라감, 대신 이펙트는 켜짐
            if (player.Hp > 0) player.StartCoroutine(Healing(player, num, time));
        }
    }

    IEnumerator Poisoning(Enemy target, int num, float time) // Poison과 같은 코드
    {
        int count = 0;

        if (target == null) yield break;

        //  HUDcontroller.ActiveUpdate(1, true);

        HUDController HUDcontroller = target.HpHUD.GetComponent<HUDController>();

        HUDcontroller.Status[1].SetActive(true);

        while (target.Hp > 0
            && EnemySpawner.EnemyArray.Contains(target)
            && count < num)
        {
            count++;

            target.Hp -= damage;

            target.HpBarUpdate();

            if (target.Hp <= 0)
            {
                target.KillTarget(target);

                yield break;
            }
            else
            {
                target.IsHit = true;

                target.TriggerUpdate(target, "Hit");

                target.StartCoroutine(target.Hit(target, 1f));
            }
            yield return new WaitForSeconds(0.5f);
        }
        if (time < EndTime) yield break;

        HUDcontroller.Status[1].SetActive(false);
    }

    IEnumerator Healing(Player player, int num, float time) // Poison 데미지 주는 부분만 힐로 바꿈
    {
        int count = 0;

        HUDController HUDcontroller = player.HpHUD.GetComponent<HUDController>();

        HUDcontroller.Status[0].SetActive(true);

        while (player.Hp > 0 && count < num)
        {
            count++;

            player.Hp += heal;

            if (player.Hp > player.InitHp) player.Hp = player.InitHp;
            
            player.HpBarUpdate();

            yield return new WaitForSeconds(0.5f);
        }
        if (time < EndTime) yield break;

        HUDcontroller.Status[0].SetActive(false);
    }
}
