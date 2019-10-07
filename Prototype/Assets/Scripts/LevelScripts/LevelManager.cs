using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private Level[] levels;

    public Transform[] enemySpawnPos;

    public float timeNextTo;
    public WaitForSeconds timeNext;

    [HideInInspector]
    public bool isNextLevel;

    private int levelCurrent = -1;
    private float timeCanNextLevelDefault = 3f;

    private void Start()
    {
        isNextLevel = true;
        timeNext = new WaitForSeconds(timeNextTo);
    }

    private void LateUpdate()
    {
        if (isNextLevel)
        {
            isNextLevel = false;
            levelCurrent++;

            if(levelCurrent < levels.Length)
                LogSystem.LogWarning("Level: " + levelCurrent);
            else
            {
                LogSystem.LogByColor("Game Done!!!!!!", "grey");
                return;
            }

            StartCoroutine(levels[levelCurrent].PlayLevel());
            StartCoroutine(levels[levelCurrent].SpawnFunc());
        }

        if(!isNextLevel && EnemyManager.Instance.enemies.Count == 0 && timeCanNextLevelDefault <= 0)
        {
            isNextLevel = true;
            timeCanNextLevelDefault = 3f;
        }

        TickTimeCanNextLevel();
    }

    private void TickTimeCanNextLevel()
    {
        if (timeCanNextLevelDefault > 0)
            timeCanNextLevelDefault -= Time.deltaTime;
    }
}


[System.Serializable]
public class Level
{
    public SpawnIE SpawnFunc;
    //Default 10 waves
    [SerializeField] private Wave[] waves;
    private bool isNextLevel;

    public IEnumerator PlayLevel()
    {
        //Change UI


        //active
        for (int i = 0; i < waves.Length; i++)
        {
            LogSystem.LogSuccess("Wave: " + i);
            SpawnFunc = waves[i].ReturnFunc();

            while (!IsNextWave(waves[i]))
            {
                yield return LevelManager.Instance.timeNext;

            }
        }

        //LevelManager.Instance.isNextLevel = true;
        isNextLevel = true;
    }

    public bool NextLevel()
    {
        return isNextLevel;
    }

    public bool IsNextWave(Wave wave)
    {
        return wave.WaveDone();
    }
}

public delegate IEnumerator SpawnIE();

[System.Serializable]
public class Wave
{
    [SerializeField] private int enemyNumber;
    //[SerializeField] private float timeCooldownSpawn;
    [SerializeField] private EnemyWave[] enemies;

    private int counter;

    public SpawnIE ReturnFunc()
    {
        return SpawnEnemy;
    }

    public IEnumerator SpawnEnemy()
    {
        yield return null;
        while (counter++ < enemyNumber)
        {
            var rand = Random.Range(0, 100);
            int randIndex = 0;

            int totalRatio = 0;
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies.Length == 1)
                {
                    randIndex = 0;
                    break;
                }

                if (rand < enemies[i].ratio + totalRatio && rand >= totalRatio)
                {
                    randIndex = i;
                    break;
                }
                else
                {
                    totalRatio += enemies[i].ratio;
                }
            }

            var enemy = PoolManager.Instance.PopPool(enemies[randIndex].enemy.ToString());
            var enemyBase = enemy.GetComponent<BaseEnemy>();
            var floorPos = LevelManager.Instance.enemySpawnPos[enemyBase.floorIndex].position;
            enemy.transform.position = (Random.insideUnitCircle / 3f) + new Vector2(floorPos.x + 1, floorPos.y);

            counter++;
            yield return LevelManager.Instance.timeNext;
        }
    }

    public bool WaveDone()
    {
        return counter >= enemyNumber;
    }
}
