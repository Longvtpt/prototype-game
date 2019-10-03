using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Arrow : BaseWeapon
{
    //public const float TIME_MOVE_DEFAULT = 1f;
    public const float VELOCITY_WEAPON = 10f;

    [Header("DOTween")]
    public Ease ease_type;
    public AnimationCurve curve;

    private Vector2 dir = Vector2.right;

    private void Update()
    {
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        transform.DOKill(true);
    }


    public override void ChangeSpecial()
    {
        throw new System.NotImplementedException();
    }

    public override void DirectAttack(Vector2 _dir)
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, _dir);
        dir = _dir;
    }


    public override void Move(Transform target)
    {
        //Temp: base move
        //transform.DOPath(path, TIME_MOVE_DEFAULT / speed, PathType.CubicBezier, PathMode.TopDown2D, 10, Color.blue).SetEase(ease_type).;

        var distance = Vector2.Distance(target.position, transform.position);
        transform.DOMove(target.position, speed * distance / VELOCITY_WEAPON).SetEase(ease_type).
            OnComplete(() =>
            {
                if(gameObject.activeSelf)
                    PoolManager.Instance.PushPool(gameObject, PoolName.ARROW.ToString());
            });

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
