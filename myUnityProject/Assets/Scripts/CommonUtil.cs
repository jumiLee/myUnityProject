using UnityEngine;
using UnityEngine.UI;


public class CommonUtil : MonoBehaviour
{

    public GameObject panelAlert;
    public string url;

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