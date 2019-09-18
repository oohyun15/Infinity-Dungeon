using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [Space]

    [Header("Enemy Initialization")]
    public int Point; // score point
    public float KnockBackTime;
    public float KnockBackResistance;
    public int AttackMotionNum; // 애니메이션 숫자

    [Space]

    [Header("Enemy Ingame Information")]
    public int targetIndex;
    public bool IsKnock = false;

    [HideInInspector] public float Height; // 에너미 스프라이트 이미지의 사이즈가 제각각이라 높이를 따로 구해서 체력바를 구해야해서 쓰는 변수
    [HideInInspector] public float EndTime = 0; // 현재 ElectronicShield 구현 시 중복 방지를 위한 코드. 후에 다른 방식으로 수정이 필요함.
    [HideInInspector] public float EndTime2 = 0; // 애는 Ghost 구현에 쓸 코드
    [HideInInspector] public float EndTime3 = 0; // Poisoning

    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        target = null;

        // InitHp += GameManager.instance.StageLevel * 30;

        // InitDamage += GameManager.instance.StageLevel * 5;

        Hp = InitHp;

        AttackDelay = InitAttackDelay;

        Damage = InitDamage;

        Speed = InitSpeed;

        HpHUD.value = 1.0f;

        Height = GetComponent<Renderer>().bounds.size.y * 0.5f + 0.4f;

        targetIndex = GameManager.instance.playerTargetIndex;

        FindTarget();
    }
    protected override IEnumerator Move()
    {
        StopCoroutine(Attack());

        while (target.transform.localPosition.x - gameObject.transform.localPosition.x + AttackRange < Mathf.Epsilon)
        {
            if (GameManager.instance.IsActive())
            {
                if (target.Hp <= 0) FindTarget();

                gameObject.transform.Translate(Vector3.left * Time.deltaTime * Speed);

                // HpHUD.transform.position = Camera.main.WorldToScreenPoint(transform.localPosition + Vector3.up * transform.localScale.y * 0.4f);

                HpHUD.transform.position = Camera.main.WorldToScreenPoint(transform.localPosition + Vector3.up * Height);
            }
            yield return null;
        }
        yield return StartCoroutine(Attack());
    }
    public override void Hurt(int Damage, float KnockBackRange)
    {
        Hp -= Damage;

        HpBarUpdate();

        if (Hp <= 0
            && EnemySpawner.EnemyArray.Contains(this)) // 3명이 같은 타겟일 경우 중복 호출을 방지하기 위한 코드
            KillTarget(this);

        //    else if (target.Hp <= 0) break;

        else if (!IsHit)
        {
            IsHit = true;

            TriggerUpdate(this, "Hit");

            if (!IsKnock)
                StartCoroutine(KnockBack(this, KnockBackRange));
        }
    }
    protected override IEnumerator Attack()
    {
        StopCoroutine(Move());

        while (target.Hp > 0)
        {
            if (!GameManager.instance.IsActive()) yield return null;

            else if (target.transform.position.x - gameObject.transform.position.x + AttackRange < Mathf.Epsilon) yield return StartCoroutine(Move());

            else
            {
                int index = Random.Range(0, AttackMotionNum);

                TriggerUpdate(this, "Attack");

                IntUpdate(this, "AttackNum", index);

                yield return new WaitForSeconds(AttackDelay);

                if (!target.IsHit
                    && Hp > 0)
                {
                    if (target.Hp <= 0) break;

                    target.Hurt(Damage, 0);
                }
            }
        }
        FindTarget();
    }

    protected override void FindTarget()
    {
        // Move에서 오던 Attack에서 오던 기존에 실행되던 코루틴 모두 정지
        StopCoroutine(Move());
        StopCoroutine(Attack());
        /* if (GameManager.instance.SkillObList[0] != null)
         {   
             target = GameManager.instance.SkillObList[0];
            // StartCoroutine(Move());
         }

         else if (GameManager.instance.SkillObList[1] != null)
         {
             target = GameManager.instance.SkillObList[1];
             StartCoroutine(Move());
         }
         */

        if (targetIndex < GameManager.instance.playerList.Length)
        {
            target = GameManager.instance.playerList[targetIndex++];

            StartCoroutine(Move());
        }
        else GameManager.instance.gameState = GameManager.GameState.GameOver;
    }
}
