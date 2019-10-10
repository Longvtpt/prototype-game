using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockSkill : ASkill
{
    public override void ActiveSkill(Vector2 from, Vector2 to)
    {
        LogSystem.LogSuccess("ShockSkill");
    }

    public override void CancelSkill()
    {
    }
}
