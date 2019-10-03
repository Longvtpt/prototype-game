﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] slots;

    [SerializeField]
    private GameObject[] heroPrefabs;

    private int slotCount;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && slotCount < 5)
        {
            AddHero(slotCount, 0);
            slotCount++;
        }    
    }

    public void AddHero(int slotIndex, int heroIndex)
    {
        var hero = Instantiate(heroPrefabs[heroIndex]) as GameObject;
        var heroScript = hero.GetComponent<Hero>();
        heroScript.SetSlotIndex(slotIndex);

        hero.transform.position = slots[slotIndex].transform.position - Vector3.right * 2;
        heroScript.MoveToPosAttack(slots[slotIndex].transform.position);
    }
}
