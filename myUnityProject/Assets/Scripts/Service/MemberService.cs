using UnityEngine;
using Packet;

namespace Service{

    public class MemberService : MonoBehaviour
    { 
        public HttpSock HttpObject;

        public MemberInfoPacket _MemberInfoPacket;  //sessionObject에 들고 다닐 정보

        private void Start()
        {
            HttpObject = gameObject.GetComponent<HttpSock>();
        }

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