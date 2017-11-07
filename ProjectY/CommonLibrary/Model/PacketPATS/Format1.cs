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
        public string Time { get; set; }
        public string ExchangeNo { get; set; }
        public string CommodityNo { get; set; }
        public string ContractDate { get; set; }
        public string LastPrice{ get; set; }
        public int LastVolume{ get; set; }
        public int TotalVolume { get; set; }
        public string HighPrice{ get; set; }
        public string LowPrice{ get; set; }
        public List<PriceVolumeS> BidData { get; set; }
        public List<PriceVolumeS> AskData { get; set; }

        public Format1(byte[] stream)
        {
            try
            {
                PATStoProxyFormat priceData = PriceStructFromBytes(stream);
                Time = string.Format("{0}:{1}:{2}", priceData.Last.Hour, priceData.Last.Minute, priceData.Last.Second);
                ExchangeNo = priceData.SymbolInfo.ExchangeName;
                CommodityNo = priceData.SymbolInfo.CommodityName;
                ContractDate = priceData.SymbolInfo.ContractDate;
                LastPrice = priceData.Last.Price;
                LastVolume = priceData.Last.Volume;
                TotalVolume = priceData.Total.Volume;
                HighPrice = priceData.High.Price;
                LowPrice = priceData.Low.Price;
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM0.Price, Volume = priceData.BidDOM0.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM1.Price, Volume = priceData.BidDOM1.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM2.Price, Volume = priceData.BidDOM2.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM3.Price, Volume = priceData.BidDOM3.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM4.Price, Volume = priceData.BidDOM4.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM5.Price, Volume = priceData.BidDOM5.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM6.Price, Volume = priceData.BidDOM6.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM7.Price, Volume = priceData.BidDOM7.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM8.Price, Volume = priceData.BidDOM8.Volume });
                BidData.Add(new PriceVolumeS() { Price = priceData.BidDOM9.Price, Volume = priceData.BidDOM9.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM0.Price, Volume = priceData.OfferDOM0.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM1.Price, Volume = priceData.OfferDOM1.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM2.Price, Volume = priceData.OfferDOM2.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM3.Price, Volume = priceData.OfferDOM3.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM4.Price, Volume = priceData.OfferDOM4.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM5.Price, Volume = priceData.OfferDOM5.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM6.Price, Volume = priceData.OfferDOM6.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM7.Price, Volume = priceData.OfferDOM7.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM8.Price, Volume = priceData.OfferDOM8.Volume });
                AskData.Add(new PriceVolumeS() { Price = priceData.OfferDOM9.Price, Volume = priceData.OfferDOM9.Volume });
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("Format1(): ErrMsg = {0}.", err.Message));
            }
        }
        
        private PATStoProxyFormat PriceStructFromBytes(byte[] arr)
        {
            PATStoProxyFormat str = new PATStoProxyFormat();

            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            str = (PATStoProxyFormat)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }

        private string PriceDetailStructToString(PriceDetailStruct price)
        {
            return string.Format("Price{0};Volume{0};AgeCounter{0};Direction{0};Time{0}:{0}:{0}", price.Price, price.Volume, price.AgeCounter, price.Direction, price.Hour, price.Minute, price.Second);
        }
    }
}
