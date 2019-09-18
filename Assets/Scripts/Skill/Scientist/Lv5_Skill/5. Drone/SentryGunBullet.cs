using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGunBullet : MonoBehaviour
{

    [HideInInspector] public int Damage;
    [HideInInspector] public float speed;


    // Use this for initialization
    void Start()
    {
        StartCoroutine(Move());
    }

    private void OnTriggerEnter2D(Collider2D other) // 에이전트 총알과 똑같음
    {

        if (other.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            // null exception 있음. 확인 바람 0.42b
            if (enemy == null)
                return;

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

                    enemy.StartCoroutine(enemy.Hit(enemy, 1f));

                   // if (!enemy.IsKnock) enemy.StartCoroutine(enemy.KnockBack(enemy, 1f));
                }
                Destroy(gameObject); 
            }
        }
    }


    private IEnumerator Move()
    {
        Vector3 _position = new Vector3(Mathf.Tan(-30f), 1, 0);

        while (transform.position.x < 10f) 
            //transform.position.y < 0f)
        {
            transform.Translate(_position.normalized * speed * Time.deltaTime);

            yield return null;
        }
        Destroy(gameObject);
    }


}
