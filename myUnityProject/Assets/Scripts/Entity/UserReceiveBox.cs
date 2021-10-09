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
        public DateTime issue_dt;
        public string receive_msg;
        public DateTime check_dt;
        public int check_flag;
        public string sender_nickname;
        public string rwd_nm;
    }
}