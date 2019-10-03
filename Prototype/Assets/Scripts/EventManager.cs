using System;
using System.Collections.Generic;

public enum GameEvent
{
    //Character
    MOVE_CHARACTER,
    ATTACK_CHARACTER,

    //Enemy
    DIE_ENEMY,

    //Turn base
    ROUND_DONE,
    LEVEL_COMPLETE
}

public class EventManager
{
    public static Dictionary<GameEvent, List<Action>> EventDictionary = new Dictionary<GameEvent, List<Action>>();

    public static void AddListener(GameEvent _event, Action method)
    {
        if (!EventDictionary.ContainsKey(_event))
            EventDictionary.Add(_event, new List<Action>());

        EventDictionary[_event].Add(method);
    }

    public static void RemoveListener(GameEvent _event, Action method)
    {
        if (EventDictionary.ContainsKey(_event))
            EventDictionary[_event].Remove(method);
    }

    public static void CallEvent(GameEvent _event)
    {
        if (!EventDictionary.ContainsKey(_event))
            return;
        for (int i = 0; i < EventDictionary[_event].Count; i++)
        {
            EventDictionary[_event][i].Invoke();
        }
    }
}