using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ADEnemyAttack : MonoBehaviour
{
    private int damage;

    [SerializeField] private float speedMove;

    public void SetupDamage(int dmg)
    {
        damage = dmg;
    }

    public void Move()
    {
        //TODO: find hero pos
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Hero"))
        {
            var hero = other.GetComponent<Hero>();
            hero.Damaged(damage);
        }
    }
}
