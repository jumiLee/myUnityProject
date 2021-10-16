using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Entity;

namespace PrefabView
{
    [SerializeField]
    public class MsgBoxPrefabView2 : MonoBehaviour
    {
        public Text message;
        public Text sender;
        public Text issueDate;
        public Sprite rwdImg;

        /*
        public MsgBoxPrefabView(Transform rootView)
        {
            //message.text = rootView.receive_msg.ToString();
            //sender.text = rootView.sender_nickname.ToString();
            //issueDate.text = rootView.issue_dt.ToString();
            //rwdImg = rootView.Find("img_item").GetComponent<Sprite>();
        }
        */
    }

}
