using System;
using Entity;
using System.Collections.Generic;

namespace Packet
{
    [Serializable]
    public class ResultPacket : HeaderPacket
    {
        public string sid;
    }
}