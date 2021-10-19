using System;

namespace Entity
{
    [Serializable]
    public class UserPayment
    {
        public int user_account;
        public int trade_no;
        public string prod_id;
        public int trade_amt;
        public string trade_req_dt;
        public string trade_key;
        public int trade_res_cd;
        public string trade_res_mgs;
        public string trade_res_dt;
        public string trade_res_key;
        public string order_info;
        public int device_type;
        public int payment_type;
        public int ch_type;
        public string ch_id;
        public int req_time_diff; //결제요청부터 현재까 시간 
        public int accept_itval;  //req_time_diff 허용 시간(초)
    }
}