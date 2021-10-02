using UnityEngine;
using Packet;

namespace Service
{
    public class CharacterControl : MonoBehaviour
    {
        public HttpSock HttpObject; 

        public CharacterPacket _CharacterPacket; //sessionObject에 들고 다닐 정보

        private void Start()
        {
            HttpObject = gameObject.GetComponent<HttpSock>();
        }

        public CharacterPacket GetCharacterList(int user_account)
        {
            string json = HttpObject.Connect("selectCharacterList.do", "user_account=" + user_account);

            return JsonUtility.FromJson<CharacterPacket>(json);
        }
    }
}