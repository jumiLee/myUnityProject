using System;
using Entity;

namespace Packet
{
    [Serializable]
    public class MembeMemberInitialInfoPacket : HeaderPacket
    {
        public string sid;
        public UserCharacter carryUserCharacter;    //대표캐릭터 
        public string new_msg_flag;
        public string new_item_flag;
        public string new_achv_flag;
        public string new_frd_flag;
        public string attend_show_flag; //Y:출석체크화면 보여줌 N:인보여줌 
    }
}