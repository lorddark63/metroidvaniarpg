using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;

    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButton selectedTab;

    public List<GameObject> objectsToSwap;
    public Image sectionIcon;
    public List<Sprite> sectionTabImages;
    
    public void Subscribe(TabButton btn)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(btn);
    }

    public void OnTabEnter(TabButton btn)
    {
        ResetTabs();
        if(selectedTab == null || btn != selectedTab)
        {
            btn.background.sprite = tabHover;
        }
    }

    public void OnTabExit(TabButton btn)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButton btn)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = btn;

        selectedTab.Selected();

        ResetTabs();
        
        btn.background.sprite = tabActive;

        int index = btn.transform.GetSiblingIndex();
    
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
                sectionIcon.sprite = sectionTabImages[i];
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (var btn in tabButtons)
        {
            if(selectedTab != null && btn == selectedTab) 
                continue;

            btn.background.sprite = tabIdle;
        }
    }
}
