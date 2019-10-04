using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASkill : MonoBehaviour
{
    public float timeCooldown;
    public int takeMana;
    public bool canActive = true;

    private void Start()
    {
        canActive = true;
    }

    public abstract void ActiveSkill(Vector2 from, Vector2 to);

    public abstract void CancelSkill();
}
