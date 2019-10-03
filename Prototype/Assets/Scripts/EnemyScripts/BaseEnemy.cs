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
    public const float MOVE_TIMEBASE_TO_HERO = 5f;
    public const float MOVE_TIMEBASE_TO_POS = 1f;

    private int enemyIndex;
    private Animator anim;
    private EnemyState state;

    public int health;
    public int floorIndex;
    public float enemySpeed;
    public bool isMove;

    void Start()
    {
        anim = GetComponent<Animator>();

        SwitchState(EnemyState.Move);
    }

    private void OnEnable()
    {

        EnemyManager.Instance.AddEnemy(this);

        StartCoroutine(MoveTo());
    }

    private IEnumerator MoveTo()
    {
        yield return new WaitForSeconds(0.1f);

        if (isMove)
        {
            transform.DOMoveX(EnemyManager.Instance.heroPos[floorIndex].position.x + 1, MOVE_TIMEBASE_TO_HERO * enemySpeed, false);
        }
        else
        {
            transform.DOMoveX(EnemyManager.Instance.enemyPos[floorIndex].position.x + 1, MOVE_TIMEBASE_TO_POS * enemySpeed);
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
        health -= damaged;

        CheckDie();
    }

    private void CheckDie()
    {
        if(health <= 0)
        {
            //Effect die


            //Take
            PoolManager.Instance.PushPool(gameObject, PoolName.BASE_ENEMY.ToString());
        }
    }
}
