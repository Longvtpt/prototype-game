using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialArrow : Arrow
{
    private void Start()
    {
        nameWeapon = PoolName.SPECIAL_ARROW;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals(TagManager.ENEMY))
        {
            //LogSystem.LogSuccess("Damage skill --- " + damage);
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.Damaged(damage);

            //PoolManager.Instance.PushPool(gameObject, PoolName.ARROW.ToString());
        }
    }
}
