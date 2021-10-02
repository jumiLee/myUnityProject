using UnityEngine;
using UnityEngine.UI;


public class CommonUtil : MonoBehaviour
{

    public GameObject panelAlert;
    public string url;
    public GameObject openNewPanel;  //현재창을 닫고 
    public GameObject closeNewPanel; //새로운 창을 열고자 할때 세팅 


    public void HandleAlert(string msg)
    {
        //Open Alert Panel -> show error message
        panelAlert.SetActive(true);
        //panelAlert.GetComponent<Text>().text = msg;
        panelAlert.transform.GetComponent<Text>().text = msg;
        //panelAlert.GetComponent<Text>().text = msg;
    }

    public void ControlPanel(GameObject panel)
    {
        if (panel != null)
        {
            bool isActive = panel.activeSelf;

            panel.SetActive(!isActive);
        }

        //새롭게 열 창
        if (openNewPanel != null)
        {
            bool isActive = openNewPanel.activeSelf;

            openNewPanel.SetActive(!isActive);
        }

        //현재 열려있는 창 
        if (closeNewPanel != null)
        {
            bool isActive = closeNewPanel.activeSelf;

            closeNewPanel.SetActive(!isActive);
        }
    }

    //수정필요
    public void OpenUrl(GameObject panel)
    {
        if (panel != null)
        {
            Application.OpenURL(url);
        }
    }
}