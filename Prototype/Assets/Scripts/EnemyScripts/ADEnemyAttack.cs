using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ADEnemyAttack : MonoBehaviour
{
    private int damage;

    [SerializeField] private float speedMove;

    public void SetupBullet(int dmg)
    {
        damage = dmg;
    }

    public void Move()
    {
        //TODO: find near hero pos
        Transform heroPos = HeroManager.Instance.GetNearHero(transform.position);
        if (heroPos == null)
            return;

        var dir = (heroPos.position - transform.position).normalized;
        transform.DOMove(dir * 10, 3f).SetEase(Ease.Linear).OnComplete(() => PoolManager.Instance.PushPool(gameObject, PoolName.BULLET_ENEMY.ToString()));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Hero"))
        {
            var hero = other.GetComponent<Hero>();
            hero.Damaged(damage);

            //Push to pool
            PoolManager.Instance.PushPool(gameObject, PoolName.BULLET_ENEMY.ToString());
        }
    }
}
