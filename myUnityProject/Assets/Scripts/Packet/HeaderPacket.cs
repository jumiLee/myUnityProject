using System;
using UnityEngine;

namespace Packet
{
    [Serializable]
    public class HeaderPacket 
    {
        public int resultCd;       //결과코드
        public string resultMsg;    //결과 메세지
        public int account;
    }
}