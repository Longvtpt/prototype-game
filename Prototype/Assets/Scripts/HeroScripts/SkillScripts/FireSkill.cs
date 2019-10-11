using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : ASkill
{
    public override void ActiveSkill(Vector2 from, Vector2 to)
    {
        StartCoroutine(Skill(from, to));
    }

    IEnumerator Skill(Vector2 from, Vector2 to)
    {
        yield return new WaitForSeconds(timeActionAnim);

        var obj = PoolManager.Instance.PopPool(PoolName.FIRE_SKILL_OBJ.ToString(), from) as GameObject;
        var baseWeapon = obj.GetComponent<BaseWeapon>();
        var dir = (to - from).normalized;
        baseWeapon.DirectAttack(dir);
        baseWeapon.Move(to + dir * 5);
    }

    public override void CancelSkill()
    {
        throw new System.NotImplementedException();
    }

}
