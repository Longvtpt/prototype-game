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

public class Hero : MonoBehaviour
{
    //Temp
    public int level;
    public int damage;
    public int mana = 100;

    [HideInInspector]
    public bool canAttack;
    public float timeCooldownBase;
    public float timeCooldownSkill;

    public HeroState state;

    private int slotIndex;
    private Animator anim;
    private float timeCooldownAttack;

    [SerializeField]
    private Transform weaponPos;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            SetAnimation(HeroState.AttackBase);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            SetAnimation(HeroState.AttackSkill);
        }

        var enemy = GetEnemyNear();
        //Temp: Set state by enemy
        if (canAttack && timeCooldownAttack <= 0 && enemy != null)
        {
            //Can use skill?
            if (mana >= 50)
            {
                SwitchState(HeroState.AttackSkill);

                mana -= 50;
                timeCooldownAttack = timeCooldownSkill;
            }
            else
            {
                //Base attack
                SwitchState(HeroState.AttackBase);

                timeCooldownAttack = timeCooldownBase;

                //Create Weapon
                var weapon = InstantiateArrow();
                weapon.DirectAttack((enemy.transform.position - weaponPos.position).normalized);
                weapon.Move(enemy);
            }
        }
    }

    private BaseWeapon InstantiateArrow()
    {
        var obj = PoolManager.Instance.PopPool(PoolName.ARROW.ToString(), weaponPos.position, Quaternion.identity) as GameObject;
        return obj.GetComponent<BaseWeapon>();
    }

    private void LateUpdate()
    {
        TimeCooldownTick();
    }

    private void FixedUpdate()
    {
        if(Time.frameCount % 10 == 0)
        {
            RecruitManaTick();
        }
    }

    private void TimeCooldownTick()
    {
        timeCooldownAttack -= Time.deltaTime;
    }

    private void RecruitManaTick()
    {
        mana += 1;
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

    private Transform GetEnemyNear()
    {
        Transform enemy = null;

        float minDistance = -1;
        var enemies = EnemyManager.Instance.enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemy == null)
            {
                enemy = enemies[i].transform;
                minDistance = Vector2.Distance(enemy.position, transform.position);
            }
            else
            {
                float dis = 0;
                if((dis = Vector2.Distance(transform.position, enemies[i].transform.position)) < minDistance)
                {
                    enemy = enemies[i].transform;
                    minDistance = dis;
                }
            }
        }
        return enemy;
    }
}

public interface Skill
{
    void Active();
}

