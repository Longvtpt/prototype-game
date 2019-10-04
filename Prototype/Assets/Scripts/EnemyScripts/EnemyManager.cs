using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    [HideInInspector]
    public List<BaseEnemy> enemies;

    public Transform[] enemyPos;
    public Transform[] heroPos;

    [HideInInspector]
    public int healthTotal;

    void Start()
    {
        enemies = new List<BaseEnemy>();
    }

    //Temp
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("<color=green>" + GetEnemy(0).position.ToString() + "</color>");
        }
    }

    public void AddEnemy(BaseEnemy newEnemy)
    {
        enemies.Add(newEnemy);
    }

    public void DeleteEnemy(BaseEnemy enemy)
    {
        enemies.Remove(enemy);
    }

    public Transform GetEnemy(int index)
    {
        if (index > enemies.Count || index < 0)
        {
            Debug.LogError("Issue with enemy index");
            return null;
        }

        var enemy = enemies[index].transform;
        return enemy;
    }

    public Transform GetEnemyNear(Transform from)
    {
        Transform enemy = null;

        float minDistance = -1;
        var enemies = EnemyManager.Instance.enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemy == null)
            {
                enemy = enemies[i].transform;
                minDistance = Vector2.Distance(enemy.position, from.position);
            }
            else
            {
                float dis = 0;
                if ((dis = Vector2.Distance(from.position, enemies[i].transform.position)) < minDistance)
                {
                    enemy = enemies[i].transform;
                    minDistance = dis;
                }
            }
        }
        return enemy;
    }
}


