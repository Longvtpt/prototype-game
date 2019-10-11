using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : Singleton<HeroManager>
{
    public SkillUIManager skillUIManager;
    public HeroUITabControl heroUITab;

    [SerializeField]
    private Transform[] slots;

    [SerializeField]
    private GameObject[] heroPrefabs;

    public int heroCanAttack = 0;
    private int slotCount = 0;

    public List<Hero> heroDieList = new List<Hero>();
    private List<Transform> heroPosList = new List<Transform>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddHeroAction();
        }
    }

    public void AddHeroAction()
    {
        if (slotCount >= 5)
            return;

        AddHero(slotCount, slotCount);
        slotCount++;
    }

    private void AddHero(int slotIndex, int heroIndex)
    {
        var hero = Instantiate(heroPrefabs[heroIndex]) as GameObject;
        var heroScript = hero.GetComponent<Hero>();
        heroScript.SetSlotIndex(slotIndex);

        hero.transform.position = slots[slotIndex].transform.position - Vector3.right * 2;
        heroScript.MoveToPosAttack(slots[slotIndex].transform.position);
        StartCoroutine(AddToListHeroPos(hero.transform));

        //Unlock UI skill
        skillUIManager.AddSkill(slotIndex, heroScript.skillUISprite, heroScript);

        //Add to HeroUI Tab
        heroUITab.AddHeroToUI(heroScript);
    }

    private IEnumerator AddToListHeroPos(Transform pos)
    {
        yield return WaitForSecondCache.WAIT_TIME_ONE_POINT_FIVE;
        heroCanAttack++;
        heroPosList.Add(pos);
    }

    public Transform GetNearHero(Vector2 pos)
    {
        Transform target = null;
        float minDistance = -1;

        for (int i = 0; i < heroPosList.Count; i++)
        {
            if (heroPosList[i] == null)
                continue;

            var dis = Vector2.Distance(pos, heroPosList[i].position);
            if(minDistance == -1 || dis < minDistance)
            {
                minDistance = dis;
                target = heroPosList[i];
            }
        }
        return target;
    }

    public void HeroDie(Hero hero)
    {
        heroDieList.Add(hero);
        heroCanAttack--;

        heroPosList.Remove(hero.transform);
    }

    public void RevivalHero(Hero hero)
    {
        heroDieList.Remove(hero);
        heroCanAttack++;

        heroPosList.Add(hero.transform);
    }
}