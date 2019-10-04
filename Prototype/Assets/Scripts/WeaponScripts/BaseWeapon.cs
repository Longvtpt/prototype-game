using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BaseWeapon : MonoBehaviour
{
    public PoolName nameWeapon;
    public int damage;
    public float speed;

    public float timeLife;

    private float timeLifeCurrent;

    public abstract void DirectAttack(Vector2 dir);

    public abstract void ChangeSpecial();

    public abstract void Move(Vector2 target);

    protected virtual void OnEnable()
    {
        timeLifeCurrent = timeLife;
    }

    protected virtual void OnDisable()
    {
        transform.DOKill(true);
    }

    protected virtual void LateUpdate()
    {
        if(timeLifeCurrent <= 0)
        {
            PoolManager.Instance.PushPool(gameObject, nameWeapon.ToString());
        }
        else
        {
            TickTimelife();
        }
    }

    private void TickTimelife()
    {
        timeLifeCurrent -= Time.deltaTime;
    }
}
