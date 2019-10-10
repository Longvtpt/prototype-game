using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigArrowSkill : ASkill
{
    [SerializeField]
    private GameObject PrefabSkill;

    public override void ActiveSkill(Vector2 from, Vector2 to)
    {
        //Instantiate a fire arrow
        //LogSystem.LogSuccess("Fire skill actived!");
        StartCoroutine(Skill(from, to));
        
    }

    IEnumerator Skill(Vector2 from, Vector2 to)
    {
        yield return new WaitForSeconds(timeActionAnim);

        var obj = PoolManager.Instance.PopPool(PoolName.SPECIAL_ARROW.ToString(), from) as GameObject;
        var baseWeapon = obj.GetComponent<BaseWeapon>();
        var dir = (to - from).normalized;
        baseWeapon.DirectAttack(dir);
        baseWeapon.Move(to + dir * 5);
    }


    public override void CancelSkill()
    {
        canActive = false;
    }
}
