using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    [HideInInspector] public int ChainNum;
    [HideInInspector] public int timer;
    [HideInInspector] public int Damage;

    public GameObject EffectPrefab1;
    public GameObject EffectPrefab2;

    private void Start()
    {
        Damage = ChainNum * 20;

        timer = ChainNum;

        GameObject effectPrefab1 = Instantiate(EffectPrefab1,
            new Vector3(transform.position.x, 1f, 0f), Quaternion.Euler(-90f, 0f, 0f));

        effectPrefab1.transform.parent = GameManager.instance.EffectHolder.transform;

        StartCoroutine(Explode());
    }

    private void OnTriggerStay2D(Collider2D other) // 방벽에 부딪힌 적들을 계속해서 밀쳐낸다.
    {                                               // 데미지 주는 코드에서 데미지 부분 빼고 넉백부분만 넣음
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy == null) return;

            if (enemy.Hp > 0
                && EnemySpawner.EnemyArray.Contains(enemy))
            {
                enemy.IsHit = true;

                enemy.TriggerUpdate(enemy, "Hit");

                enemy.StartCoroutine(enemy.KnockBack(enemy, 1.5f));
            }
        }
    }

    private IEnumerator Explode()       // 보호막과 가까운 거리에 있는 적 모두에게 피해를 준다.
    {

        yield return new WaitForSeconds(timer);

        for (int i = 0; i < EnemySpawner.EnemyArray.Count; i++)
        {
            Enemy enemy = EnemySpawner.EnemyArray[i];

            if (enemy.gameObject.transform.position.x - transform.position.x <= 3.0f
           && enemy.gameObject.transform.position.x - transform.position.x >= -3.0f)
            {
                enemy.Hp -= Damage;
                enemy.HpBarUpdate();

                if (enemy.Hp <= 0)
                {
                    enemy.KillTarget(enemy);
                }

                else
                {
                    enemy.IsHit = true;

                    enemy.TriggerUpdate(enemy, "Hit");

                    enemy.StartCoroutine(enemy.Hit(enemy, 1f));

                    // if (!enemy.IsKnock) enemy.StartCoroutine(enemy.KnockBack(enemy, 1f));
                }


            }

        }
        Destroy(gameObject); // 데미지 주고 파괴

        GameObject effectPrefab2 = Instantiate(EffectPrefab2, transform.position, Quaternion.Euler(-90f, 0f, 0f));

        effectPrefab2.transform.parent = GameManager.instance.EffectHolder.transform;

        yield break;
    }

}
