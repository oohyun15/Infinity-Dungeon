using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{

    [HideInInspector] public int ChainNum;
    [HideInInspector] public float Speed;
    [HideInInspector] public int Damage;
    [HideInInspector] public float timer;
    public GameObject EffectPrefab;

    private void Start()
    {
        switch (ChainNum)
        {
            case 1:
                timer = 1.0f;
                break;
            case 2:
                timer = 2.0f;
                break;
            case 3:
                timer = 3.0f;
                break;
        }
        StartCoroutine(Move());
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

                    if (enemy.EndTime2 < time)
                    {
                        enemy.EndTime2 = time;

                        enemy.StartCoroutine(Binding(enemy, time));
                    }
                }
            }
        }
    }

    IEnumerator Binding(Enemy enemy, float time)
    {
        HUDController HUDcontroller = enemy.HpHUD.GetComponent<HUDController>();

        enemy.Speed = 0.0f;       // 대상의 스피드 0 으로 만들어 움직임 멈춤

        HUDcontroller.Status[10].SetActive(true);

        GameObject BindingEffect = Instantiate(EffectPrefab, enemy.transform.position, Quaternion.identity);

        BindingEffect.transform.parent = enemy.transform;

        yield return new WaitForSeconds(timer);

        Destroy(BindingEffect);

        if (time < enemy.EndTime2) yield break;    // 문제점 EndTime이 계속 증가해서 break가 걸려버린다.

        enemy.Speed = enemy.InitSpeed;      // 시간이 끝나면 저장된 속도로 복구

        HUDcontroller.Status[10].SetActive(false);        
    }


    private IEnumerator Move()
    {
        while (transform.position.x < 20.0f)
        {
            transform.Translate(Vector3.right * Speed * Time.deltaTime);

            yield return null;
        }
        Destroy(gameObject);
    }
}
