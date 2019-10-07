using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int enemyNumber;
    [SerializeField]
    private EnemyWave[] enemies;
    [SerializeField]
    private float timeCooldown;

    [SerializeField]
    private bool isSpawn;

    private int counter;

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        yield return null;
        while (counter++ < enemyNumber)
        {
            yield return new WaitForSeconds(timeCooldown);

            if (isSpawn)
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

                var enemyPos = Random.insideUnitCircle / 3f + new Vector2(transform.position.x, transform.position.y) + Vector2.right;
                var enemy = PoolManager.Instance.PopPool(enemies[randIndex].enemy.ToString(), enemyPos, Quaternion.identity);
                counter++;
            }
        }
    }
}

[System.Serializable]
public struct EnemyWave
{
    public PoolName enemy;
    public int ratio;
}
