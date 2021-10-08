using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;
using System.Collections.Generic;

namespace Service
{
    public class AttendControl : MonoBehaviour
    {
        public HttpSock httpSock;
        public MemberService memberService;
        public CommonUtil commonUtil;

        public GameObject prefab;       //Attend prefab
        public RectTransform content;   //list will be added on this panel
        public UserAttendPacket userAttendPacket;
        public List<AttendView> views = new List<AttendView>();
    
        private void Start()
        {
            GetUserAttendList();
        }

        //출석 조회 
        public void GetUserAttendList()
        {

            string json = httpSock.Connect("selectUserAttend.do",
                                           "user_account=" + memberService._MemberInfoPacket.account);

            userAttendPacket = JsonUtility.FromJson<UserAttendPacket>(json);

            //Generate item list
            if (userAttendPacket.resultCd == 0)
            {
                GenerateItemList(userAttendPacket.userAttendList);
            }
            else
            {
                commonUtil.HandleAlert(userAttendPacket.resultMsg);
            }
        }

        //출석 등록  
        public void RegisterUserAttend()
        {

            string json = httpSock.Connect("registerUserAttend.do",
                                           "user_account=" + memberService._MemberInfoPacket.account);

            userAttendPacket = JsonUtility.FromJson<UserAttendPacket>(json);

            //Generate item list
            if (userAttendPacket.resultCd == 0)
            {
                GenerateItemList(userAttendPacket.userAttendList);
            }
            else
            {
                commonUtil.HandleAlert(userAttendPacket.resultMsg);
            }
        }

        //Generate list with prefab icon
        void GenerateItemList(List<UserAttend> userAttendList)
        {
            //Destory pre-generated views
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }
            views.Clear();

            //Generate new item list with itemList 
            foreach (var model in userAttendList)
            {
                GameObject instance = GameObject.Instantiate(prefab.gameObject) as GameObject;
                instance.transform.SetParent(content, false);

                //생성한 Item Icon에 값 설정  
                var view = InitializeItemView(instance, model);
                views.Add(view);
            }
        }

        //make item view according to item class
        AttendView InitializeItemView(GameObject viewGameObject, UserAttend model)
        {
            AttendView view = new AttendView(viewGameObject.transform);

            view.day_no.text = model.day_no.ToString() + "일";
            view.rwd_id.text = model.rwd_id.ToString();
            view.rwd_type = Resources.Load<Sprite>("Images/icon/" + model.rwd_type.ToString()) as Sprite;

            return view;
        }

        //AttendPrefab structure
        [SerializeField]
        public class AttendView
        {
            public Text day_no;
            public Text rwd_id;
            public Sprite rwd_type;

            public AttendView(Transform rootView)
            {
                day_no  = rootView.Find("txt_day_no").GetComponent<Text>();
                rwd_id  = rootView.Find("txt_rwd_id").GetComponent<Text>();
                rwd_type= rootView.Find("img_rwd_type").GetComponent<Sprite>();
            }
        }

    }
}