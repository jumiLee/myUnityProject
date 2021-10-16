using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections.Generic;
using System;

namespace Service
{
    public class MessageBoxControl : MonoBehaviour
    {
        public UserSession userSession;
        public CommonUtil commonUtil;

        public GameObject prefab;   //item prefab for generating item icon
        public RectTransform content;   //item list will be added this panel

        public UserReceiveBoxPacket userReceiveBoxPacket;
        public List<MsgBoxPrefabView> views = new List<MsgBoxPrefabView>();

        private HttpSock httpSock;
        private int user_account;

        private void Start()
        {
            httpSock = userSession._HttpObject;
        }

        //Search My Inventory Items by Item Category 
        public void GetUserReceiveBox()
        {
            string json = httpSock.Connect("selectReceiveBox.do",
                                            "job_code=1"
                                          + "&user_account=" + userSession._UserDetail.account );

            userReceiveBoxPacket = JsonUtility.FromJson<UserReceiveBoxPacket>(json);

            //Generate item list
            if (userReceiveBoxPacket.resultCd == 0)
            {
                GenerateItemList(userReceiveBoxPacket.userReceiveBoxList);
            }
            else
            {
                commonUtil.HandleAlert(userReceiveBoxPacket.resultMsg);
            }
        }

        //Receive Message Reward
        public void ReceiveReward(int user_account, int receive_sn)
        {
            int job_code = (int)ReceiveBoxJobType.RECEIVE_BOX_RECEIVE_ONE;

            string json = httpSock.Connect( "mgmtReceiveBox.do",
                                            "job_code=" + job_code
                                          + "&user_account=" + user_account
                                          + "&receive_sn=" + receive_sn);

            ResultPacket resultPacket = JsonUtility.FromJson<ResultPacket>(json);

           //Generate item list
           if (resultPacket.resultCd == 0)
           {
                    GetUserReceiveBox();
           }
           else
           {
               commonUtil.HandleAlert(resultPacket.resultMsg);
           }
        }

        //Generate items list with item prefab icon
        void GenerateItemList(List<UserReceiveBox> list)
        {
            //Destory pre-generated views
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            views.Clear();

            //Generate new item list with itemList 
            foreach (var model in list)
            {
                GameObject instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);

                //생성한 Item Icon에 값 설정  
                var view = InitializeItemView(instance, model);

                //생성한 버튼에 onClick Event 생성 
                if (instance.GetComponentInChildren<Button>())
                {
                    instance.GetComponentInChildren<Button>()
                        .onClick.AddListener(() => ReceiveReward(model.user_account, model.receive_sn));
                }
                else
                {
                    Debug.Log("object instantiate Failed!");
                }
                views.Add(view);
            }
        }

        //TODO image check
        //make item view according to item class
        MsgBoxPrefabView InitializeItemView(GameObject viewGameObject, UserReceiveBox model)
        {
            MsgBoxPrefabView view = new MsgBoxPrefabView(viewGameObject.transform);

            view.message.text   = model.receive_msg.ToString();
            view.sender.text    = model.sender_nickname.ToString();
            DateTime myDate     = DateTime.Parse(model.issue_dt);
            view.issueDate.text = myDate.ToString("yyyy.MM.dd.HH.mm.ss");
            view.rwdImg.sprite  = Resources.Load<Sprite>("Images/rwd_type/rwd_type_" + model.rwd_type.ToString()) as Sprite;

            return view;
        }

        [SerializeField]
        public class MsgBoxPrefabView
        {
            public Text message;
            public Text sender;
            public Text issueDate;
            public Image rwdImg;

            public MsgBoxPrefabView(Transform rootView)
            {
                message     = rootView.Find("txt_message").GetComponent<Text>();
                sender      = rootView.Find("txt_sender").GetComponent<Text>();
                issueDate   = rootView.Find("txt_issue_dt").GetComponent<Text>();
                rwdImg      = rootView.Find("img_rwd").GetComponent<Image>();
            }
        }
    }
}