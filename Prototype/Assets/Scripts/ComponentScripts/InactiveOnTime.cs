using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveOnTime : MonoBehaviour
{
    [SerializeField] private float timeToInactive;
    [SerializeField] private PoolName namePrefab;

    IEnumerator Inactive()
    {
        yield return new WaitForSeconds(timeToInactive);

        PoolManager.Instance.PushPool(gameObject, namePrefab.ToString());
    }

    private void OnEnable()
    {
        StartCoroutine(Inactive());
    }
}
