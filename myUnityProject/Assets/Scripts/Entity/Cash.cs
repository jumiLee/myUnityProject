using System;

namespace Entity
{
    [Serializable]
    public class Cash
    {
        public int cash_id;
        public string item_nm;
        public int device_type;
        public int payment_type;
        public string prod_id;
        public int cash_type_cd;    //1:$, 2:won
        public int cash_amt;
        public int cash_dis_amt;
        public int coin_amt;
        public string coin_nm;
        public string coin_desc;
        public string del_flag;
        public string display_flag;
        public int prod_img;
    }

}