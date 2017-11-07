using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLibrary.Model.PacketTPEX
{
    [Serializable]
    public class Format1
    {
        #region Logger
        /// <summary>
        /// 記錄器
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion Logger
        public Header Header;
        public string StockID { get; set; }//股票代號
        public string StockName { get; set; }//股票簡稱
        public string IndCode { get; set; }// 產業別
                                           //02食品工業 03塑膠工業 04紡織纖維 05電機械 06電器纜 08玻璃陶瓷 10鋼鐵工業 11橡膠工業 
                                           //14建材營造15航運業 16觀光事業 17金融業 18貿易百貨 20 其他 21化學工業 22 生技醫療 
                                           //23 油電燃氣業 24 半導體業 25 電腦及週邊設備業 26 光電業 27 通信網路業 28 電子零組件業 
                                           //29 電子通路業 30 資訊服務業 31 其他電子業 32文化創意業 33農業科技 80管理股票
        public string StkCode { get; set; }// 証券別
                                           //W1 認購權證、等比例發行(公開發行時原始轉換標的股數為1000)
                                           //W2 認購權證、不等比例發行(公開發行時原始轉換標的股數不為1000)
                                           //W3 認售權證、等比例發行(公開發行時原始轉換標的股數為1000)
                                           //W4 認售權證、不等比例發行(公開發行時原始轉換標的股數不為1000)
                                           //BS 本國企業上市屬證券股，FB 本國企業上市屬銀行股
                                           //空白 其他本國企業上市證券，RR 其它外國企業上市證券
                                           //RS 外國企業上市屬證券股，RB 外國企業上市屬銀行股
        public string MarkStkCount { get; set; }//股票筆數註記
        /// 股票異常代碼 00—正常 01—注意 02—處置 03—注意及處置04—再次處置 05—注意及再次處置06—彈性處置 07—注意及彈性處置
        public int StkErrCode { get; set; }
        public string MarkStockType { get; set; }/// 類股註記   0，1:股為中小企業股
        public int ReferencePrice { get; set; }/// 今日參考價v9
        public int UpperPrice { get; set; }//v9
        public int LowerPrice { get; set; }//v9
        public string MarkNoTenYuan { get; set; }///非十元面額註記        
        public string MarkErrPush { get; set; }/// 異常推介個股註記        
        public string MarkS { get; set; }/// 特殊異常註記        
        public string MarkDayTrade { get; set; }// 現股當沖註記        
        public string MarkW { get; set; }///豁免平盤下融券賣出註記        
        public string MarkM { get; set; }///豁免平盤下借券賣出註記        
        public int MatchSecond { get; set; }///撮合循環秒數
        public string WarrantCode { get; set; }//權證識別碼
        public int StrikePrice { get; set; }//履約價格v9
        public int PrePerformanceQty { get; set; }///前一營業日履約數量
        public int PreCancelQty { get; set; }///前一營業日註銷數量
        public int IssueBalanceQty { get; set; }///發行餘額量
        public int ExerciseRatio { get; set; }///行使比率v9
        public int UpperLimitPrice { get; set; }//v9
        public int LowerLimitPrice { get; set; }//v9
        public int ExpiryDate { get; set; }///到期日
        public int Unit { get; set; }//交易單位
        public string CurrencyCode { get; set; }//幣別 "   ":新台幣 CNY:人民、JPY日圓 KRW韓國 USD美元 CAD 加拿大 GBP 英鎊 EUR歐元 SEK瑞典克節、HKD港幣 SGD新加坡
        public int MarkQuotesLine { get; set; }//行情資訊線路註記
        public Format1(byte[] data)
        {
            try
            {
                Header = new Header(data);
                if (data.Length < Header.Length - 3)//-3 無檢查碼和terminal code
                {
                    _logger.Info(string.Format("TPEX_Format1(): datalength = {0} needlength = {1}  Data={2}.", data.Length, Header.Length-3, Encoding.Default.GetString(data)));
                    return;
                }
                StockID = Encoding.ASCII.GetString(data, 10, 6).Trim();
                StockName = Encoding.Default.GetString(data, 16, 16).Trim();
                IndCode = Encoding.ASCII.GetString(data, 32, 2).Trim();
                StkCode = Encoding.ASCII.GetString(data, 34, 2).Trim();
                MarkStkCount = Encoding.ASCII.GetString(data, 36, 2).Trim();
                StkErrCode = Functions.ConvertToFormat9(data, 38, 1);
                MarkStockType = Encoding.ASCII.GetString(data, 39, 1).Trim();
                ReferencePrice = Functions.ConvertToFormat9(data, 40, 3);
                UpperPrice = Functions.ConvertToFormat9(data, 43, 3);
                LowerPrice = Functions.ConvertToFormat9(data, 46, 3);
                MarkNoTenYuan = Encoding.ASCII.GetString(data, 49, 1).Trim();
                MarkErrPush = Encoding.ASCII.GetString(data, 50, 1).Trim();
                MarkS = Encoding.ASCII.GetString(data, 51, 1).Trim();
                MarkDayTrade = Encoding.ASCII.GetString(data, 52, 1).Trim();
                MarkW = Encoding.ASCII.GetString(data, 53, 1).Trim();
                MarkM = Encoding.ASCII.GetString(data, 54, 1).Trim();
                MatchSecond = Functions.ConvertToFormat9(data, 55, 3);
                WarrantCode = Encoding.ASCII.GetString(data, 58, 1).Trim();
                StrikePrice = Functions.ConvertToFormat9(data, 59, 4);
                PrePerformanceQty = Functions.ConvertToFormat9(data, 63, 5);
                PreCancelQty = Functions.ConvertToFormat9(data, 68, 5);
                IssueBalanceQty = Functions.ConvertToFormat9(data, 73, 5);
                ExerciseRatio = Functions.ConvertToFormat9(data, 78, 4);
                UpperLimitPrice = Functions.ConvertToFormat9(data, 82, 4);
                LowerLimitPrice = Functions.ConvertToFormat9(data, 86, 4);
                ExpiryDate = Functions.ConvertToFormat9(data, 90, 4);
                Unit = Functions.ConvertToFormat9(data, 94, 3);
                CurrencyCode = Encoding.ASCII.GetString(data, 97, 3).Trim();
                MarkQuotesLine = Functions.ConvertToFormat9(data, 100, 1);

            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("TPEX_Format1(): ErrMsg = {0} Data={1}.", err.Message,Encoding.Default.GetString(data)));
            }
        }
    }
}
