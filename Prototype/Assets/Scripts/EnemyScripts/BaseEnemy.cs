using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum EnemyState
{
    Idle,
    Move,
    Attack
}

[RequireComponent(typeof(Animator))]
public class BaseEnemy : MonoBehaviour
{
    public PoolName enemyType;
    public const float MOVE_TIMEBASE_TO_HERO = 5f;
    public const float MOVE_TIMEBASE_TO_POS = 1f;

    private int enemyIndex;
    private Animator anim;
    private EnemyState state;

    public int health;
    [HideInInspector]
    public HpMaskControl hpMask;
    public int coinHolding = 1;
    public int floorIndex;
    public float enemySpeed;
    public bool isMove;
    public int damageAttack;

    public float timeCooldownAttack = 1f;
    private float timeCooldown;
    private bool canAttack;

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField]
    private float attackRange = 1f;
#endif


    //[SerializeField]
    private int healthCurrent;

    void Start()
    {
        anim = GetComponent<Animator>();
        SwitchState(EnemyState.Move);
    }

    private void OnEnable()
    {
        timeCooldown = 0;
        healthCurrent = health;
        EnemyManager.Instance.AddEnemy(this);

        if(anim != null)
            SwitchState(EnemyState.Move);

        //UI
        LevelManager.Instance.topUI.AddHealth(health);

        //Reset health bar
        if (hpMask != null)
            hpMask.ResetHealthBar();
    }

    private void Update()
    {
        if (canAttack && timeCooldown <= 0)
            SwitchState(EnemyState.Attack);
    }

    private void FixedUpdate()
    {
#if UNITY_EDITOR
        ShowAttackRange();
#endif

        TickTimeAttack();
    }

    private void TickTimeAttack()
    {
        if (timeCooldown > 0)
            timeCooldown -= Time.deltaTime;
    }

    private IEnumerator MoveTo()
    {
        yield return new WaitForSeconds(0.1f);


        if (GameManager.Instance.move_AVersion)
        {
            if (isMove)
            {
                transform.DOMoveX(EnemyManager.Instance.heroPos[floorIndex].position.x + 1, MOVE_TIMEBASE_TO_HERO * enemySpeed, false).SetEase(Ease.Linear);
            }
            else
            {
                var time = MOVE_TIMEBASE_TO_POS * enemySpeed;
                transform.DOMoveX(EnemyManager.Instance.enemyPos[floorIndex].position.x, MOVE_TIMEBASE_TO_POS * enemySpeed).SetEase(Ease.Linear);
                yield return new WaitForSeconds(time);

                //Change enemy state
                canAttack = true;
            }
        }
        else
        {
            transform.DOMoveX(transform.position.x - 1.5f, 0.5f).SetEase(Ease.InOutQuart);
            yield return WaitForSecondCache.WAIT_TIME_HAFT;

            if (isMove)
            {
                var timeMove = MOVE_TIMEBASE_TO_HERO * enemySpeed;
                transform.DOMoveX(EnemyManager.Instance.heroPos[floorIndex].position.x + 0.5f, timeMove, false).SetEase(Ease.Linear);
                yield return new WaitForSeconds(timeMove);
            }

            canAttack = true;
        }
    }

    private void OnDisable()
    {
        transform.DOKill(true);

        if(EnemyManager.Instance != null)
            EnemyManager.Instance.DeleteEnemy(this);
    }

    private void SwitchState(EnemyState _state)
    {
        state = _state;
        SetAnimation(_state);

        if (_state == EnemyState.Move)
        {
            canAttack = false;
            StartCoroutine(MoveTo());
        }

        if (_state == EnemyState.Attack)
        {
            canAttack = true;
            Attack();
        }
    }

    private void SetAnimation(EnemyState _state)
    {
        switch (_state)
        {
            case EnemyState.Idle:
                SwitchAnimation("_idle");
                break;
            case EnemyState.Move:
                SwitchAnimation("_move");
                break;
            case EnemyState.Attack:
                SwitchAnimation("_attack");
                break;
            default:
                break;
        }
    }

    private void SwitchAnimation(string animation)
    {
        anim.SetTrigger(animation);
    }

    public void Damaged(int damaged)
    {
        int decrease = healthCurrent > damaged ? damaged : healthCurrent;
        healthCurrent -= damaged;

        //Hp bar
        //hpMask.SetPositionMask(damaged / (float)health);
        hpMask.SetRatio(damaged / (float)health);
        //UI
        LevelManager.Instance.topUI.InteractWithSlider(decrease);

        CheckDie();
    }

    private void CheckDie()
    {
        if(healthCurrent <= 0)
        {
            //Instantiate coin
            for (int i = 0; i < coinHolding; i++)
            {
                var coinObj = PoolManager.Instance.PopPool(PoolName.COIN.ToString()) as GameObject;
                var pos = Random.insideUnitCircle / 2 + Vector2.down / 4 + new Vector2(transform.position.x, transform.position.y);
                coinObj.transform.position = pos;
            }

            //Effect die

            //Take
            PoolManager.Instance.PushPool(gameObject, enemyType.ToString());
        }
    }

    public void ShowAttackRange()
    {
        Debug.DrawRay(transform.position, Vector3.left * attackRange, Color.red, Time.fixedDeltaTime);
    }

    protected virtual void Attack()
    {
        timeCooldown = timeCooldownAttack;
        //attack action

    }
}
