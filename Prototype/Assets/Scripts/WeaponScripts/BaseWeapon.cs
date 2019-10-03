using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    public int damage;
    public int speed;

    public abstract void DirectAttack(Vector2 dir);

    public abstract void ChangeSpecial();

    public abstract void Move(Vector2 target = new Vector2());
}
