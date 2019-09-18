using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Addict : MonoBehaviour {

    [HideInInspector] public int Damage;
    [HideInInspector] public int num;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy" )
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy == null) return;

            if (enemy.Hp > 0
                && EnemySpawner.EnemyArray.Contains(enemy))
            {
                float time = num * 0.5f + Time.fixedTime;

                if (enemy.EndTime3 < time) enemy.EndTime3 = time;

                enemy.StartCoroutine(Poisoning(enemy, num, time));
            }
        }   
    }

    IEnumerator Poisoning(Enemy target, int num, float time)
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
            target.Hp -= Damage;
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
        if (time < target.EndTime3) yield break;

        HUDcontroller.Status[1].SetActive(false);
    }
}
