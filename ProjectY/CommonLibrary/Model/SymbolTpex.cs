using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Model
{
    [Serializable]
    public class SymbolTpex : INotifyPropertyChanged
    {
        public SymbolTpex()
        { }
        
        public SymbolTpex(PacketTPEX.Format1 data)
        {
            _symbolNo = data.StockID;
            _symbolName = data.StockName;
            _market = "上市";
            _indCode = data.IndCode;
            _stkCode = data.StkCode;
            _stkCountMark = data.MarkStkCount;
            _stkErrCode = data.StkErrCode;
            _markStockType = data.MarkStockType;
            _referencePrice = data.ReferencePrice;
            _upperPrice = data.UpperPrice;
            _lowerPrice = data.LowerPrice;
            _markNoTenYuan = data.MarkNoTenYuan;
            _markErrPush = data.MarkErrPush;
            _markS = data.MarkS;
            _markDayTrade = data.MarkDayTrade;
            _markW = data.MarkW;
            _markM = data.MarkM;
            _matchSecond = data.MatchSecond;
            //---------權證資料
            _warrantCode = data.WarrantCode;
            _strikePrice = data.StrikePrice;
            _prePerformanceQty = data.PrePerformanceQty;
            _preCancelQty = data.PreCancelQty;
            _issueBalanceQty = data.IssueBalanceQty;
            _exerciseRatio = data.ExerciseRatio;
            _upperLimitPrice = data.UpperLimitPrice;
            _lowerLimitPrice = data.LowerLimitPrice;
            _expiryDate = data.ExpiryDate;
            //---------其他
            _unit = data.Unit;
            _currencyCode = data.CurrencyCode;
            _markQuotesLine = data.MarkQuotesLine;
            OnPropertyChanged();
        }

        #region Fields
        private string _symbolNo;
        private string _symbolName;
        private string _market;
        private string _indCode;
        private string _stkCode;
        private string _stkCountMark;
        private int _stkErrCode;
        private string _markStockType;
        private int _referencePrice;
        private int _upperPrice;
        private int _lowerPrice;
        private string _markNoTenYuan;
        private string _markErrPush;
        private string _markS;
        private string _markDayTrade;
        private string _markW;
        private string _markM;
        private int _matchSecond;
        //---------權證資料
        private string _warrantCode;
        private int _strikePrice;
        private int _prePerformanceQty;
        private int _preCancelQty;
        private int _issueBalanceQty;
        private int _exerciseRatio;
        private int _upperLimitPrice;
        private int _lowerLimitPrice;
        private int _expiryDate;
        //---------其他
        private int _unit;
        private string _currencyCode;
        private int _markQuotesLine;
        #endregion

        #region Properties
        /// <summary>
        /// 股票代碼
        /// </summary>
        public string SymbolNo
        {
            get
            {
                return _symbolNo;
            }
            internal set
            {
                if (_symbolNo == value)
                    return;

                _symbolNo = value;
                OnPropertyChanged("SymbolNo");
            }
        }
        /// <summary>
        /// 股票簡稱
        /// </summary>
        public string SymbolName
        {
            get
            {
                return _symbolName;
            }
            internal set
            {
                if (_symbolName == value)
                    return;

                _symbolName = value;
                OnPropertyChanged("SymbolName");
            }
        }
        /// <summary>
        /// 市場別代碼(上市、上櫃)
        /// </summary>
        public string Market
        {
            get
            {
                return _market;
            }
            internal set
            {
                if (_market == value)
                    return;

                _market = value;
                OnPropertyChanged("Market");
            }
        }
        /// <summary>
        /// 產業別 02食品工業 03塑膠工業 04紡織纖維 05電機械 06電器纜 08玻璃陶瓷 10鋼鐵工業 11橡膠工業 
        ///        14建材營造15航運業 16觀光事業 17金融業 18貿易百貨 20 其他 21化學工業 22 生技醫療 
        ///        23 油電燃氣業 24 半導體業 25 電腦及週邊設備業 26 光電業 27 通信網路業 28 電子零組件業 
        ///        29 電子通路業 30 資訊服務業 31 其他電子業 32文化創意業 33農業科技 80管理股票 
        /// </summary>
        public string IndCode
        {
            get
            {
                return _indCode;
            }
            internal set
            {
                if (_indCode == value)
                    return;

                _indCode = value;
                OnPropertyChanged("IndCode");
            }
        }
        /// <summary>
        /// 証券別  W1 認購權證、等比例發行(公開發行時原始轉換標的股數為1000)
        ///         W2 認購權證、不等比例發行(公開發行時原始轉換標的股數不為1000)
        ///         W3 認售權證、等比例發行(公開發行時原始轉換標的股數為1000)
        ///         W4 認售權證、不等比例發行(公開發行時原始轉換標的股數不為1000)
        ///         BS 本國企業上市屬證券股，FB 本國企業上市屬銀行股
        ///         空白 其他本國企業上市證券，RR 其它外國企業上市證券
        ///         RS 外國企業上市屬證券股，RB 外國企業上市屬銀行股
        /// </summary>
        public string StkCode
        {
            get
            {
                return _stkCode;
            }
            internal set
            {
                if (_stkCode == value)
                    return;

                _stkCode = value;
                OnPropertyChanged("StkCode");
            }
        }
        /// <summary>
        /// 股票筆數註記
        /// </summary>
        public string StkCountMark
        {
            get
            {
                return _stkCountMark;
            }
            internal set
            {
                if (_stkCountMark == value)
                    return;

                _stkCountMark = value;
                OnPropertyChanged("StkCountMark");
            }
        }
        /// <summary>
        /// 股票異常代碼 00—正常 01—注意 02—處置 03—注意及處置04—再次處置 05—注意及再次處置06—彈性處置 07—注意及彈性處置
        /// </summary>
        public int StkErrCode
        {
            get
            {
                return _stkErrCode;
            }
            internal set
            {
                if (_stkErrCode == value)
                    return;

                _stkErrCode = value;
                OnPropertyChanged("StkErrCode");
            }
        }
        /// <summary>
        /// 類股註記   0，1:股為中小企業股
        /// </summary>
        public string MarkStockType
        {
            get
            {
                return _markStockType;
            }
            internal set
            {
                if (_markStockType == value)
                    return;

                _markStockType = value;
                OnPropertyChanged("MarkStockType");
            }
        }
        /// <summary>
        /// 今日參考價小數2位
        /// </summary>
        public int ReferencePrice
        {
            get
            {
                return _referencePrice;
            }
            internal set
            {
                if (_referencePrice == value)
                    return;

                _referencePrice = value;
                OnPropertyChanged("ReferencePrice");
            }
        }
        /// <summary>
        /// 漲停小數2位
        /// </summary>
        public int UpperPrice
        {
            get
            {
                return _upperPrice;
            }
            internal set
            {
                if (_upperPrice == value)
                    return;

                _upperPrice = value;
                OnPropertyChanged("UpperPrice");
            }
        }
        /// <summary>
        /// 跌停小數2位
        /// </summary>
        public int LowerPrice
        {
            get
            {
                return _lowerPrice;
            }
            internal set
            {
                if (_lowerPrice == value)
                    return;

                _lowerPrice = value;
                OnPropertyChanged("LowerPrice");
            }
        }
        /// <summary>
        /// 非十元面額註記
        /// </summary>
        public string MarkNoTenYuan
        {
            get
            {
                return _markNoTenYuan;
            }
            internal set
            {
                if (_markNoTenYuan == value)
                    return;

                _markNoTenYuan = value;
                OnPropertyChanged("MarkNoTenYuan");
            }
        }
        /// <summary>
        /// 異常推介個股註記
        /// </summary>
        public string MarkErrPush
        {
            get
            {
                return _markErrPush;
            }
            internal set
            {
                if (_markErrPush == value)
                    return;

                _markErrPush = value;
                OnPropertyChanged("MarkErrPush");
            }
        }
        /// <summary>
        /// 特殊異常註記
        /// </summary>
        public string MarkS
        {
            get
            {
                return _markS;
            }
            internal set
            {
                if (_markS == value)
                    return;

                _markS = value;
                OnPropertyChanged("MarkS");
            }
        }
        /// <summary>
        /// 現股當沖註記
        /// </summary>
        public string MarkDayTrade
        {
            get
            {
                return _markDayTrade;
            }
            internal set
            {
                if (_markDayTrade == value)
                    return;

                _markDayTrade = value;
                OnPropertyChanged("MarkDayTrade");
            }
        }
        /// <summary>
        /// 豁免平盤下融券賣出註記
        /// </summary>
        public string MarkW
        {
            get
            {
                return _markW;
            }
            internal set
            {
                if (_markW == value)
                    return;

                _markW = value;
                OnPropertyChanged("MarkW");
            }
        }
        /// <summary>
        /// 豁免平盤下借券賣出註記
        /// </summary>
        public string MarkM
        {
            get
            {
                return _markM;
            }
            internal set
            {
                if (_markM == value)
                    return;

                _markM = value;
                OnPropertyChanged("MarkM");
            }
        }
        /// <summary>
        /// 撮合循環秒數
        /// </summary>
        public int MatchSecond
        {
            get
            {
                return _matchSecond;
            }
            internal set
            {
                if (_matchSecond == value)
                    return;

                _matchSecond = value;
                OnPropertyChanged("MatchSecond");
            }
        }
        /// <summary>
        /// 權證識別碼
        /// </summary>
        public string WarrantCode
        {
            get
            {
                return _warrantCode;
            }
            internal set
            {
                if (_warrantCode == value)
                    return;

                _warrantCode = value;
                OnPropertyChanged("WarrantCode");
            }
        }
        /// <summary>
        /// 覆約價小數2位
        /// </summary>
        public int StrikePrice
        {
            get
            {
                return _strikePrice;
            }
            internal set
            {
                if (_strikePrice == value)
                    return;

                _strikePrice = value;
                OnPropertyChanged("StrikePrice");
            }
        }
        /// <summary>
        /// 前一營業日履約數量
        /// </summary>
        public int PrePerformanceQty
        {
            get
            {
                return _prePerformanceQty;
            }
            internal set
            {
                if (_prePerformanceQty == value)
                    return;

                _prePerformanceQty = value;
                OnPropertyChanged("PrePerformanceQty");
            }
        }
        /// <summary>
        /// 前一營業日註銷數量
        /// </summary>
        public int PreCancelQty
        {
            get
            {
                return _preCancelQty;
            }
            internal set
            {
                if (_preCancelQty == value)
                    return;

                _preCancelQty = value;
                OnPropertyChanged("PreCancelQty");
            }
        }
        /// <summary>
        /// 發行餘額量
        /// </summary>
        public int IssueBalanceQty
        {
            get
            {
                return _issueBalanceQty;
            }
            internal set
            {
                if (_issueBalanceQty == value)
                    return;

                _issueBalanceQty = value;
                OnPropertyChanged("IssueBalanceQty");
            }
        }
        /// <summary>
        /// 行使比率
        /// </summary>
        public int ExerciseRatio
        {
            get
            {
                return _exerciseRatio;
            }
            internal set
            {
                if (_exerciseRatio == value)
                    return;

                _exerciseRatio = value;
                OnPropertyChanged("ExerciseRatio");
            }
        }
        /// <summary>
        /// 上限價格小數2位
        /// </summary>
        public int UpperLimitPrice
        {
            get
            {
                return _upperLimitPrice;
            }
            internal set
            {
                if (_upperLimitPrice == value)
                    return;

                _upperLimitPrice = value;
                OnPropertyChanged("UpperLimitPrice");
            }
        }
        /// <summary>
        /// 下限價格小數2位
        /// </summary>
        public int LowerLimitPrice
        {
            get
            {
                return _lowerLimitPrice;
            }
            internal set
            {
                if (_lowerLimitPrice == value)
                    return;

                _lowerLimitPrice = value;
                OnPropertyChanged("LowerLimitPrice");
            }
        }
        /// <summary>
        /// 到期日
        /// </summary>
        public int ExpiryDate
        {
            get
            {
                return _expiryDate;
            }
            internal set
            {
                if (_expiryDate == value)
                    return;

                _expiryDate = value;
                OnPropertyChanged("ExpiryDate");
            }
        }
        /// <summary>
        /// 每張單位
        /// </summary>
        public int Unit
        {
            get
            {
                return _unit;
            }
            internal set
            {
                if (_unit == value)
                    return;

                _unit = value;
                OnPropertyChanged("Unit");
            }
        }
        /// <summary>
        /// 幣別 "   ":新台幣 CNY:人民、JPY日圓 KRW韓國 USD美元 CAD 加拿大 GBP 英鎊 EUR歐元 SEK瑞典克節、HKD港幣 SGD新加坡
        /// </summary>
        public string CurrencyCode
        {
            get
            {
                return _currencyCode;
            }
            internal set
            {
                if (_currencyCode == value)
                    return;

                _currencyCode = value;
                OnPropertyChanged("CurrencyCode");
            }
        }
        /// <summary>
        /// 行情線路註記
        /// </summary>
        public int MarkQuotesLine
        {
            get
            {
                return _markQuotesLine;
            }
            internal set
            {
                if (_markQuotesLine == value)
                    return;

                _markQuotesLine = value;
                OnPropertyChanged("MarkQuotesLine");
            }
        }
        #endregion

        #region func
        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }

    [Serializable]
    public class SymbolTpexList : ConcurrentDictionary<string, SymbolTpex>
    {
        static SymbolTpexList()
        {
            AllSymbolTpexList = new SymbolTpexList();
        }

        public static SymbolTpexList AllSymbolTpexList
        {
            get;
            internal set;
        }

        public static void AddSymbolTpexData(SymbolTpex data)
        {
            AllSymbolTpexList.TryAdd(data.SymbolNo, data);
        }

        public static void SetSymbolTseDataList(IDictionary<string, SymbolTpex> data)
        {
            AllSymbolTpexList = new SymbolTpexList();
            foreach (var obj in data)
            {
                AllSymbolTpexList.TryAdd(obj.Key, obj.Value);
            }
        }

        public static void AddTpexData(PacketTPEX.Format1 data)
        {
            SymbolTpex temp = new SymbolTpex(data);
            AllSymbolTpexList.TryAdd(data.StockID, temp);
        }

        public static ObservableCollection<SymbolTpex> GetFilterSymbolTpexList(string symbolno, string symbolName)
        {
            IEnumerable<SymbolTpex> data = AllSymbolTpexList.Values.Where(x => x.SymbolNo == symbolno & (string.IsNullOrEmpty(symbolName) ? true : x.SymbolNo == symbolName));
            return new ObservableCollection<SymbolTpex>(data);
        }
    }
}
