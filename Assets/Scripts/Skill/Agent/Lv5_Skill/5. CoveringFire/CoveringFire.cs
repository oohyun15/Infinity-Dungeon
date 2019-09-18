using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에이전트 지원사격 : 각 체인마다 다른 공격으로 적들을 공격한다.
// 1체인 : 현재 에이전트의 타겟(타겟이 없다면 가장 가까운 적)에게 큰 데미지를 준다.
//  데미지 : 120
// 2체인 : 현재 에이전트의 타겟(타겟이 없다면 가장 가까운 적)과 그 주변 적에게 적은 데미지를 준다.
//  데미지 : 40  범위 : 2.5
// 3체인 : 현재 에이전트의 타겟(타겟이 없다면 가장 가까운 적)과 그 주변 적에게 큰 데미지를 준다.
//  데미지 : 80  범위 : 3.75

public class CoveringFire : Skill
{
    public int Damage; // 40 줄거임
    public float KnockBackRange;
    public GameObject Sniping;
    public RangeSniping rangeSniping;
    public RangeSniping rangeSniping2;

    public override void Activation(int chainNum)
    {
        Player player = GameManager.instance.playerList[0];

        Enemy target = (Enemy)(player.target);

        if (target == null)
        {
            float Dist = Mathf.Infinity;

            for (int i = 0; i < EnemySpawner.EnemyArray.Count; i++)
            {
                if (EnemySpawner.EnemyArray[i].transform.position.x - player.transform.position.x < Dist)
                {
                    Dist = EnemySpawner.EnemyArray[i].transform.position.x - player.transform.position.x;

                    target = EnemySpawner.EnemyArray[i];
                }
            }
        }
        GameManager.instance.StartCoroutine(Fire(chainNum, target));
    }

    private IEnumerator Fire(int chainNum, Enemy target)
    {
        if (chainNum == 1)
        {
            GameObject _Sniping = Instantiate(Sniping, target.transform.position, Quaternion.identity);

            _Sniping.transform.parent = GameManager.instance.EffectHolder.transform;

            yield return new WaitForSeconds(0.3f);

            // 120 데미지
            target.Hurt(Damage * 3, KnockBackRange);

            Destroy(_Sniping);

            yield break;
        }

        else if (chainNum == 2)
        {
            RangeSniping _RangeSniping = Instantiate(rangeSniping, new Vector3(target.transform.position.x, 1.0f, 0.0f), Quaternion.identity);

            _RangeSniping.transform.parent = GameManager.instance.EffectHolder.transform;

            int SDamage = Damage;

            _RangeSniping.Damage = SDamage;

            _RangeSniping.WaitTime = 0.3f;

            yield return null;
        }

        else if (chainNum == 3)
        {
            RangeSniping _RangeSniping2 = Instantiate(rangeSniping2, new Vector3(target.transform.position.x, 1.0f, 0.0f), Quaternion.identity);

            _RangeSniping2.transform.parent = GameManager.instance.EffectHolder.transform;

            int SDamage = Damage * 2;

            _RangeSniping2.Damage = SDamage;

            _RangeSniping2.WaitTime = 0.3f;

            yield return null;
        }
    }
}
