using System;

namespace Entity
{
    [Serializable]
    public class UserCharEquipItem : Item
    {
        public int item_uniqueID;
        public int user_account;
        public int char_id;
        public int user_char_sn;
        public int equip_yn;
    }
}