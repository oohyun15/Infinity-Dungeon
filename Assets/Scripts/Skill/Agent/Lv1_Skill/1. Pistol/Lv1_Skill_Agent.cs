using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 요원 스킬: 권총 발사
/// 1체인: 1발 X 40 피해
/// 2체인: 2발 X 40 피해
/// 3체인: 4발 X 40 피해
/// </summary>

public class Lv1_Skill_Agent : Skill
{

    public int damage;
    public float speed;
    public Bullet _Bullet;

    public override void Activation(int chainNum)
    {
        int num = (int)Mathf.Pow(2.0f, chainNum - 1);
        GameManager.instance.StartCoroutine(Shot(num, chainNum));
    }

    IEnumerator Shot(int num, int chainNum)
    {
        int count = 0;
        while (count < num)
        {
            count++;
            Bullet bullet = Instantiate(_Bullet,
                                        GameManager.instance.playerList[0].transform.position,
                                        Quaternion.identity);

            bullet.Damage = damage;
            bullet.ChainNum = chainNum;
            bullet.speed = speed;
            bullet.transform.parent = GameManager.instance.EffectHolder.transform;
            
            SoundManager.instance.GunFire.Play();

            yield return new WaitForSeconds(0.1f);
        }
    }



}
