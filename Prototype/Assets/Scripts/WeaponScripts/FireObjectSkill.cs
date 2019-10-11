using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireObjectSkill : Arrow
{
    private int counter;

    protected override void OnEnable()
    {
        base.OnEnable();
        counter = 1;
    }

    private void Start()
    {
        nameWeapon = PoolName.FIRE_SKILL_OBJ;
    }
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals(TagManager.ENEMY) || counter == 1)
        {
            counter--;
            //Effects
            PoolManager.Instance.PopPool(PoolName.EXPLOSION.ToString(), other.transform.position, Quaternion.identity);

            PoolManager.Instance.PushPool(gameObject, nameWeapon.ToString());
        }
    }
}
