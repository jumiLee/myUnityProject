using System;

namespace Entity
{
    [Serializable]
    public class UserDetail
    {
        public int account;     //사용자 계정
        public string nickname; //닉네임
        public int point;       //게임머니 
        public int coin;        
        public int gold;       
    }
}