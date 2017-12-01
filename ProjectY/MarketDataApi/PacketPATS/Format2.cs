using System;

namespace MarketDataApi.PacketPATS
{
    /// <summary>
    /// 行情資料格式
    /// </summary>
    public class Format2
    {
        public string ExchangeNo { get; set; }
        public string CommodityNo { get; set; }
        public string ContractDate { get; set; }
        public double LastPrice{ get; set; }
        public int LastVolume { get; set; }

        public Format2(byte[] stream, string symbol)
        {
            try
            {
                var data = symbol.Split('.');
                ExchangeNo = data[0];
                CommodityNo = data[1];
                ContractDate = data[2];
                LastPrice = BitConverter.ToDouble(stream, 0);
                LastVolume = BitConverter.ToInt32(stream, 8);
            }
            catch (Exception)
            {
            }
        }
    }
}
