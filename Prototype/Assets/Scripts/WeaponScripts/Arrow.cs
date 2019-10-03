using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Arrow : BaseWeapon
{
    public const float TIME_MOVE_DEFAULT = 1f;

    private Vector2 defaultPos;

    private void OnEnable()
    {
        if(defaultPos != Vector2.zero)
            transform.position = defaultPos;
    }

    private void OnDisable()
    {
        transform.DOKill(true);
    }


    public override void ChangeSpecial()
    {
        throw new System.NotImplementedException();
    }

    public override void DirectAttack(Vector2 dir)
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, dir);
    }


    public override void Move(Vector2 target = new Vector2())
    {
        if (defaultPos == null)
            defaultPos = transform.position;

        //Temp: base move
        transform.DOMove(target, TIME_MOVE_DEFAULT / speed).
            OnComplete(() => PoolManager.Instance.PushPool(gameObject, PoolName.ARROW.ToString()));

        //transform.DOLocalMoveX(transform.position.x + 10, TIME_MOVE_DEFAULT * speed).
        //    OnComplete(() => PoolManager.Instance.PushPool(gameObject, PoolName.ARROW.ToString()));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals(TagManager.ENEMY))
        {
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.Damaged(damage);


            PoolManager.Instance.PushPool(gameObject, PoolName.ARROW.ToString());
        }
    }
}
