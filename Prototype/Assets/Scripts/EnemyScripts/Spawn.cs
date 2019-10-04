using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public int enemyNumber;
    [SerializeField]
    private GameObject[] enemyPrefabs;
    [SerializeField]
    private float timeCooldown;

    private WaitForSeconds timeCoolDownSpawn;
    [SerializeField]
    private bool isSpawn;

    private int counter;

    private void Start()
    {
        timeCoolDownSpawn = new WaitForSeconds(timeCooldown);

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
                var enemyPos = Random.insideUnitCircle / 3.5f + new Vector2(transform.position.x, transform.position.y) + Vector2.right;
                var enemy = PoolManager.Instance.PopPool(PoolName.BASE_ENEMY.ToString(), enemyPos, Quaternion.identity);
                counter++;
            }
        }
    }
}
