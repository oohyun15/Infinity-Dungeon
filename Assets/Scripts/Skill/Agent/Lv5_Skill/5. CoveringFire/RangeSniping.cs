using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSniping : MonoBehaviour
{

    [HideInInspector] public int Damage;
    [HideInInspector] public float WaitTime;
    private float GenTime;

    private void Start()
    {
        GenTime = Time.fixedTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // 씨발 진짜 이런 좆같은 코드 왜쓰는거지? 존나 답답하네. 코루틴 못쓰는거 자랑하는건가
        if (Time.fixedTime < GenTime + WaitTime) return;  

        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            if (enemy == null) return;

            if (enemy.Hp > 0
                && EnemySpawner.EnemyArray.Contains(enemy))
            {
                enemy.Hp -= Damage;

                enemy.HpBarUpdate();

                if (enemy.Hp <= 0) enemy.KillTarget(enemy);

                else
                {
                    enemy.IsHit = true;

                    enemy.TriggerUpdate(enemy, "Hit");

                    if (!enemy.IsKnock) enemy.StartCoroutine(enemy.KnockBack(enemy, 1f));
                }
            }
        }
        Destroy(gameObject);
    }
}
