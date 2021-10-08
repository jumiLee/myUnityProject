using UnityEngine;
using UnityEngine.UI;
using Packet;

namespace Service
{
    public class InitialSetControl : MonoBehaviour
    {

        public GameObject userSessionObject;
        public CommonUtil commonUtil;


        public InputField email;
        public InputField pwd;
        public InputField nickname;

        public GameObject registerPanel;
        public GameObject loginPanel;

        public Text user_gold;
        public Text user_coin;
        public Text user_nickname;

        public GameObject inventory_new_btn;
        public GameObject message_new_btn;
        public GameObject friend_new_btn;
        public GameObject Panel_attend;

        public MemberService memberService;
        public CharacterControl characterControl;

        private MemberInfoPacket memberInfoPacket;
        private CharacterPacket characterPacket;
        private MembeMemberInitialInfoPacket membeMemberInitialInfoPacket;

        private void Start()
        {
            memberService = userSessionObject.GetComponent<MemberService>();
            characterControl = userSessionObject.GetComponent<CharacterControl>();
        }

        //Check Login
        public void LoginCheck()
        {
            memberInfoPacket = memberService.LoginCheck(email.text, pwd.text);

        
            if (memberInfoPacket.resultCd == 0)   
            {
                //출석체크 등록 


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

            memberInfoPacket = memberService.RegisterMember(email.text, pwd.text, nickname.text);
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
            memberService._MemberInfoPacket = memberInfoPacket;

            this.user_gold.text = memberInfoPacket.userDetail.gold.ToString();
            this.user_coin.text = memberInfoPacket.userDetail.coin.ToString();
            this.user_nickname.text = memberInfoPacket.userDetail.nickname;

            membeMemberInitialInfoPacket = memberService.GetUserInitialInfo(memberInfoPacket.account);

        //New Sign Setting
            if (membeMemberInitialInfoPacket.new_item_flag.Equals("Y")) 
                inventory_new_btn.SetActive(true);
            if (membeMemberInitialInfoPacket.new_msg_flag.Equals("Y")) 
                message_new_btn.SetActive(true);
            if (membeMemberInitialInfoPacket.new_frd_flag.Equals("Y")) 
                friend_new_btn.SetActive(true);

        //대표 캐릭터 정보 세팅  
            characterControl._CharacterPacket.carryUserCharacter = membeMemberInitialInfoPacket.carryUserCharacter;

        //출석체크 창 열기  
            if (membeMemberInitialInfoPacket.attend_show_flag.Equals("Y"))
                Panel_attend.SetActive(true);
        }
    }
}