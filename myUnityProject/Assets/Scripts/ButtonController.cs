using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    //open & close panel function
    public void ControlPanel(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
        }
    }

    //open web url function
    public void OpenUrl(GameObject panel)
    { 
        if (panel != null)
        {
            Application.OpenURL("http://unity3d.com/");
        }
    }
}
