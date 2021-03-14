using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Poison 그대로 배낌 거의 데미지 줄 때 실명거는 것만 다름
public class AcidSmoke : MonoBehaviour {

	public int Damage;
    [HideInInspector] public int LoseRate;
    [HideInInspector] public int ChainNum;
    [HideInInspector] public bool IsGround = false;
    public Blinding Broken;
    private new Rigidbody2D rigidbody;

    private void Start()
    {
        LoseRate = ChainNum;
        float temp = Random.Range(Mathf.Tan(30f * Mathf.Deg2Rad), Mathf.Tan(60f * Mathf.Deg2Rad));
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.AddForce(new Vector2(1f, temp).normalized * 400f);
        rigidbody.AddTorque(10f); // 회전축
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsGround) return;

        else
        {
            IsGround = true;

            float positionX = gameObject.transform.position.x;

            int num = 3 * (int)Mathf.Pow(2.0f, ChainNum - 1);

            Blinding broken = Instantiate(Broken,
                                          new Vector3(positionX, 1.13f, 0f),
                                          Quaternion.identity);

            broken.transform.parent = GameManager.instance.EffectHolder.transform;

            broken.Damage = Damage;

            broken.num = num;

            broken.LoseRate = LoseRate;

            SoundManager.instance.GlassBroken.Play();

            // 연기 사라지는거 수동으로 시간 맞춰놓음
            Destroy(broken.gameObject, 4.4f);

            Destroy(gameObject);
        }
    }
}
