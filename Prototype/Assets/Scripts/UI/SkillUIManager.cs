using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    [SerializeField] private SkillUI[] skillBtts;


    public void AddSkill(int index, Sprite imgSkill, Hero hero)
    {
        skillBtts[index].ActiveSkill();
        skillBtts[index].SetImgSkill(imgSkill);
        skillBtts[index].heroTarget = hero;
    }
}
