using System;

namespace Entity {
    [Serializable]
    public class UserFriend 
    {
        public int user_account;
        public int frd_account;
        public int frd_status_cd;
        public string frd_nickname;
        public int frd_friend_cnt;
        public int frd_following_cnt;
    }
}