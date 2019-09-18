using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectronicShield : MonoBehaviour
{

    public int ChainNum;
    public int Damage;
    public float timer;
    public float DownSpeed;

    private void Start()
    {
        switch (ChainNum)
        {
            case 1:
                Damage = 10;
                timer = 1.5f;
                break;
            case 2:
                Damage = 20;
                timer = 2.5f;
                break;
            case 3:
                Damage = 40;
                timer = 4.0f;
                break;
        }

        // Debug.Log((transform.position.x - 3.0f) + " ~ " + (transform.position.x + 3.0f));

        StartCoroutine(Move());

        Destroy(gameObject, timer);
    }


    private void OnTriggerEnter2D(Collider2D collision) // 부딪힌 적에게 데미지 주기
    {
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

                    enemy.StartCoroutine(enemy.Hit(enemy, timer));

                    float time = timer + Time.fixedTime;

                    if (enemy.EndTime < time)
                    {
                        enemy.EndTime = time;

                        enemy.StartCoroutine(SlowTimer(enemy, time)); // 데미지 주고 느리게 하는 코루틴 실행
                    }
                }
            }
        }
    }

    private IEnumerator Move()
    {
        while (transform.position.x < 20.0f)
        {
            transform.Translate(Vector3.right * 3.0f * Time.deltaTime);

            yield return null;
        }
    }


    private IEnumerator SlowTimer(Enemy enemy, float time) // 느리게 하기
    {
        HUDController HUDcontroller = enemy.HpHUD.GetComponent<HUDController>();

        enemy.Speed = enemy.InitSpeed * (1 - DownSpeed * ChainNum); // 체인에 따라 느려짐

        HUDcontroller.Status[9].SetActive(true);

        yield return new WaitForSeconds(timer); // 지속시간 후에

        if (time < enemy.EndTime) yield break;

        enemy.Speed = enemy.InitSpeed;                     // 원상복귀

        HUDcontroller.Status[9].SetActive(false);
    }
}
