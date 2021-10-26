using System;

namespace Entity
{
    [Serializable]
    public class UserReceiveBox
    {
        public int user_account;
        public int receive_sn;
        public int sender_account;
        public int rwd_type;
        public int rwd_id;
        public int rwd_sub_id;
        public string issue_dt;
        public string receive_msg;
        public string check_dt;
        public string check_flag;
        public string sender_nickname;
        public string rwd_nm;
    }
}