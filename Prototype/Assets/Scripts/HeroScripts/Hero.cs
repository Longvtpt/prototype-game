using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum HeroState
{
    Idle,
    Move,
    AttackBase,
    AttackSkill,
    Die
}

[RequireComponent(typeof(ASkill))]
public class Hero : MonoBehaviour
{
    //Temp
    public string nameHero;
    public int level;
    public int hpBase;
    public int damage;
    public Sprite skillUISprite;
    public Texture heroIcon;
    public PoolName baseAttackItem = PoolName.ARROW;

    [HideInInspector]
    public bool canAttack;
    public float timeCooldownBase;

    public HeroState state;

    private int hpCurrent;
    private int slotIndex;
    private Animator anim;
    private SpriteRenderer heroSprite;
    private Collider2D coll;

    private float timeCooldownAttack;
    private float timeCooldownSkill;

    [HideInInspector]
    public float TimeSkill;

    [SerializeField]
    private Transform weaponPos;

    private ASkill[] skills;

    private Transform enemyTarget;

    [SerializeField] private float timeDie;
    private float timeDieCurrent;
    public bool isDie;
    private Color dieColor;
    private Color aliveColor;
    [SerializeField] private GameObject ghostPrefab;
    private Transform ghost;


    [Header("Update Level Info")]
    [SerializeField] private int increaseDamage;
    [SerializeField] private int increaseHp;

    private void Start()
    {
        hpCurrent = hpBase;
        anim = GetComponent<Animator>();
        heroSprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        aliveColor = heroSprite.color;
        dieColor = new Color(heroSprite.color.r, heroSprite.color.g, heroSprite.color.b, 0.3f);

        skills = GetComponents<ASkill>();

        TimeSkill = skills[0].timeCooldown;

        //Events
        //EventManager.AddListener(GameEvent.DIE_HERO, Die);
        EventManagerWithParam<Hero>.AddListener(GameEvent.DIE_HERO, Die);
        EventManagerWithParam<Hero>.AddListener(GameEvent.REVIVAL_HERO, Revival);
    }

    private void Update()
    {
        if (isDie)
            return;

        enemyTarget = EnemyManager.Instance.GetEnemyNear(transform);
        //Temp: Set state by enemy
        if (canAttack && timeCooldownAttack <= 0 && enemyTarget != null)
        {

            //Base attack
            SwitchState(HeroState.AttackBase);

            timeCooldownAttack = timeCooldownBase;

            //Create Weapon
            var weapon = InstantiateArrow();
            weapon.DirectAttack((enemyTarget.transform.position - weaponPos.position).normalized);
            weapon.Move(enemyTarget.position);

        }
    }

    private void LateUpdate()
    {
        TimeCooldownTick();
        CheckRevival();

    }

    public void UsingSkillActive()
    {
        
        SwitchState(HeroState.AttackSkill);

        timeCooldownSkill = skills[0].timeCooldown;
        timeCooldownAttack = skills[0].timeActionAnim;

        //Active skill
        //Temp: Set skill index by level
        if (enemyTarget == null)
        {
            skills[0].ActiveSkill(transform.position, transform.position + Vector3.right * 10);
        }
        else
        {
            skills[0].ActiveSkill(transform.position, enemyTarget.position);
        }

        //Effect
        GameManager.Instance.skillCamEff.ActiveSkill(timeCooldownAttack, heroSprite);
    }

    private BaseWeapon InstantiateArrow()
    {
        var obj = PoolManager.Instance.PopPool(baseAttackItem.ToString(), weaponPos.position, Quaternion.identity) as GameObject;
        obj.GetComponent<BaseWeapon>().AddDamageFromHero(damage);
        return obj.GetComponent<BaseWeapon>();
    }

    private void TimeCooldownTick()
    {
        if (timeCooldownAttack > 0)
            timeCooldownAttack -= Time.deltaTime;

        if (timeCooldownSkill > 0)
            timeCooldownSkill -= Time.deltaTime;

        if (timeDieCurrent > 0)
            timeDieCurrent -= Time.deltaTime;
    }

    public void SwitchState(HeroState _state)
    {
        //Action
        state = _state;

        SetAnimation(_state);
    }

    public void SetSlotIndex(int index)
    {
        slotIndex = index;
    }

    public int GetSlotIndex()
    {
        return slotIndex;
    }

    public void MoveToPosAttack(Vector3 pos)
    {
        transform.DOMoveX(pos.x, 1.5f).OnComplete(() => canAttack = true);
    }

    private void SetAnimation(HeroState _state)
    {
        switch (_state)
        {
            case HeroState.Idle:
                SwitchAnimation("_idle");
                break;
            case HeroState.Die:
                SwitchAnimation("_die");
                break;
            case HeroState.Move:
                SwitchAnimation("_move");
                break;
            case HeroState.AttackBase:
                SwitchAnimation("_attackBase");
                break;
            case HeroState.AttackSkill:
                SwitchAnimation("_attackSkill");
                break;
            default:
                break;
        }
    }

    private void SwitchAnimation(string animation)
    {
        anim.SetTrigger(animation);
    }

#region Levels
    public void UpLevel()
    {
        level += 1;
        hpBase += increaseHp;
        hpCurrent += increaseHp;
        damage += increaseDamage;
    }
    #endregion

#region Damage
    public void Damaged(int damage)
    {
        hpCurrent -= damage;

        if (hpCurrent <= 0)
            //EventManager.CallEvent(GameEvent.DIE_HERO);
            //Die();
            EventManagerWithParam<Hero>.CallEvent(GameEvent.DIE_HERO, this);
    }

    private void Die(Hero hero)
    {
        if (this != hero)
            return;

        timeDieCurrent = timeDie;
        isDie = true;
        coll.enabled = false;

        //Change 
        var color = heroSprite.color;
        heroSprite.DOColor(dieColor, 1f);
        HeroManager.Instance.HeroDie(this);
        SwitchState(HeroState.Die);

        if(ghost == null)
        {
            //Show ghost
            var ghostObj = Instantiate(ghostPrefab, transform) as GameObject;
            ghostObj.transform.position = transform.position + Vector3.up / 2;
            ghost = ghostObj.transform;
        }
        else
        {
            ghost.gameObject.SetActive(true);
        }
    }

    private void CheckRevival()
    {
        if(isDie == true && timeDieCurrent <= 0)
        {
            EventManagerWithParam<Hero>.CallEvent(GameEvent.REVIVAL_HERO, this);
        }
    }

    private void Revival(Hero hero)
    {
        if (hero != this)
            return;

        hpCurrent = hpBase;
        isDie = false;
        coll.enabled = true;
        heroSprite.DOColor(aliveColor, 0.5f);
        HeroManager.Instance.RevivalHero(this);
        SwitchState(HeroState.Idle);

        ghost.gameObject.SetActive(false);
    }

#endregion
}

