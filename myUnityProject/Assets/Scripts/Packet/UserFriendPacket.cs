using System;
using Entity;
using System.Collections.Generic;

namespace Packet
{

    [Serializable]
    public class UserFriendPacket : HeaderPacket
    {
        public string sid;
        public List<UserFriend> userFriendList;
    }
}