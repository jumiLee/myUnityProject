using System;
using Entity;
using System.Collections.Generic;

namespace Packet
{
    [Serializable]
    public class CashPacket : HeaderPacket
    {
        public string sid;
        public List<Cash> cashList;
    }
}