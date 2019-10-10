using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class LevelManager : Singleton<LevelManager>
{
    public TopUIControl topUI;

    [SerializeField] private Level[] levels;

    public Transform[] enemySpawnPos;

    public float timeNextTo;

    [HideInInspector]
    public bool isNextLevel;

    public int levelCurrent = 0;
    private float timeCanNextLevelDefault = 1f;

    private void Start()
    {
        isNextLevel = true;
    }

    private void LateUpdate()
    {
        if (isNextLevel)
        {
            isNextLevel = false;
            timeCanNextLevelDefault = 3f;
            levelCurrent++;

            if(levelCurrent >= levels.Length)
            {
                LogSystem.LogByColor("Game Done!!!!!!", "grey");
                return;
            }

            StartCoroutine(levels[levelCurrent].PlayLevel());
        }

        //if(!isNextLevel && EnemyManager.Instance.enemies.Count == 0 && timeCanNextLevelDefault <= 0)
        //{
        //    isNextLevel = true;
        //    timeCanNextLevelDefault = 3f;
        //}

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
    //Default 10 waves
    [SerializeField] private Wave[] waves;

    public IEnumerator PlayLevel()
    {
        //UI
        LevelManager.Instance.topUI.SetupWaveAndLevel(LevelManager.Instance.levelCurrent, waves.Length);

        yield return WaitForSecondCache.WAIT_TIME_ONE;

        //active
        for (int i = 0; i < waves.Length; i++)
        {
            CoroutineHandler.StartStaticCoroutine(waves[i].SpawnEnemy(i));

            //All enemy is died
            while (!IsNextWave(waves[i]))
            {
                yield return WaitForSecondCache.WAIT_TIME_ONE;
            }
        }

        LevelManager.Instance.isNextLevel = true;
    }

    public bool IsNextWave(Wave wave)
    {
        return wave.WaveDone();
    }
}


[System.Serializable]
public class Wave
{
    [Range(1, 5)]
    [SerializeField] private int enemyNumber;
    //[SerializeField] private float timeCooldownSpawn;
    [SerializeField] private EnemyWave[] enemies;

    private int counter;

    public IEnumerator SpawnEnemy(int waveIndex)
    {
        yield return null;

        //UI
        LevelManager.Instance.topUI.ResetWaveUI(waveIndex);

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
            enemy.transform.position = (Random.insideUnitCircle / 2f) + new Vector2(floorPos.x + 1.5f, floorPos.y);

            //Debug.Log(counter);

            if (GameManager.Instance.move_AVersion)
                yield return WaitForSecondCache.WAIT_TIME_TWO;
            else
                yield return WaitForSecondCache.WAIT_TIME_MIN;

            ////UI
            //LevelManager.Instance.topUI.AddHealth(enemyBase.health);
        }
    }

    public bool WaveDone()
    {
        return counter >= enemyNumber && EnemyManager.Instance.enemies.Count == 0;
    }
}

[System.Serializable]
public struct EnemyWave
{
    public PoolName enemy;
    public int ratio;
}
