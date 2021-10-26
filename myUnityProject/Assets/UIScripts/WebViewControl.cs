using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;
using Service;
using Entity;

public class WebViewControl : MonoBehaviour
{

    public CanvasWebViewPrefab canvasWebViewPrefab;
    public UserSession userSession;
    public GameObject targetPanel;  //웹화면을 싸고 있는 부모 패널 
    public GameObject windowPanel;  //실제 웹화면이 나올 패널 

    private HttpSock httpSock;
    private int user_account = 0;
    private string url;

    public void openAttendWebWindow()
    {
        this.user_account = userSession._UserDetail.account;
        this.url = userSession._HttpObject.getMainURL();
            //UserSessionObject.GetComponent<HttpSock>().getMainURL();

        url = url +"userAttend.do?user_account=" + user_account;

        //출석이벤트 창 초기화 
        if (user_account != 0)
        {
            targetPanel.SetActive(true);
            InstanceCanvasWebView();
        }
    }

    //void InstanceCanvasWebView(GameObject _canvas)
    void InstanceCanvasWebView()
    {
        //var canvas = GameObject.Find("Canvas");
        canvasWebViewPrefab = CanvasWebViewPrefab.Instantiate();
        //canvasWebViewPrefab.transform.parent = canvas.transform;
        canvasWebViewPrefab.transform.SetParent(windowPanel.transform, false);
        var rectTransform = canvasWebViewPrefab.transform as RectTransform;
        rectTransform.anchoredPosition3D = Vector3.zero;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        canvasWebViewPrefab.transform.localScale = Vector3.one;
        canvasWebViewPrefab.Initialized += (sender, e) => {
            canvasWebViewPrefab.WebView.LoadUrl(url);
        };
    }
}
