using System;

namespace Entity
{
    [Serializable]
    public class Item
    {
        public int item_id;
        public string item_nm;
        public int item_category;
        public int item_type;
        public int item_price;
        public int unit_cd;
        public string item_desc;
        public int item_value;
        public int item_cnt;
        public int rare_degree;
        public int item_period_flag;
        public int item_period;
        public int item_dup_flag;
        public int item_new_flag;
        public int item_order;
        public int item_img_no;
        public int item_equip_flag;
        public int item_grade;
        public int use_flag;
        public string create_dt;
        public string del_dt;
    }

}