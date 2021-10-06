using System;
using Entity;
using System.Collections.Generic;

namespace Packet
{

    [Serializable]
    public class UserCharEquipItemPacket : HeaderPacket
    {
        public string sid;
        public List<UserCharEquipItem> userCharEquipItemList;  
    }
}