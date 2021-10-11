using System;

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
        public DateTime create_dt;
        public DateTime last_mod_dt;
    }
}