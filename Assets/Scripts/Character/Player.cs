using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{
    // public variable
 

    [Space]

    [Header("Player Initialization")]
    public float KnockBackRange;

    [HideInInspector] public Vector3 InitPlayerPosition;

    // private variable
    private Vector3 playerPosition;
    private Vector3 playerRotation;
    private int countTime = 0;



    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();

        animator = GetComponent<Animator>();

        InitPlayerPosition = gameObject.transform.localPosition;

        Speed = InitSpeed;

        DefensiveRatio = InitDefensiveRatio;
    }

    public void PlayerMove(float position)
    {
        StopAllCoroutines();

        BoolUpdate(this, "PlayerAlive", true);

        playerPosition = InitPlayerPosition + position * Vector3.right;

        target = null;

        StartCoroutine(Move());
    }


    protected override IEnumerator Move()
    {
        while (playerPosition.x - gameObject.transform.localPosition.x > Mathf.Epsilon)
        {
            transform.Translate(Vector3.right * Time.deltaTime * Speed);

            yield return null;
        }
        yield return StartCoroutine(Seeking());

    }

    IEnumerator Seeking()
    {
        while (target == null)
        {
            if (GameManager.instance.gameState != GameManager.GameState.GamePlay) StopAllCoroutines(); // 안전 장치, 게임 오버 및 게임 클리어 시 플레이어가 하고 있는 코루틴을 모두 중지 시켜 널타입 익셉션에서 벗어나게 하자

            if (GameManager.instance.IsActive())
            {
                for (int i = 0; i < EnemySpawner.EnemyArray.Count; i++)
                {
                    if (gameObject.transform.position.x - EnemySpawner.EnemyArray[i].transform.position.x + AttackRange > Mathf.Epsilon)
                    {
                        if (target == null) target = EnemySpawner.EnemyArray[i];

                        else if (EnemySpawner.EnemyArray[i].transform.position.x - target.transform.position.x < Mathf.Epsilon) target = EnemySpawner.EnemyArray[i];
                    }
                }
            }
            yield return null;
        }
        yield return StartCoroutine(Attack());
    }

    protected override IEnumerator Attack()
    {
        StopCoroutine(Seeking());

        while (target.Hp > 0
            && gameObject.transform.position.x - target.transform.position.x + AttackRange > Mathf.Epsilon)
        {
            if (GameManager.instance.gameState != GameManager.GameState.GamePlay) StopAllCoroutines(); // 안전 장치, 게임 오버 및 게임 클리어 시 플레이어가 하고 있는 코루틴을 모두 중지 시켜 널타입 익셉션에서 벗어나게 하자

            if (!GameManager.instance.IsActive()) yield return null;

            else
            {
                // Player Attack
                TriggerUpdate(this, "PlayerAttack");

                yield return new WaitForSeconds(AttackDelay);

                // Enemy Defend
                if (target.HaveDefend
                    && Random.Range(0, 3) == 0) TriggerUpdate(target, "Defend");

                else if (target.Hp <= 0) break;

                else
                {
                    target.Hurt(Damage, KnockBackRange);
                }
            }
        }
        target = null;

        FindTarget();

        yield return null;
    }
    public override void Hurt(int damage, float KnockBackRange)
    {
        
        Hp -= (int)(Damage * DefensiveRatio);

        // Debug.Log("Damage =" + FinalDamage + "DR =" + DefensiveRatio);

        HpBarUpdate();

        if (Hp <= 0) KillTarget(this);

        IsHit = true;

        // StartCoroutine(Hit(target, 0.7f));

        PlayerHit(0.7f);
    }

    protected override void FindTarget()
    {
        if (GameManager.instance.gameState != GameManager.GameState.GamePlay) StopAllCoroutines(); // 안전 장치, 게임 오버 및 게임 클리어 시 플레이어가 하고 있는 코루틴을 모두 중지 시켜 널타입 익셉션에서 벗어나게 하자

        StopCoroutine(Move());
        StopCoroutine(Attack());

        StartCoroutine(Seeking());
    }

    public void PlayerHit(float timer)
    {
        InvokeRepeating("Flash", 0f, timer * 0.1f);
    }

    public void Flash()
    {
        if (countTime < 10)
        {
            if (countTime % 2 == 0) spr.color = new Color32(255, 255, 255, 90);

            else spr.color = new Color32(255, 255, 255, 180);

            countTime++;
        }
        else if (Hp > 0)
        {
            spr.color = Color.white;

            IsHit = !IsHit;

            countTime = 0;

            CancelInvoke("Flash");
        }
        else
        {
            spr.color = new Color(1f, 1f, 1f, 0f);

            countTime = 0;

            CancelInvoke("Flash");
        }
    }
}
