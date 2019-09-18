using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinding : MonoBehaviour {

    [HideInInspector] public int Damage;
    [HideInInspector] public int num;
    [HideInInspector] public int LoseRate;

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

                enemy.StartCoroutine(PoisoningAndBlinding(enemy, num, time));
            }
        }   
    }

    IEnumerator PoisoningAndBlinding(Enemy target, int num, float time)
    {
        int count = 0;

        if (target == null) yield break;

        HUDController HUDcontroller = target.HpHUD.GetComponent<HUDController>();

        int RandomNum = Random.Range(0, LoseRate);

        if (RandomNum != 0)
        {
            target.Damage = 0;

            HUDcontroller.Status[11].SetActive(true);
        }
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

        target.Damage = target.InitDamage;

        HUDcontroller.Status[1].SetActive(false);

        HUDcontroller.Status[11].SetActive(false);
    }
}
