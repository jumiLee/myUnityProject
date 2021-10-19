using System;
using Entity;
using System.Collections.Generic;

namespace Packet
{
    [Serializable]
    public class ResultPaymentPacket : HeaderPacket
    {
        public string sid;
        public int tradeNo;
        public string appId;
        public string orderNo;
        public int trade_res_cd;
        public string trade_res_msg;
        public string trade_res_key;
    }
}