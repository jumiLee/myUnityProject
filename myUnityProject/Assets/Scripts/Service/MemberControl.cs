using UnityEngine;
using UnityEngine.UI;
using Packet;

namespace Service
{
    public class MemberControl : MonoBehaviour
    {

        public GameObject userSessionObject;
        public CommonUtil commonUtil;


        public InputField email;
        public InputField pwd;
        public InputField nickname;

        public GameObject registerPanel;
        public GameObject loginPanel;


        public MemberService memberService;
        private MemberInfoPacket memberInfoPacket;

        private void Start()
        {
            memberService = userSessionObject.GetComponent<MemberService>();
        }

        public void LoginCheck()
        {
            memberInfoPacket = memberService.LoginCheck(email.text, pwd.text);

        
            if (memberInfoPacket.resultCd == 0)   
            {
                //UserSessionObject에 사용자 정보 set
                memberService._MemberInfoPacket = memberInfoPacket;
                //close current login window
                loginPanel.SetActive(false);
            }
            else
            {
                //fail alert window 
                commonUtil.HandleAlert(memberInfoPacket.resultMsg);
            }
        }

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
                registerPanel.SetActive(false);
                //UserSessionObject에 사용자 정보 set
                memberService._MemberInfoPacket = memberInfoPacket;
            }
            else //register fail
            {
                //show error message
                commonUtil.HandleAlert(memberInfoPacket.resultMsg);
            }
        }
    }
}