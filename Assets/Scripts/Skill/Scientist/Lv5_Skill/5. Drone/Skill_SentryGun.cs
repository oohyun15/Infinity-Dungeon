using UnityEngine;


// 1체인 3초간 0.3초마다 발사 데미지 10
// 2체인 3초간 0.2초마다 발사 데미지 10
// 3체인 3초간 0.1초마다 발사 데미지 10

public class Skill_SentryGun : Skill {

    public SentryGun _sentrygun;
    public int Damage;
    public float speed;

    public override void Activation(int chainNum)
    {
        Vector3 _position = GameManager.instance.playerList[1].transform.position;


        SentryGun sentryGun = Instantiate(_sentrygun, 
            new Vector3(_position.x + 0.3f, _position.y + 2.0f, _position.z) , Quaternion.identity);

        sentryGun.transform.parent = GameManager.instance.EffectHolder.transform;

        sentryGun.speed = speed;

        sentryGun.Damage = Damage;

        sentryGun.ChainNum = chainNum;
    }


}
