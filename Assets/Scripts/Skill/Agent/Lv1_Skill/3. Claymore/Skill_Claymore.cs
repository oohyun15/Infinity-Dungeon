using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 에이전트 크레모어 : 데미지를 주고 더 멀리 넉백
// 1체인 데미지 40 
// 2체인 데미지 60
// 3체인 데미지 80

public class Skill_Claymore : Skill {

    [HideInInspector]public int Damage;
    public float Speed;
    public float KnockBackRange;

    public GameObject _claymore;
    public ClaymoreBullet _claymorebullet;

    public override void Activation(int chainNum)
    {
        GameObject claymore = Instantiate(_claymore,
            new Vector3(GameManager.instance.playerList[0].transform.position.x + 1.5f, 1.3f, 0.0f), Quaternion.identity);


        claymore.transform.parent = GameManager.instance.EffectHolder.transform;

        ClaymoreBullet claymoreBullet = Instantiate(_claymorebullet,
            claymore.transform.position + Vector3.right * 0.2f, Quaternion.identity);

        switch (chainNum)
        {
            case 1:
                Damage = 10;
                break;
            case 2:
                Damage = 30;
                break;
            case 3:
                Damage = 50;
                break;
        }

        claymoreBullet.Damage = Damage;
        claymoreBullet.Speed = Speed;
        claymoreBullet.ChainNum = chainNum;
        claymoreBullet.KnockBackRange = KnockBackRange;
        claymoreBullet.transform.parent = GameManager.instance.EffectHolder.transform;

        Destroy(claymore, 0.5f);
        //StartCoroutine(Shooting(claymore));
    }

    /*private IEnumerator Shooting(GameObject claymore)
    {
        while(GameManager.instance.playerList[0].target == null)
        {
            yield return null;
        }

        ClaymoreBullet claymoreBullet = Instantiate(_claymorebullet,
            claymore.transform.position + Vector3.right * 0.2f, Quaternion.identity);

        claymoreBullet.damage = damage;
        claymoreBullet.speed = speed;
        claymoreBullet.ChainNum = ChainNum;
    }*/
}
