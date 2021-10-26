using System;
using System.Collections.Generic;

namespace Entity {
    [Serializable]
    public class UserCharacter : Character
    {
        public int user_account;
        public int user_char_sn;
        public int user_char_lv;
        public int user_char_exp;
        public int carry_flag;
        //public string char_cust_info;
        public CharacterCustInfo char_cust_info;
        public List<UserCharEquipItem> char_equip_items; //현재 장착 아이템 정보 
        public DateTime create_dt;
        public DateTime last_mod_dt;
    }
}