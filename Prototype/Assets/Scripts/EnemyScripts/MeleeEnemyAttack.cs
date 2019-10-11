using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MeleeEnemyAttack : MonoBehaviour
{
    private BaseEnemy enemy;

    private Collider2D coll;
    private void Start()
    {
        coll = GetComponent<Collider2D>();
        coll.enabled = false;

        enemy = transform.GetComponentInParent<BaseEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Hero"))
        {
            var hero = other.GetComponent<Hero>();
            hero.Damaged(enemy.damageAttack);

            coll.enabled = false;
        }
    }
}
