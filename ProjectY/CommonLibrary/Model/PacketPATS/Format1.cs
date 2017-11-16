using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonLibrary.Model.PacketPATS
{
    /// <summary>
    /// 行情資料格式
    /// </summary>
    [Serializable]
    public class Format1
    {
        #region Logger
        /// <summary>
        /// 記錄器
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion Logger
        public string ExchangeNo { get; set; }
        public string CommodityNo { get; set; }
        public string ContractDate { get; set; }
        public string LastTime { get; set; }
        public string BidTime { get; set; }
        public double LastPrice{ get; set; }
        public int TotalVolume { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double BidPrice0 { get; set; }
        public int BidVolume0 { get; set; }
        public double OfficePrice0 { get; set; }
        public int OfficeVolume0 { get; set; }
        public double BidPrice1 { get; set; }
        public int BidVolume1 { get; set; }
        public double OfficePrice1 { get; set; }
        public int OfficeVolume1 { get; set; }
        public double BidPrice2 { get; set; }
        public int BidVolume2 { get; set; }
        public double OfficePrice2 { get; set; }
        public int OfficeVolume2 { get; set; }
        public double BidPrice3 { get; set; }
        public int BidVolume3 { get; set; }
        public double OfficePrice3 { get; set; }
        public int OfficeVolume3 { get; set; }
        public double BidPrice4 { get; set; }
        public int BidVolume4 { get; set; }
        public double OfficePrice4 { get; set; }
        public int OfficeVolume4 { get; set; }
        public double BidPrice5 { get; set; }
        public int BidVolume5 { get; set; }
        public double OfficePrice5 { get; set; }
        public int OfficeVolume5 { get; set; }
        public double BidPrice6 { get; set; }
        public int BidVolume6 { get; set; }
        public double OfficePrice6 { get; set; }
        public int OfficeVolume6 { get; set; }
        public double BidPrice7 { get; set; }
        public int BidVolume7 { get; set; }
        public double OfficePrice7 { get; set; }
        public int OfficeVolume7 { get; set; }
        public double BidPrice8 { get; set; }
        public int BidVolume8 { get; set; }
        public double OfficePrice8 { get; set; }
        public int OfficeVolume8 { get; set; }
        public double BidPrice9 { get; set; }
        public int BidVolume9 { get; set; }
        public double OfficePrice9 { get; set; }
        public int OfficeVolume9 { get; set; }
        
        public Format1(byte[] stream, string symbol)
        {
            try
            {
                var data = symbol.Split('.');
                ExchangeNo = data[0];
                CommodityNo = data[1];
                ContractDate = data[2];
                LastTime = string.Format("{0}:{1}:{2}", stream[0], stream[1], stream[2]);
                BidTime = string.Format("{0}:{1}:{2}", stream[3], stream[4], stream[5]);
                LastPrice = BitConverter.ToDouble(stream, 6);
                TotalVolume = BitConverter.ToInt32(stream, 14);
                HighPrice = BitConverter.ToDouble(stream, 18);
                LowPrice = BitConverter.ToDouble(stream, 26);
                BidPrice0 = BitConverter.ToDouble(stream, 34);
                BidVolume0 = BitConverter.ToInt32(stream, 42);
                OfficePrice0 = BitConverter.ToDouble(stream, 46);
                OfficeVolume0 = BitConverter.ToInt32(stream, 54);
                BidPrice1 = BitConverter.ToDouble(stream, 58);
                BidVolume1 = BitConverter.ToInt32(stream, 66);
                OfficePrice1 = BitConverter.ToDouble(stream, 70);
                OfficeVolume1 = BitConverter.ToInt32(stream, 78);
                BidPrice2 = BitConverter.ToDouble(stream, 82);
                BidVolume2 = BitConverter.ToInt32(stream, 90);
                OfficePrice2 = BitConverter.ToDouble(stream, 94);
                OfficeVolume2 = BitConverter.ToInt32(stream, 102);
                BidPrice3 = BitConverter.ToDouble(stream, 106);
                BidVolume3 = BitConverter.ToInt32(stream, 114);
                OfficePrice3 = BitConverter.ToDouble(stream, 118);
                OfficeVolume3 = BitConverter.ToInt32(stream, 126);
                BidPrice4 = BitConverter.ToDouble(stream, 130);
                BidVolume4 = BitConverter.ToInt32(stream, 138);
                OfficePrice4 = BitConverter.ToDouble(stream, 142);
                OfficeVolume4 = BitConverter.ToInt32(stream, 150);
                BidPrice5 = BitConverter.ToDouble(stream, 154);
                BidVolume5 = BitConverter.ToInt32(stream, 162);
                OfficePrice5 = BitConverter.ToDouble(stream, 166);
                OfficeVolume5 = BitConverter.ToInt32(stream, 174);
                BidPrice6 = BitConverter.ToDouble(stream, 178);
                BidVolume6 = BitConverter.ToInt32(stream, 186);
                OfficePrice6 = BitConverter.ToDouble(stream, 190);
                OfficeVolume6 = BitConverter.ToInt32(stream, 198);
                BidPrice7 = BitConverter.ToDouble(stream, 202);
                BidVolume7 = BitConverter.ToInt32(stream, 210);
                OfficePrice7 = BitConverter.ToDouble(stream, 214);
                OfficeVolume7 = BitConverter.ToInt32(stream, 222);
                BidPrice8 = BitConverter.ToDouble(stream, 226);
                BidVolume8 = BitConverter.ToInt32(stream, 234);
                OfficePrice8 = BitConverter.ToDouble(stream, 238);
                OfficeVolume8 = BitConverter.ToInt32(stream, 246);
                BidPrice9 = BitConverter.ToDouble(stream, 250);
                BidVolume9 = BitConverter.ToInt32(stream, 258);
                OfficePrice9 = BitConverter.ToDouble(stream, 262);
                OfficeVolume9 = BitConverter.ToInt32(stream, 270);
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("Format1(): ErrMsg = {0}.", err.Message));
            }
        }
    }
}
