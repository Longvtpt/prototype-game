using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ColliderWithEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag.Equals("Enemy"))
        {
            var enemy = other.GetComponent<BaseEnemy>();
            enemy.Damaged(100);
        }

        GetComponent<Collider2D>().enabled = false;
    }
}
