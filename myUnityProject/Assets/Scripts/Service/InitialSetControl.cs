using UnityEngine;
using UnityEngine.UI;
using Packet;

namespace Service
{
    public class InitialSetControl : MonoBehaviour
    {

        //public GameObject userSessionObject;
        public UserSession _UserSession;
        public CommonUtil commonUtil;


        public InputField email;
        public InputField pwd;
        public InputField nickname;

        public InputField login_email;
        public InputField login_pwd;

        public GameObject registerPanel;
        public GameObject loginPanel;
//        public GameObject Panel_attend;

        public Text user_gold;
        public Text user_coin;
        public Text user_nickname;

        public GameObject inventory_new_btn;
        public GameObject message_new_btn;
        public GameObject friend_new_btn;


        public MemberControl memberControl;
        public AttendControl attendControl;
        // public CharacterControl characterControl;
        private MemberInfoPacket memberInfoPacket;
        //private CharacterPacket characterPacket;
        private MembeMemberInitialInfoPacket membeMemberInitialInfoPacket;

        private void Start()
        {
            //memberInfoPacket = new MemberInfoPacket();
            //memberService = userSessionObject.GetComponent<MemberService>();
            //characterControl = userSessionObject.GetComponent<CharacterControl>();
            memberControl.HttpObject = _UserSession._HttpObject;
        }

        //Check Login
        public void LoginCheck()
        {
            memberInfoPacket = memberControl.LoginCheck(login_email.text, login_pwd.text);

        
            if (memberInfoPacket.resultCd == 0)   
            {
                //Character 가져옴 
                membeMemberInitialInfoPacket = memberControl.GetUserInitialInfo(memberInfoPacket.account);

                //UserSessionObject에 사용자 정보 set
                SetEssentialInfo();

                //close current login window
                loginPanel.SetActive(false);
            }
            else
            {
                //fail alert window 
                commonUtil.HandleAlert(memberInfoPacket.resultMsg);
            }
        }

        //Todo : 별도 class로 분리하자 
        //Register new Member 
        public void RegisterMember()
        {
            int result_cd = 0;
           

            if (string.IsNullOrEmpty(email.text))
            {
                commonUtil.HandleAlert("이메일을 입력하세요.");
                return;
            }

            if (string.IsNullOrEmpty(pwd.text))
            {
                commonUtil.HandleAlert("비밀번호를 입력하세요.");
                return;
            }

            if (string.IsNullOrEmpty(nickname.text))
            {
                commonUtil.HandleAlert("닉네임을 입력하세요.");
                return;
            }

            memberInfoPacket = memberControl.RegisterMember(email.text, pwd.text, nickname.text);
            result_cd = memberInfoPacket.resultCd;

            if (result_cd == 0)  //register success
            {
                SetEssentialInfo();
                registerPanel.SetActive(false);
            }
            else //register fail
            {
                //show error message
                commonUtil.HandleAlert(memberInfoPacket.resultMsg);
            }
        }


        //Setting Essential Information at UserSessionObject after login or register
        void SetEssentialInfo()
        {
        //Member Info

            _UserSession._UserDetail = memberInfoPacket.userDetail;

        //New Sign Setting
            _UserSession._NoticeNew.new_msg = membeMemberInitialInfoPacket.new_item_flag;
            _UserSession._NoticeNew.new_inventory = membeMemberInitialInfoPacket.new_item_flag;
            _UserSession._NoticeNew.new_friend = membeMemberInitialInfoPacket.new_frd_flag;

        //화면에 보여질 항목 세팅 
            this.user_gold.text = _UserSession._UserDetail.gold.ToString();
            this.user_coin.text = _UserSession._UserDetail.coin.ToString();
            this.user_nickname.text = _UserSession._UserDetail.nickname;

        //대표 캐릭터 정보 세팅  
            _UserSession._UserCharacter = membeMemberInitialInfoPacket.carryUserCharacter;

            if (_UserSession._NoticeNew.new_inventory.Equals("Y"))
                inventory_new_btn.SetActive(true);
            if (_UserSession._NoticeNew.new_msg.Equals("Y")) 
                message_new_btn.SetActive(true);
            if (_UserSession._NoticeNew.new_friend.Equals("Y")) 
                friend_new_btn.SetActive(true);

            //출석체크 창 열기  
            if (membeMemberInitialInfoPacket.attend_show_flag.Equals("Y"))
            {
                //Panel_attend.SetActive(true);
                //attendControl.httpSock = _UserSession._HttpObject;
                attendControl.GetUserAttendList(_UserSession._UserDetail.account);
            }
               
            _UserSession.Panel_lobby.SetActive(true);
        }
    }
}