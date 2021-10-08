using System;
using Entity;
using System.Collections.Generic;

namespace Packet
{
    [Serializable]
    public class CharacterPacket : HeaderPacket
    {
        public string sid; 
        //public List<UserCharacter> userCharacterList;
        public UserCharacter carryUserCharacter;
    }
}