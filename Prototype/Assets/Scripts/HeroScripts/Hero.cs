using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum HeroState
{
    Idle,
    Move,
    AttackBase,
    AttackSkill
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

    private int slotIndex;
    private Animator anim;
    private SpriteRenderer heroSprite;
    private float timeCooldownAttack;
    private float timeCooldownSkill;

    [HideInInspector]
    public float TimeSkill;

    [SerializeField]
    private Transform weaponPos;

    private ASkill[] skills;

    private Transform enemyTarget;


    [Header("Update Level Info")]
    [SerializeField] private int increaseDamage;
    [SerializeField] private int increaseHp;

    private void Start()
    {
        anim = GetComponent<Animator>();
        heroSprite = GetComponent<SpriteRenderer>();

        skills = GetComponents<ASkill>();

        TimeSkill = skills[0].timeCooldown;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SetAnimation(HeroState.AttackBase);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetAnimation(HeroState.AttackSkill);
        }

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

    private void LateUpdate()
    {
        TimeCooldownTick();
    }

    private void TimeCooldownTick()
    {
        if (timeCooldownAttack > 0)
            timeCooldownAttack -= Time.deltaTime;

        if (timeCooldownSkill > 0)
            timeCooldownSkill -= Time.deltaTime;
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
        damage += increaseDamage;
    }
#endregion
}

