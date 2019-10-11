using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ADEnemy : BaseEnemy
{
    [SerializeField] private PoolName bulletName;

    [SerializeField] private Transform spawnWeaponPos;

    protected override void Attack()
    {
        base.Attack();

        //Instantiate bullet
        var bulletObj = PoolManager.Instance.PopPool(bulletName.ToString(), spawnWeaponPos.position, Quaternion.identity) as GameObject;
        var bullet = bulletObj.GetComponent<ADEnemyAttack>();
        bullet.SetupDamage(damageAttack);
    }
}
