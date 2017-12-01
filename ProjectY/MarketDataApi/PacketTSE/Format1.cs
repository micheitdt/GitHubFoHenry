using System;
using System.Text;

namespace MarketDataApi.PacketTSE
{
    public class Format1
    {
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
        public string StkCountMark { get; set; }//股票筆數註記
        // 股票異常代碼 00—正常 01—注意 02—處置 03—注意及處置04—再次處置 05—注意及再次處置06—彈性處置 07—注意及彈性處置
        public int StkErrCode { get; set; }
        public int ReferencePrice { get; set; }// 今日參考價V9
        public int UpperPrice { get; set; }//V9
        public int LowerPrice { get; set; }//V9
        public string MarkNoTenYuan { get; set; }//非十元面額註記
        public string MarkErrPush { get; set; }// 異常推介個股註記
        public string MarkS { get; set; }//特殊異常註記
        public string MarkDayTrade { get; set; }// 現股當沖註記
        public string MarkW { get; set; }//豁免平盤下融券賣出註記
        public string MarkM { get; set; }//豁免平盤下借券賣出註記
        public int MatchSecond { get; set; }//撮合循環秒數
        public string WarrantCode { get; set; }//權證識別碼
        public int StrikePrice { get; set; }//覆約價V9
        public int PrePerformanceQty { get; set; }//前一營業日履約數量
        public int PreCancelQty { get; set; }//前一營業日註銷數量
        public int IssueBalanceQty { get; set; }//發行餘額量
        public int ExerciseRatio { get; set; }//行使比率
        public int UpperLimitPrice { get; set; }//上限價V9
        public int LowerLimitPrice { get; set; }//下限價V9
        public int ExpiryDate { get; set; }//到期日
        public string ForeignStkCode { get; set; }//國外識別碼
        public int Unit { get; set; }//單位
        public string CurrencyCode { get; set; }//幣別 "   ":新台幣 CNY:人民、JPY日圓 KRW韓國 USD美元 CAD 加拿大 GBP 英鎊 EUR歐元 SEK瑞典克節、HKD港幣 SGD新加坡
        public int MarkQuotesLine { get; set; }//行情線路註記
        public Format1(byte[] data)
        {
            try
            {
                Header = new Header(data);
                if (data.Length < Header.Length - 3)//-3 無檢查碼和terminal code
                {
                    return;
                }
                StockID = Encoding.ASCII.GetString(data, 10, 6).Trim();
                StockName = Encoding.Default.GetString(data,16,16).Trim();
                IndCode = Encoding.ASCII.GetString(data, 32, 2).Trim();
                StkCode = Encoding.ASCII.GetString(data, 34, 2).Trim();
                StkCountMark = Encoding.ASCII.GetString(data, 36, 2).Trim();
                StkErrCode = Functions.ConvertToFormat9(data, 38, 1);
                ReferencePrice = Functions.ConvertToFormat9(data, 39, 3);
                UpperPrice = Functions.ConvertToFormat9(data, 42, 3);
                LowerPrice = Functions.ConvertToFormat9(data, 45, 3);
                MarkNoTenYuan = Encoding.ASCII.GetString(data, 48, 1).Trim();
                MarkErrPush = Encoding.ASCII.GetString(data, 49, 1).Trim();
                MarkS = Encoding.ASCII.GetString(data, 50, 1).Trim();
                MarkDayTrade = Encoding.ASCII.GetString(data, 51, 1).Trim();
                MarkW = Encoding.ASCII.GetString(data, 52, 1).Trim();
                MarkM = Encoding.ASCII.GetString(data, 53, 1).Trim();
                MatchSecond = Functions.ConvertToFormat9(data, 54, 3);
                WarrantCode = Encoding.ASCII.GetString(data, 57, 1).Trim();
                StrikePrice = Functions.ConvertToFormat9(data, 58, 4);
                PrePerformanceQty = Functions.ConvertToFormat9(data, 62, 5);
                PreCancelQty = Functions.ConvertToFormat9(data, 67, 5);
                IssueBalanceQty = Functions.ConvertToFormat9(data, 72, 5);
                ExerciseRatio = Functions.ConvertToFormat9(data, 77, 4);
                UpperLimitPrice = Functions.ConvertToFormat9(data, 81, 4);
                LowerLimitPrice = Functions.ConvertToFormat9(data, 85, 4);
                ExpiryDate = Functions.ConvertToFormat9(data, 89, 4);
                ForeignStkCode = Encoding.ASCII.GetString(data, 93, 1).Trim();
                Unit = Functions.ConvertToFormat9(data, 94, 3);
                CurrencyCode = Encoding.ASCII.GetString(data, 97, 3).Trim();
                MarkQuotesLine = Functions.ConvertToFormat9(data, 100, 1);
            }
            catch (Exception)
            {
            }
        }
    }
}
