using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CannonControl : MonoBehaviour
{
    [SerializeField]
    private Transform gunHead;
    [SerializeField]
    private Transform attackPos;
    [SerializeField]
    private float timeRotate = 0.5f;
    [SerializeField]
    private float timeCooldownAttack = 0.75f;

    [SerializeField]
    private bool autoRotate;
    [SerializeField]
    private bool autoAttack;

    private Transform enemy;
    private bool canAttack = true;
    private float timeAttack;


    private void Update()
    {
        if (autoAttack && (canAttack && timeAttack <= 0) && enemy != null)
        {
            StartCoroutine(Attack());
        }
        else
            //Temp: input
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(Attack());
            }
    }

    private void LateUpdate()
    {
        TickTimeAttack();
    }

    private void FixedUpdate()
    {
        if (autoRotate && Time.frameCount % 15 == 0)
        {
            FindEnemy();
            if (autoRotate)
                AutoRotate();
        }
    }

    private void TickTimeAttack()
    {
        if (timeAttack > 0)
            timeAttack -= Time.deltaTime;
    }

    private void FindEnemy()
    {
        enemy = EnemyManager.Instance.GetEnemyNear(transform);
    }

    private void AutoRotate()
    {

        if (enemy == null)
        {
            canAttack = false;
            return;
        }
        else
            canAttack = true;

        //Rotate to enemy
        var targetQuar = Quaternion.FromToRotation(Vector2.right, (enemy.position - transform.position).normalized);
        transform.DORotateQuaternion(targetQuar, timeRotate);
    }

    IEnumerator Attack()
    {
        yield return null;
        if (!autoRotate)
        {
            AutoRotate();
            yield return new WaitForSeconds(timeRotate);
        }

        var obj = PoolManager.Instance.PopPool(PoolName.ARROW.ToString(), attackPos.position);
        var weapon = obj.GetComponent<BaseWeapon>();

        if(enemy == null)
        {
            weapon.Move(transform.position + Vector3.right * 5);
            weapon.DirectAttack(Vector2.right);
        }
        else
        {
            weapon.Move(enemy.position);
            weapon.DirectAttack((enemy.position - attackPos.position).normalized);
        }


        timeAttack = timeCooldownAttack;

    }
}