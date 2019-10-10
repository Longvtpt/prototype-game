using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCamEff : MonoBehaviour
{
    [SerializeField] private SpriteRenderer border;

    private int counter = 0;

    private void Start()
    {
        border.enabled = false;
    }

    public void ActiveSkill(float timer, SpriteRenderer sprite)
    {
        counter++;
        border.enabled = true;
        sprite.sortingOrder = 5;

        StartCoroutine(Inactive(timer, sprite));
    }

    private IEnumerator Inactive(float timer, SpriteRenderer sprite)
    {
        yield return new WaitForSeconds(timer);

        sprite.sortingOrder = 0;
        counter--;

        if(counter == 0)
            border.enabled = false;
    }
}
