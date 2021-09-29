using UnityEngine;
using Packet;

namespace Service{

    public class MemberService : MonoBehaviour
    { 
        public HttpSock HttpObject;

        public MemberInfoPacket _MemberInfoPacket;  //session에 들고 다닐 정보

        private void Start()
        {
            HttpObject = gameObject.GetComponent<HttpSock>();
        }
        /*
        public Member MemberObject;
        public void SelectMember()
        {

            string json = HttpObject.Connect("selectMember.do", "email=" + email.text + "&pwd=" + pwd.text);
            
            MemberObject = JsonUtility.FromJson<Member>(json);
        }
        */

        //login check -> return member info.
        public MemberInfoPacket LoginCheck(string email, string pwd)
        {
            string json = HttpObject.Connect("loginChk.do", "email=" + email + "&pwd=" + pwd);

            return JsonUtility.FromJson<MemberInfoPacket>(json);

        }

        public MemberInfoPacket RegisterMember(string email, string pwd, string nickname)
        {
            string json = HttpObject.Connect("register.do", "email=" + email + "&pwd=" + pwd + "&nickname=" + nickname);

            return JsonUtility.FromJson<MemberInfoPacket>(json);
        }
    }
}