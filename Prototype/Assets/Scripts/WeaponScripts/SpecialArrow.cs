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
            //Effects
            PoolManager.Instance.PopPool(PoolName.BLEED.ToString(), other.transform.position, Quaternion.identity);


            //LogSystem.LogSuccess("Damage skill --- " + damage);
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.Damaged(damage);
        }
    }
}
