using DG.Tweening;
using UnityEngine;
using System.Collections;

public class Arrow : BaseWeapon
{
    //public const float TIME_MOVE_DEFAULT = 1f;
    public PoolName WeaponName = PoolName.ARROW;
    public const float VELOCITY_WEAPON = 10f;

    [Header("DOTween")]
    public Ease ease_type;

    private Vector2 dir = Vector2.right;


    private void Start()
    {
        nameWeapon = WeaponName;
    }

    private void Update()
    {
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
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


    public override void Move(Vector3 target)
    {
        //Temp: base move
        //transform.DOPath(path, TIME_MOVE_DEFAULT / speed, PathType.CubicBezier, PathMode.TopDown2D, 10, Color.blue).SetEase(ease_type).;

        var distance = Vector2.Distance(target, transform.position);
        var dir = (target - transform.position).normalized;
        transform.DOMove(target + dir * 5, speed * distance / VELOCITY_WEAPON).SetEase(ease_type).
            OnComplete(() =>
            {
                if(gameObject.activeSelf)
                    PoolManager.Instance.PushPool(gameObject, nameWeapon.ToString());
            });

        //transform.DOLocalMoveX(transform.position.x + 10, TIME_MOVE_DEFAULT * speed).
        //    OnComplete(() => PoolManager.Instance.PushPool(gameObject, PoolName.ARROW.ToString()));
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (counterCollCurrent-- > 0 && other.gameObject.tag.Equals(TagManager.ENEMY))
        {
            var enemy = other.gameObject.GetComponent<BaseEnemy>();
            enemy.Damaged(damage);

            PoolManager.Instance.PushPool(gameObject, WeaponName.ToString());
        }
    }
}
