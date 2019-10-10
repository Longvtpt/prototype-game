using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUITabControl : MonoBehaviour
{
    [SerializeField] private Transform contents;
    [SerializeField] private GameObject elementPrefab;

    public void AddHero(Hero hero)
    {
        var element = Instantiate(elementPrefab, contents) as GameObject;
        var info = element.GetComponent<HeroInfoElement>();

        info.SetupImage(hero.heroIcon);
        info.SetupInfo(hero.nameHero, hero.level.ToString(), hero.hpBase.ToString(), hero.damage.ToString());
    }
}
