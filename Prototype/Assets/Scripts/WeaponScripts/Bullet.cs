using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//TODO: ========> Will doing

public class Bullet : BaseWeapon
{
    public AnimationCurve ease_y_velocity;
    public Transform target;
    public Transform from;
    private float timeStart;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            from = transform;
            timeStart = Time.time;
            Move(target, transform);
        }
    }

    public override void ChangeSpecial()
    {
        Debug.LogError("Bullet Special");
    }

    public override void DirectAttack(Vector2 dir)
    {
        LogSystem.LogWarning("Bullet Attack");
    }

    public void Move(Transform target, Transform _from)
    {
        Vector2 mid = target.position - from.position + Vector3.up;
        StartCoroutine(PositionBezier(from.position, target.position, mid, 10, timeStart));
    }

    private IEnumerator PositionBezier(Vector2 from, Vector2 to, Vector2 middle, float speed, float timeStart)
    {
        yield return null;

        while (true)
        {
            yield return null;
            var deltaTime = Time.time - timeStart;
            Vector2 pos = Mathf.Pow((1 - (timeStart)), 2) * from + 2 * (1 - timeStart) * timeStart * middle + Mathf.Pow(timeStart, 2) * to;

            transform.position = pos;

            if (Vector2.Distance(to, pos) < 0.5f)
                break;
        }
        
    }

    public override void Move(Vector2 target)
    {
        throw new System.NotImplementedException();
    }
}
