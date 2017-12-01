using System;

namespace MarketDataApi.PacketPATS
{
    /// <summary>
    /// 行情資料格式
    /// </summary>
    public class Format1
    {
        public string ExchangeNo { get; set; }
        public string CommodityNo { get; set; }
        public string ContractDate { get; set; }
        public string Time { get; set; }
        public int TotalVolume { get; set; }
        public double HighPrice { get; set; }
        public double LowPrice { get; set; }
        public double OpeningPrice { get; set; }
        public double CloseingPrice { get; set; }
        public double BidDOMPrice0 { get; set; }
        public int BidDOMVolume0 { get; set; }
        public double OfferDOMPrice0 { get; set; }
        public int OfferDOMVolume0 { get; set; }
        public double BidDOMPrice1 { get; set; }
        public int BidDOMVolume1 { get; set; }
        public double OfferDOMPrice1 { get; set; }
        public int OfferDOMVolume1 { get; set; }
        public double BidDOMPrice2 { get; set; }
        public int BidDOMVolume2 { get; set; }
        public double OfferDOMPrice2 { get; set; }
        public int OfferDOMVolume2 { get; set; }
        public double BidDOMPrice3 { get; set; }
        public int BidDOMVolume3 { get; set; }
        public double OfferDOMPrice3 { get; set; }
        public int OfferDOMVolume3 { get; set; }
        public double BidDOMPrice4 { get; set; }
        public int BidDOMVolume4 { get; set; }
        public double OfferDOMPrice4 { get; set; }
        public int OfferDOMVolume4 { get; set; }
        public double BidDOMPrice5 { get; set; }
        public int BidDOMVolume5 { get; set; }
        public double OfferDOMPrice5 { get; set; }
        public int OfferDOMVolume5 { get; set; }
        public double BidDOMPrice6 { get; set; }
        public int BidDOMVolume6 { get; set; }
        public double OfferDOMPrice6 { get; set; }
        public int OfferDOMVolume6 { get; set; }
        public double BidDOMPrice7 { get; set; }
        public int BidDOMVolume7 { get; set; }
        public double OfferDOMPrice7 { get; set; }
        public int OfferDOMVolume7 { get; set; }
        public double BidDOMPrice8 { get; set; }
        public int BidDOMVolume8 { get; set; }
        public double OfferDOMPrice8 { get; set; }
        public int OfferDOMVolume8 { get; set; }
        public double BidDOMPrice9 { get; set; }
        public int BidDOMVolume9 { get; set; }
        public double OfferDOMPrice9 { get; set; }
        public int OfferDOMVolume9 { get; set; }
        public double ReferencePrice { get; set; }

        public Format1(byte[] stream, string symbol)
        {
            try
            {
                var data = symbol.Split('.');
                ExchangeNo = data[0];
                CommodityNo = data[1];
                ContractDate = data[2];
                Time = string.Format("{0}:{1}:{2}", stream[0], stream[1], stream[2]);
                TotalVolume = BitConverter.ToInt32(stream, 3);
                HighPrice = BitConverter.ToDouble(stream, 7);
                LowPrice = BitConverter.ToDouble(stream, 15);
                OpeningPrice = BitConverter.ToDouble(stream, 23);
                CloseingPrice = BitConverter.ToDouble(stream, 31);
                BidDOMPrice0 = BitConverter.ToDouble(stream, 39);
                BidDOMVolume0 = BitConverter.ToInt32(stream, 47);
                OfferDOMPrice0 = BitConverter.ToDouble(stream, 51);
                OfferDOMVolume0 = BitConverter.ToInt32(stream, 59);
                BidDOMPrice1 = BitConverter.ToDouble(stream, 63);
                BidDOMVolume1 = BitConverter.ToInt32(stream, 71);
                OfferDOMPrice1 = BitConverter.ToDouble(stream, 75);
                OfferDOMVolume1 = BitConverter.ToInt32(stream, 83);
                BidDOMPrice2 = BitConverter.ToDouble(stream, 87);
                BidDOMVolume2 = BitConverter.ToInt32(stream, 95);
                OfferDOMPrice2 = BitConverter.ToDouble(stream, 99);
                OfferDOMVolume2 = BitConverter.ToInt32(stream, 107);
                BidDOMPrice3 = BitConverter.ToDouble(stream, 111);
                BidDOMVolume3 = BitConverter.ToInt32(stream, 119);
                OfferDOMPrice3 = BitConverter.ToDouble(stream, 123);
                OfferDOMVolume3 = BitConverter.ToInt32(stream, 131);
                BidDOMPrice4 = BitConverter.ToDouble(stream, 135);
                BidDOMVolume4 = BitConverter.ToInt32(stream, 143);
                OfferDOMPrice4 = BitConverter.ToDouble(stream, 147);
                OfferDOMVolume4 = BitConverter.ToInt32(stream, 155);
                BidDOMPrice5 = BitConverter.ToDouble(stream, 159);
                BidDOMVolume5 = BitConverter.ToInt32(stream, 167);
                OfferDOMPrice5 = BitConverter.ToDouble(stream, 171);
                OfferDOMVolume5 = BitConverter.ToInt32(stream, 179);
                BidDOMPrice6 = BitConverter.ToDouble(stream, 183);
                BidDOMVolume6 = BitConverter.ToInt32(stream, 191);
                OfferDOMPrice6 = BitConverter.ToDouble(stream, 195);
                OfferDOMVolume6 = BitConverter.ToInt32(stream, 203);
                BidDOMPrice7 = BitConverter.ToDouble(stream, 207);
                BidDOMVolume7 = BitConverter.ToInt32(stream, 215);
                OfferDOMPrice7 = BitConverter.ToDouble(stream, 219);
                OfferDOMVolume7 = BitConverter.ToInt32(stream, 227);
                BidDOMPrice8 = BitConverter.ToDouble(stream, 231);
                BidDOMVolume8 = BitConverter.ToInt32(stream, 239);
                OfferDOMPrice8 = BitConverter.ToDouble(stream, 243);
                OfferDOMVolume8 = BitConverter.ToInt32(stream, 251);
                BidDOMPrice9 = BitConverter.ToDouble(stream, 255);
                BidDOMVolume9 = BitConverter.ToInt32(stream, 263);
                OfferDOMPrice9 = BitConverter.ToDouble(stream, 267);
                OfferDOMVolume9 = BitConverter.ToInt32(stream, 275);
                ReferencePrice = BitConverter.ToDouble(stream, 279);
            }
            catch (Exception )
            {
            }
        }
    }
}
