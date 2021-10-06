using System;
using Entity;
using System.Collections.Generic;

namespace Packet
{
    [Serializable]
    public class UserAttendPacket : HeaderPacket
    {
        public string sid;
        public List<UserAttend> userAttendList;
    }
}