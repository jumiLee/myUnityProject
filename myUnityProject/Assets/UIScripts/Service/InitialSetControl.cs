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

        public Text user_gold;
        public Text user_coin;
        public Text user_nickname;

        public GameObject inventory_new_btn;
        public GameObject message_new_btn;
        public GameObject friend_new_btn;


        public MemberControl memberControl;
        public AttendControl attendControl;
        private ResultPacket resultPacket;
        private MembeMemberInitialInfoPacket membeMemberInitialInfoPacket;

        private void Start()
        {
            memberControl.HttpObject = _UserSession._HttpObject;
        }

        //Check Login
        public void LoginCheck()
        {
            resultPacket = memberControl.LoginCheck(login_email.text, login_pwd.text);

        
            if (resultPacket.resultCd == 0)   
            {
                //Character 가져옴 
                //membeMemberInitialInfoPacket = memberControl.GetUserInitialInfo(memberInfoPacket.account);

                //UserSessionObject에 사용자 정보 set
                SetEssentialInfo();

                //close current login window
                loginPanel.SetActive(false);
            }
            else
            {
                //fail alert window 
                commonUtil.HandleAlert(resultPacket.resultMsg);
            }
        }

        //Todo : 별도 class로 분리하자 
        //Register new Member 
        public void RegisterMember()
        {
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

            resultPacket = memberControl.RegisterMember(email.text, pwd.text, nickname.text);

            if (resultPacket.resultCd == 0)  //register success
            {
                //membeMemberInitialInfoPacket = memberControl.GetUserInitialInfo(memberInfoPacket.account);

                //UserSessionObject에 사용자 정보 set
                SetEssentialInfo();
                registerPanel.SetActive(false);
            }
            else //register fail
            { 
                //show error message
                commonUtil.HandleAlert(resultPacket.resultMsg);
            }
        }


        //Setting Essential Information at UserSessionObject after login or register
        public void SetEssentialInfo()
        {
            membeMemberInitialInfoPacket = memberControl.GetUserInitialInfo(resultPacket.account);

            //Member Info
            _UserSession._UserDetail = membeMemberInitialInfoPacket.userDetail;

        //New Sign Setting
            _UserSession._NoticeNew.new_msg = membeMemberInitialInfoPacket.new_msg_flag;
            _UserSession._NoticeNew.new_inventory = membeMemberInitialInfoPacket.new_item_flag;
            _UserSession._NoticeNew.new_friend = membeMemberInitialInfoPacket.new_frd_flag;

            if (_UserSession._NoticeNew.new_inventory.Equals("N"))  //check_flag=N 이면, new icon 보여줌 
            {
                inventory_new_btn.SetActive(true);
            }
            else
            {
                inventory_new_btn.SetActive(false);
            }
                
            if (_UserSession._NoticeNew.new_msg.Equals("N")) { 
                message_new_btn.SetActive(true);
            }
            else
            {
                message_new_btn.SetActive(false);
            }

            if (_UserSession._NoticeNew.new_friend.Equals("N"))
            {
                friend_new_btn.SetActive(true);
            }
            else
            {
                friend_new_btn.SetActive(false);
            }
                
            //화면에 보여질 항목 세팅 
            this.user_gold.text = _UserSession._UserDetail.gold.ToString();
            this.user_coin.text = _UserSession._UserDetail.coin.ToString();
            this.user_nickname.text = _UserSession._UserDetail.nickname;

        //대표 캐릭터 정보 세팅  
            _UserSession._UserCharacter = membeMemberInitialInfoPacket.carryUserCharacter;

            //출석체크 창 열기  
            if (membeMemberInitialInfoPacket.attend_show_flag.Equals("Y"))
            {
                attendControl.GetUserAttendList(_UserSession._UserDetail.account);
            }
               
            _UserSession.Panel_lobby.SetActive(true);
        }
    }
}