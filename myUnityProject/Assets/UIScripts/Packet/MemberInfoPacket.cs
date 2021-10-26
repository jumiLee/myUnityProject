using System;
using Entity;

namespace Packet
{

    [Serializable]
    public class MemberInfoPacket : HeaderPacket
    {
        public string sid;
        public UserDetail userDetail;   //user detail info    
    }
}