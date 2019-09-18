using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreBullet : MonoBehaviour
{

    [HideInInspector] public Vector3 StartPoint;
    [HideInInspector] public Vector3 EndPoint;
    [HideInInspector] public int Damage;
    [HideInInspector] public float Speed;
    [HideInInspector] public int ChainNum;
    [HideInInspector] public float endPoint;
    [HideInInspector] public float KnockBackRange;
    public GameObject EffectPrefab;

    private void Start()
    {
        endPoint = GameManager.instance.playerList[0].transform.position.x + 3.0f;
        //총알이 나가는 최대 거리

        Damage += ((ChainNum - 1) * 20);

        // Debug.Log("Damage: " + damage);

        StartCoroutine(Moving());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // endpoint까지 가는 동안 부딪힌 모든 적을 밀치고 데미지를 준다.
        // 부딪혀도 파괴 X
        if (collision.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

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

                    if (!enemy.IsKnock) enemy.StartCoroutine(enemy.KnockBack(enemy, KnockBackRange));
                }
            }
        }
    }
    public IEnumerator Moving()
    {
        while (transform.position.x < endPoint)
        {
            transform.Translate(new Vector3(endPoint, transform.position.y, 0.0f) * Speed * Time.deltaTime);

            yield return null;
        }
        Destroy(gameObject);

        GameObject effectPrefab = Instantiate(EffectPrefab, transform.position, Quaternion.identity);

        effectPrefab.transform.parent = GameManager.instance.EffectHolder.transform;

        yield break;
    }
}
