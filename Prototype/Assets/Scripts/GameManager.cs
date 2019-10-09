using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private AState[] states;

    public AState currentState;

    public bool move_AVersion;

    public Camera cam;

    public void SwitchState(AState _state)
    {

    }
}


[System.Serializable]
public abstract class AState : MonoBehaviour
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Tick();
}

public class TagManager
{
    public const string ENEMY = "Enemy";
    public const string WEAPON = "Weapon";
}
