using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class BaseWeapon : MonoBehaviour
{
    protected PoolName nameWeapon;
    public int damage;
    public float speed;

    public float timeLife;

    private float timeLifeCurrent;

    public abstract void DirectAttack(Vector2 dir);

    public abstract void ChangeSpecial();

    public abstract void Move(Vector3 target);

    //Coll
    [SerializeField]
    private int counterColl = 1;
    protected int counterCollCurrent;

    protected int damageCurrent;

    protected virtual void OnEnable()
    {
        damageCurrent = damage;
        counterCollCurrent = counterColl;
        timeLifeCurrent = timeLife;
    }

    public void AddDamageFromHero(int damageHero)
    {
        damageCurrent += damageHero;
    }

    //protected virtual void OnDisable()
    //{
    //    transform.DOKill(true);
    //}

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
