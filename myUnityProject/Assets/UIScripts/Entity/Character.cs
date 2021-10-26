using System;

namespace Entity
{
    [Serializable]
    public class Character 
    {
        public int char_id;
        public string char_nm;
        public int char_type;
        public int char_max_lv;
        public int char_lv;
        public string char_desc;
        public string del_flag;
        public DateTime del_dt;
        public string char_shape_info;
    }
}

