using UnityEngine;
using Packet;

namespace Service
{
    public class CharacterControl : MonoBehaviour
    {
        public HttpSock httpSock;
        public MemberService memberService;
        public CommonUtil commonUtil;

        public CharacterPacket characterPacket;
        
        public void GetCharacterList()
        {
            string json = httpSock.Connect("selectCharacterList.do", "user_account=" + memberService._MemberInfoPacket.account);

            characterPacket = JsonUtility.FromJson<CharacterPacket>(json);

            //구현부분 필요에 맞게 바꿀 것 
           
            if (characterPacket.resultCd == 0)
            {   
                Debug.Log("characterPacket:" + characterPacket.userCharacterList.Count + "개의 캐릭터가 있습니다.");
                            
            }
            else
            {
                //fail alert window 
                commonUtil.HandleAlert(characterPacket.resultMsg);
            }
        }
    }
}