using UnityEngine;
using UnityEngine.UI;
using Packet;
using Entity;

namespace Service
{
    //related unity object : CharacterControlObject 
    public class CharacterControl : MonoBehaviour
    {
        public UserSession userSession;
        public CharacterCustInfo char_cust_info; //mapping character customizing info
        public CharacterPacket characterPacket;
        public CommonUtil commonUtil;

        public CharacterPacket GetCharacterList(int user_account)
        {
            string json = userSession._HttpObject.Connect("selectCharacterList.do", 
                                                          "user_account=" + userSession._UserCharacter.user_account);

            return JsonUtility.FromJson<CharacterPacket>(json);
        }
        //update character customizing info
        public void ModifyUserCharacter()
        {
            string char_cust_info_json = JsonUtility.ToJson(char_cust_info);
           
            string json = userSession._HttpObject.Connect("modifyUserCharacter.do", 
                                                          "user_account=" + userSession._UserCharacter.user_account
                                                        + "&char_id=" + userSession._UserCharacter.char_id
                                                        + "&user_char_sn=" + userSession._UserCharacter.user_char_sn
                                                        + "&char_cust_info_json=" + char_cust_info_json);

            characterPacket = JsonUtility.FromJson<CharacterPacket>(json);
            //Generate item list
            if (characterPacket.resultCd == 0)
            {
                userSession._UserCharacter = characterPacket.carryUserCharacter;
                commonUtil.HandleAlert("수정되었습니다.");
            }
            else
            {
                commonUtil.HandleAlert(characterPacket.resultMsg);
            }
        }

        //TODO : UI 결정되면 수정 (아마 버튼 생성 시 함수를 동적으로 생성할 것이기 때문에 값 세팅 필요없이 선택여부만 판단하면 될 것임 ..
        //Mapping Character Custom Info on UI
        public void SetEye(Text value)
        {
            this.char_cust_info.eye = int.Parse(value.text);
        }
        //TODO
        public void SetHair(Text value)
        {
            this.char_cust_info.hair = int.Parse(value.text);
        }
        //TODO
        public void SetNose(Text value)
        {
            this.char_cust_info.nose = int.Parse(value.text);
        }
        //TODO
        public void SetMouth(Text value)
        {
            this.char_cust_info.mouth = int.Parse(value.text);
        }
    }
}