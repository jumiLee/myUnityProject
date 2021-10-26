using System;
using Entity;
using System.Collections.Generic; 

namespace Packet
{
    [Serializable]
    public class ItemPacket : HeaderPacket
    {
        public string sid;
        public List<Item> itemList;
    }
}