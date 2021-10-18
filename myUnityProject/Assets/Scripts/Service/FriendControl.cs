using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections.Generic;

namespace Service
{
    public class FriendControl : MonoBehaviour
    {
        public UserSession userSession;
        public CommonUtil commonUtil;
        public GameObject Panel_Friend;

        public GameObject prefab;       //friend prefab
        public RectTransform content;   //list will be added on this panel

        private UserFriendPacket userFriendPacket;
        private List<FriendView> views = new List<FriendView>();

        //친구 조회 
        public void GetUserFriendList()
        {

            string json = userSession._HttpObject.Connect("geMyFriendList.do",
                                                          "user_account=" + userSession._UserDetail.account);

            userFriendPacket = JsonUtility.FromJson<UserFriendPacket>(json);

            //Generate item list
            if (userFriendPacket.resultCd == 0)
            {
                GenerateItemList(userFriendPacket.userFriendList);
                Panel_Friend.SetActive(true);
            }
            else
            {
                commonUtil.HandleAlert(userFriendPacket.resultMsg);
            }
        }

        //친구관리 관련 
        public void MgmtFriend(int job_code, int user_account, int frd_account)
        {
            string json = userSession._HttpObject.Connect("mgmtFriend.do",  "user_account=" + user_account +
                                                                            "&job_code="     + job_code +
                                                                            "&frd_account="  + frd_account);

            userFriendPacket = JsonUtility.FromJson<UserFriendPacket>(json);
            if (userFriendPacket.resultCd == 0)
            {
                GenerateItemList(userFriendPacket.userFriendList);
            }
            else
            {
                commonUtil.HandleAlert(userFriendPacket.resultMsg);
            }
        }

        //친구신청 
        public void RequestFriend(int frd_account)
        {
            string json = userSession._HttpObject.Connect("mgmtFriend.do", "user_account=" + userSession._UserDetail.account +
                                                                            "&job_code=" + (int)FriendMgmtType.REQUEST_FRIEND +
                                                                            "&frd_account=" + frd_account);

            userFriendPacket = JsonUtility.FromJson<UserFriendPacket>(json);
            if (userFriendPacket.resultCd == 0)
            {
                commonUtil.HandleAlert("친구신청이 완료되었습니다.");
            }
            else
            {
                commonUtil.HandleAlert(userFriendPacket.resultMsg);
            }
        }

        //Generate list with prefab icon
        void GenerateItemList(List<UserFriend> list)
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

                var view = InitializeItemView(instance, model);
                views.Add(view);
            }
        }

        //mapping prefab view with UserFriend
        FriendView InitializeItemView(GameObject viewGameObject, UserFriend model)
        {
            FriendView view = new FriendView(viewGameObject.transform);

            view.friend_count.text = model.frd_friend_cnt.ToString();
            view.like_count.text = model.frd_following_cnt.ToString();
            view.nickname.text = model.frd_nickname.ToString();

            //친구요청수락버튼 
            view.btn_add.onClick.AddListener(() => MgmtFriend((int)FriendMgmtType.ACCEPT_FRIEND, model.user_account, model.frd_account));

            //친구요청거절버튼 
            view.btn_reject.onClick.AddListener(() => MgmtFriend((int)FriendMgmtType.REJECT_FRIEND, model.user_account, model.frd_account));

            //친구삭제버튼 
            view.btn_delete.onClick.AddListener(() => MgmtFriend((int)FriendMgmtType.DELETE_FRIEND, model.user_account, model.frd_account));

            //이미 친구인 경우 
            if (model.frd_status_cd == 1)
            {
                view.comment.text = model.frd_nickname + "과 이미 친구입니다.";
                view.btn_add.interactable = false;
                view.btn_reject.interactable = false;
                view.btn_delete.interactable = true;
            }
            //친구 요청인 경우
            if (model.frd_status_cd == 2)
            {
                view.comment.text = model.frd_nickname + "님이 친구요청 도착";
                view.btn_add.interactable = true;
                view.btn_reject.interactable = true;
                view.btn_delete.interactable = false;
            }
            return view;
        }

        //AttendPrefab structure
        [SerializeField]
        public class FriendView
        {
            public Text friend_count;
            public Text like_count;
            public Text nickname;
            public Text comment;
            public Image character;
            public Button btn_delete;
            public Button btn_reject;
            public Button btn_add;

            public FriendView(Transform rootView)
            {
                friend_count = rootView.Find("txt_friend_count").GetComponent<Text>();
                like_count   = rootView.Find("txt_like_count").GetComponent<Text>();
                nickname     = rootView.Find("txt_nickname").GetComponent<Text>();
                comment      = rootView.Find("txt_comment").GetComponent<Text>();
                character    = rootView.Find("img_character").GetComponent<Image>();
                btn_delete   = rootView.Find("btn_delete").GetComponent<Button>();
                btn_reject   = rootView.Find("btn_reject").GetComponent<Button>();
                btn_add      = rootView.Find("btn_add").GetComponent<Button>();

            }
        }

    }
}