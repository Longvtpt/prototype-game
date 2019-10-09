using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Coin : MonoBehaviour
{
    public static Vector2 coinUIPos;
    public float timeMove;

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        yield return WaitForSecondCache.WAIT_TIME_ONE;
        transform.DOMove(coinUIPos, timeMove).OnComplete(() =>
        {
            EventManager.CallEvent(GameEvent.PICK_COIN);

            PoolManager.Instance.PushPool(gameObject, PoolName.COIN.ToString());
        });
    }
}
