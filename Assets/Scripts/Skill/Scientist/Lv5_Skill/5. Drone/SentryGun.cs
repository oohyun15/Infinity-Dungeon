using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGun : MonoBehaviour
{
    public static SentryGun instance = null;
    [HideInInspector] public int timer;
    [HideInInspector] public int Damage;
    [HideInInspector] public int ChainNum;
    [HideInInspector] public float ShootSpeed;
    public SentryGunBullet STbullet;
    [HideInInspector] public float speed;
    public GameObject EffectPrefab;
    private ParticleSystem effect;

    private static int angleCount = 0;

    // Use this for initialization

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            effect = instance.EffectPrefab.GetComponent<ParticleSystem>();
        }

        else if (instance != this)
        {
            Destroy(instance.gameObject);

            instance = this;

            effect = instance.EffectPrefab.GetComponent<ParticleSystem>();
        }
    }
    void Start()
    {
        // Damage += (ChainNum - 1) * 10;
        switch (ChainNum)
        {
            case 1:
                ShootSpeed = 0.3f;
                break;
            case 2:
                ShootSpeed = 0.2f;
                break;
            case 3:
                ShootSpeed = 0.1f;
                break;
        }
        StartCoroutine(Shot());

        StartCoroutine(Timeout());
    }

    private IEnumerator Shot()
    {
        Vector3 _position = transform.position + 0.23f * Vector3.down;

        while (true) // 오브젝트가 존재하는 한 계속 while문 실행
        {
            float _angle = -(10 + (Mathf.Abs(angleCount-5) * 4)); // -30 ~ -10 왔다갔다 함

            SentryGunBullet _STbullet = Instantiate(STbullet,
                                                    _position, Quaternion.Euler(0, 0, _angle));

            if (++angleCount % 11 == 0) angleCount = 0;

            _STbullet.transform.parent = GameManager.instance.EffectHolder.transform;

            _STbullet.speed = speed;

            _STbullet.Damage = Damage;

            if (!EffectPrefab.activeSelf) EffectPrefab.SetActive(true);

            effect.Play();

            SoundManager.instance.GunFire.Play();

            yield return new WaitForSeconds(ShootSpeed); // 총알 생성 후 0.3초 대기
        }
    }

    private IEnumerator Timeout() // 지속 시간이 지나면 shot을 멈추고 오브젝트 파괴
    {
        yield return new WaitForSeconds(3.0f);

        Destroy(gameObject);
    }
}

