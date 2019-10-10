using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public SkillUIManager skillUIManager;
    public HeroUITabControl heroUITab;

    [SerializeField]
    private Transform[] slots;

    [SerializeField]
    private GameObject[] heroPrefabs;

    private int slotCount = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && slotCount < 5)
        {
            AddHero(slotCount, slotCount);
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

        //Unlock UI skill
        skillUIManager.AddSkill(slotIndex, heroScript.skillUISprite, heroScript);

        //Add to HeroUI Tab
        heroUITab.AddHero(heroScript);
    }
}
