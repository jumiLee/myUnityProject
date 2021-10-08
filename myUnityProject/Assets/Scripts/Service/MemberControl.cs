using UnityEngine;
using Packet;

namespace Service
{

    public class MemberControl : MonoBehaviour
    {
        public HttpSock HttpObject;

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

        //로그인 후 필요한 정보를 조회 
        public MembeMemberInitialInfoPacket GetUserInitialInfo(int user_account)
        {

            string json = HttpObject.Connect("getUserInitialInfo.do", "user_account=" + user_account);

            return JsonUtility.FromJson<MembeMemberInitialInfoPacket>(json);
        }

    }
}