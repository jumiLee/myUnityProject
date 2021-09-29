using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{

    public List<TabButtonControl> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public TabButtonControl selectedTab;
    public List<GameObject> objectsToSwap;


    public void Subscribe(TabButtonControl button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButtonControl>();
        }
        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButtonControl button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.background.sprite = tabHover;
        } 
    }

    public void OnTabExit(TabButtonControl button)
    {
        ResetTabs();
    }

    public void OnTabSelected(TabButtonControl button)
    {
        if(selectedTab != null)
        {
            selectedTab.Deselect();
        }
        selectedTab = button;
        selectedTab.Select();

        ResetTabs();
        button.background.sprite = tabActive;
        int index = button.transform.GetSiblingIndex();
        for (int i=0; i<objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach(TabButtonControl button in tabButtons)
        {
            if(selectedTab !=null && button == selectedTab) { continue; }
            button.background.sprite = tabIdle;
        }
    }
}
