using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{



    public float timer = 4.0f;                       // 스폰 타이머
    public int BossFrequency = 10;


    public Enemy[] enemyList;
    public Enemy[] bossList;
    public GameObject Enemies;
    public static List<Enemy> EnemyArray;
    public static int EnemyCount;



    [Space]

    [Header("Enemy HUD")]
    public Slider HpHUD;
    public GameObject HUD_Holder;




    private readonly float InitEnemyPosition = 17.0f;
    private float EnemyPointY;
    private float EnemyScaleY;



    public void InitEnemyArray()
    {
        EnemyArray = new List<Enemy>();

        EnemyCount = 0;
    }

    public IEnumerator SpawnEnemy()
    {
        while (GameManager.instance.gameState == GameManager.GameState.GamePlay)
        {
            if (!GameManager.instance.GameWait
                && EnemyArray.Count < 10
                && EnemyCount < GameManager.instance.StageClearEnemyCount)
            {
                EnemySpawn();

                yield return new WaitForSeconds(Random.Range(timer - 2.0f, timer)); // timer = 4.0f
            }
            else yield return null;
        }
        yield break;
    }

    // 적 인스턴스화
    private void EnemySpawn()                         
    {
        Enemy enemy = Instantiate(
            EnemyKind(),
            EnemyPosition(),
            Quaternion.identity);

        Slider slider = Instantiate(
            HpHUD,
            HUDPosition(),
            Quaternion.identity);

        enemy.transform.parent = Enemies.transform;

        slider.transform.SetParent(HUD_Holder.transform);

        EnemyArray.Add(enemy);

        enemy.HpHUD = slider;

        EnemyCount++;
    }

    Enemy EnemyKind()
    {
        Enemy enemy = null;

        int index;

        if (EnemyCount % BossFrequency == 0
            && EnemyCount != 0)
        {
            index = Random.Range(0, bossList.Length);

            enemy = bossList[index];
        }
        else
        {
            index = Random.Range(0, enemyList.Length);

            enemy = enemyList[index];
        }

        EnemyPointY = enemy.transform.position.y;

        EnemyScaleY = enemy.transform.localScale.y;

        return enemy;
    }

    Vector3 EnemyPosition()
    {
        return new Vector3(InitEnemyPosition, EnemyPointY, 0);
    }

    Vector3 HUDPosition()
    {
        return Camera.main.WorldToScreenPoint(EnemyPosition() + Vector3.up * EnemyScaleY * 0.4f);
    }

}
