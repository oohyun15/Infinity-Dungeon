using System.Collections;
using UnityEngine;

public class Poison : MonoBehaviour
{

    public int Damage;
    [HideInInspector] public int ChainNum;
    [HideInInspector] public bool IsGround = false;
    public Addict Broken;


    private new Rigidbody2D rigidbody;


    private void Start()
    {
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

            int num = (int)Mathf.Pow(2.0f, ChainNum + 1);

            Addict broken = Instantiate(Broken,
                new Vector3(positionX, 1.13f, 0f),
                Quaternion.identity);

            broken.transform.parent = GameManager.instance.EffectHolder.transform;

            broken.Damage = Damage;

            broken.num = num;

            // 연기 사라지는거 수동으로 시간 맞춰놓음
            Destroy(broken.gameObject, 4f);

            Destroy(gameObject);
        }
    }
}
