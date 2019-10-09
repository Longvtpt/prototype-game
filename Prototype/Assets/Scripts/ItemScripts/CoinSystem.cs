using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : Singleton<CoinSystem>
{
    private int coinTotal;

    protected override void Awake()
    {
        EventManager.AddListener(GameEvent.PICK_COIN, AddCoin);
    }

    public void AddCoin()
    {
        coinTotal += 1;
    }

    public int GetCoinInfo()
    {
        return coinTotal;
    }

    public void AddAlotCoin(int amount)
    {
        coinTotal += amount;
    }

    public void DecreaseCoin(int amount)
    {
        if (coinTotal < amount)
        {
            Debug.LogError("Coint error");
            return;
        }

        coinTotal -= amount;
    }
}
