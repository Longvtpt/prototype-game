using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy
{
    [SerializeField] Collider2D collAttack;

    protected override void Attack()
    {
        base.Attack();
        collAttack.enabled = true;
    }
}
