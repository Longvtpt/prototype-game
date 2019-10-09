using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TabMenuControl : MonoBehaviour
{
    [SerializeField] private RectTransform[] tabs;

    private float startPos;

    private void Start()
    {
        startPos = -Screen.height * 1.15f;

        TurnOffTabs();
    }

    public void ActiveTab(RectTransform tab)
    {
        if (tab.gameObject.activeSelf)
        {
            InactiveTab(tab);
            return;
        }
        else
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                if(tabs[i].gameObject.activeSelf && tabs[i] != tab)
                {
                    tab.anchoredPosition = new Vector2(tab.anchoredPosition.x, tabs[i].anchoredPosition.y);
                    tab.gameObject.SetActive(true);

                    tabs[i].gameObject.SetActive(false);
                    InactiveTab(tabs[i]);
                    return;
                }
            }
        }

        //Turn on tab and move to top screen
        tab.gameObject.SetActive(true);
        tab.DOAnchorPosY(0, 0.35f);
    }

    private void TurnOffTabs()
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            InactiveTab(tabs[i]);
        }
    }

    private void InactiveTab(RectTransform tab)
    {
        if(tab.gameObject.activeSelf)
            tab.DOAnchorPosY(startPos, 0.35f).OnComplete(() => tab.gameObject.SetActive(false));
        else
            tab.anchoredPosition = new Vector2(tab.anchoredPosition.x, startPos);
    }
}
