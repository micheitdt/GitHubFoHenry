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
    public class Format2
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
            catch (Exception err)
            {
                _logger.Error(err, string.Format("Format2(): ErrMsg = {0}.", err.Message));
            }
        }
    }
}
