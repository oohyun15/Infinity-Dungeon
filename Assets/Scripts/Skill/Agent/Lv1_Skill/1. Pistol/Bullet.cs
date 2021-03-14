using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public int Damage;
    [HideInInspector] public float speed;
    [HideInInspector] public int ChainNum;


    // Use this for initialization
    void Start()
    {

        StartCoroutine(Move());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // null exception 있음. 확인 바람 0.42b
            if (enemy == null)
                return;

            if (enemy.Hp > 0
                && EnemySpawner.EnemyArray.Contains(enemy))
            //   if (enemy != null)
            {
                enemy.Hp -= Damage;

                enemy.HpBarUpdate();

                if (enemy.Hp <= 0) enemy.KillTarget(enemy);

                // 안전장치
                //      && EnemySpawner.EnemyArray.Contains(enemy)) enemy.KillTarget(enemy);

                //  else if (enemy.Hp <= 0) return;

                else
                {
                    enemy.IsHit = true;

                    enemy.TriggerUpdate(enemy, "Hit");

                    if (!enemy.IsKnock) enemy.StartCoroutine(enemy.KnockBack(enemy, 1f));
                }
                StopCoroutine(Move());
                Destroy(gameObject); // 이거 풀면 사라짐
            }

        }
    }


    IEnumerator Move()
    {
        while (transform.position.x < 20.0f)
        {
            /*
            if (GameManager.instance.gameState != GameManager.GameState.GamePlay)
            {
                StopAllCoroutines(); // 안전 장치, 게임 오버 및 게임 클리어 시 플레이어가 하고 있는 코루틴을 모두 중지 시켜 널타입 익셉션에서 벗어나게 하자
                Destroy(gameObject);
            }
            */

            transform.Translate(Vector3.right * speed * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }


}
