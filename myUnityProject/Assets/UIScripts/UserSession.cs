using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;

public class UserSession : MonoBehaviour
{
    public HttpSock _HttpObject;
    public UserDetail _UserDetail;
    public UserCharacter _UserCharacter;
    public NoticeNew _NoticeNew;

    public GameObject Panel_login;
    public GameObject Panel_lobby;

    //user session check
    private void Start()
    {
        if(_UserDetail.account == 0)
        {
            Panel_login.SetActive(true);
        }
        else
        {
            Panel_lobby.SetActive(true);
        }

    }
}