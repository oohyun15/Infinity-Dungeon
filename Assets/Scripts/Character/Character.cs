using System.Collections;
using UnityEngine;
using UnityEngine.UI;



/*******************************************
*                                          * 
*             18.07.26 수정                *
*           Class Character                * 
*    Player, Enemy의 공통 특징을 나타냄    *
*                                          *
*******************************************/


public abstract class Character : MonoBehaviour
{
    // public variable
    [Header("UI")]
    public Slider HpHUD;

    [Space]

    [Header("Initialization")]
    public int InitHp;
    public int InitDamage;
    public float InitSpeed;
    public float InitAttackDelay;
    public float InitDefensiveRatio;
    public float AttackRange;
    public bool HaveDefend;

    [Space]

    [Header("Ingame Information")]
    public int Hp;
    public int Damage;
    public float Speed;
    public float DefensiveRatio;
    public float AttackDelay;
    public Character target;
    public bool IsHit;

   [HideInInspector] public Animator animator;
   [HideInInspector] public SpriteRenderer spr;

    // private variable
    private float SmoothCheckTime;
    private float KnockBackVelocity;




    public void HpBarUpdate()
    {
        HpHUD.value = (float)Hp / InitHp;
    }

    public void TriggerUpdate(Character Char, string name)
    {
        Char.animator.SetTrigger(name);
    }

    public void BoolUpdate(Character Char, string name, bool type)
    {
        Char.animator.SetBool(name, type);
    }

    public void IntUpdate(Character Char, string name, int num)
    {
        Char.animator.SetInteger(name, num);
    }

    protected abstract void FindTarget();

    public void KillTarget(Enemy target)
    {
        BoolUpdate(target, "Alive", false);

        EnemySpawner.EnemyArray.Remove(target);

        target.StopAllCoroutines();

        StartCoroutine(Hit(target, 1.3f));

        Destroy(target.HpHUD.gameObject, 0.7f);

        Destroy(target.gameObject, 1.5f);

        GameManager.instance.score += target.Point;

        // GameManager.instance.SkillLeftCount++;

        GameManager.instance.KillCount++;

        GameManager.instance.ScoreUpdate();

        GameManager.instance.SkillLeftUpdate();

        // 다음 스테이지 넘어갈 수 있나 확인
        GameManager.instance.IsNextLevel();
    }
  

    public void KillTarget(Player target)
    {
        BoolUpdate(target, "PlayerAlive", false);

        target.StopAllCoroutines();

        // 임시로 배리어 이펙트만 캐릭터 사망 시 없애도록 함
        Destroy(Defence.effectPrefab);

        target.PlayerHit(1.5f);
    }

    protected abstract IEnumerator Move();
    protected abstract IEnumerator Attack();
    public abstract void Hurt(int damage, float KnockBackRange);

    public IEnumerator KnockBack(Enemy target, float KnockBackRange)
    {
        SmoothCheckTime = Time.fixedTime;

        Vector3 vec = target.transform.localPosition + KnockBackRange * target.KnockBackResistance * Vector3.right;

        StartCoroutine(Hit(target, 0.7f));

        while (target != null
            && SmoothCheckTime + target.KnockBackTime > Time.fixedTime)
        {
            target.transform.localPosition = new Vector3(Mathf.SmoothDamp(target.transform.localPosition.x, vec.x, ref KnockBackVelocity, target.KnockBackTime),
                vec.y,
                vec.z);

           // target.HpHUD.transform.position = Camera.main.WorldToScreenPoint(target.transform.localPosition + Vector3.up * target.transform.localScale.y * 0.4f);

            target.HpHUD.transform.position = Camera.main.WorldToScreenPoint(target.transform.localPosition + Vector3.up * target.Height);

            yield return null;
        }
        target.IsKnock = false;

        yield return null;
    }
    public IEnumerator Hit(Character target, float timer)
    {
        int countTime = 0;
        if (target.Hp > 0)
        {
            while (countTime < 10)
            {
                if (countTime % 2 == 0) target.spr.color = new Color32(255, 255, 255, 90);

                else target.spr.color = new Color32(255, 255, 255, 180);

                countTime++;
                yield return new WaitForSeconds(timer * 0.1f);
            }
                target.spr.color = Color.white;

                target.IsHit = false;

            countTime = 0;
        }
        else
        {
            while (countTime < 10)
            {
                target.spr.color = new Color(1f, 1f, 1f, 0.1f * (10 - countTime++));

                yield return new WaitForSeconds(timer * 0.1f);
            }
        }
    }
}
