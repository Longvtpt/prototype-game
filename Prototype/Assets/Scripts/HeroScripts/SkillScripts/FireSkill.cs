﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : ASkill
{
    public override void ActiveSkill(Vector2 from, Vector2 to)
    {
        LogSystem.LogSuccess("Fire skill active");
    }

    public override void CancelSkill()
    {
        throw new System.NotImplementedException();
    }

}
