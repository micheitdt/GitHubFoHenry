using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QuoteChartExample.Model
{
    [Serializable]
    public class SymbolTaifex : INotifyPropertyChanged
    {
        public SymbolTaifex()
        { }

        public SymbolTaifex(MarketDataApi.PacketTAIFEX.I010 data)
        {
            _symbolNo = data.B_ProdId;
            _typeNo = (data.H_TransmissionCode == "1") ? "期貨" : "選擇權";
            _riseLimitPrice1 = data.B_RiseLimitPrice1;//第一漲停價
            _referencePrice = data.B_ReferencePrice;//參考價
            _fallLimitPrice1 = data.B_FallLimitPrice1;//第一跌停價
            _riseLimitPrice2 = data.B_RiseLimitPrice2;//第二漲停價
            _fallLimitPrice2 = data.B_FallLimitPrice2;//第二跌停價
            _riseLimitPrice3 = data.B_RiseLimitPrice3;//第三漲停價
            _fallLimitPrice3 = data.B_FallLimitPrice3;//第三跌停價
            _proKind = data.B_ProKind;//契約種類 I:指數類  R:利率類  B:債券類  C:商品類  S:股票類  E:匯率類
            _decimalLocator = data.B_DecimalLocator;//價格欄位小數位數
            _strikePriceDecimalLocator = data.B_StrikePriceDecimalLocator;//選擇權商品代號之履約價格小數位數
            _beginDate = data.B_BeginDate;//上市日期
            _endDate = data.B_EndDate;//下市日期
            _flowGroup = data.B_FlowGroup;//流程群組
            _deliveryDate = data.B_DeliveryDate;//最後結算日

            OnPropertyChanged();
        }

        #region Fields
        private string _symbolNo;
        private string _symbolName;
        private string _typeNo;
        private int _riseLimitPrice1;//第一漲停價
        private int _referencePrice;//參考價
        private int _fallLimitPrice1;//第一跌停價
        private int _riseLimitPrice2;//第二漲停價
        private int _fallLimitPrice2;//第二跌停價
        private int _riseLimitPrice3;//第三漲停價
        private int _fallLimitPrice3;//第三跌停價
        private string _proKind;//契約種類 I:指數類  R:利率類  B:債券類  C:商品類  S:股票類  E:匯率類
        private int _decimalLocator;//價格欄位小數位數
        private int _strikePriceDecimalLocator;//選擇權商品代號之履約價格小數位數
        private int _beginDate;//上市日期
        private int _endDate;//下市日期
        private int _flowGroup;//流程群組
        private int _deliveryDate;//最後結算日
        #endregion

        #region Properties
        /// <summary>
        /// 商品代碼
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
        /// 商品名稱
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
        /// 類型代碼(期貨、選擇權)
        /// </summary>
        public string TypeNo
        {
            get
            {
                return _typeNo;
            }
            internal set
            {
                if (_typeNo == value)
                    return;

                _typeNo = value;
                OnPropertyChanged("TypeNo");
            }
        }
        /// <summary>
        /// 第一漲停價
        /// </summary>
        public int RiseLimitPrice1 {
            get
            {
                return _riseLimitPrice1;
            }
            internal set
            {
                if (_riseLimitPrice1 == value)
                    return;
                _riseLimitPrice1 = value;
                OnPropertyChanged("RiseLimitPrice1");
            }
        }
        /// <summary>
        /// 參考價
        /// </summary>
        public int ReferencePrice {
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
        /// 第一跌停價
        /// </summary>
        public int FallLimitPrice1 {
            get
            {
                return _fallLimitPrice1;
            }
            internal set
            {
                if (_fallLimitPrice1 == value)
                    return;

                _fallLimitPrice1 = value;
                OnPropertyChanged("FallLimitPrice1");
            }
        }
        /// <summary>
        /// 第二漲停價
        /// </summary>
        public int RiseLimitPrice2 {
            get
            {
                return _riseLimitPrice2;
            }
            internal set
            {
                if (_riseLimitPrice2 == value)
                    return;

                _riseLimitPrice2 = value;
                OnPropertyChanged("RiseLimitPrice2");
            }
        }
        /// <summary>
        /// 第二跌停價
        /// </summary>
        public int FallLimitPrice2 {
            get
            {
                return _fallLimitPrice2;
            }
            internal set
            {
                if (_fallLimitPrice2 == value)
                    return;

                _fallLimitPrice2 = value;
                OnPropertyChanged("FallLimitPrice2");
            }
        }
        /// <summary>
        /// 第三漲停價
        /// </summary>
        public int RiseLimitPrice3 {
            get
            {
                return _riseLimitPrice3;
            }
            internal set
            {
                if (_riseLimitPrice3 == value)
                    return;

                _riseLimitPrice3 = value;
                OnPropertyChanged("RiseLimitPrice3");
            }
        }
        /// <summary>
        /// 第三跌停價
        /// </summary>
        public int FallLimitPrice3 {
            get
            {
                return _fallLimitPrice3;
            }
            internal set
            {
                if (_fallLimitPrice3 == value)
                    return;

                _fallLimitPrice3 = value;
                OnPropertyChanged("FallLimitPrice3");
            }
        }
        /// <summary>
        /// 契約種類 I:指數類  R:利率類  B:債券類  C:商品類  S:股票類  E:匯率類
        /// </summary>
        public string ProKind {
            get
            {
                return _proKind;
            }
            internal set
            {
                if (_proKind == value)
                    return;

                _proKind = value;
                OnPropertyChanged("ProKind");
            }
        }
        /// <summary>
        /// 價格欄位小數位數
        /// </summary>
        public int DecimalLocator {
            get
            {
                return _decimalLocator;
            }
            internal set
            {
                if (_decimalLocator == value)
                    return;

                _decimalLocator = value;
                OnPropertyChanged("DecimalLocator");
            }
        }
        /// <summary>
        /// 選擇權商品代號之履約價格小數位數
        /// </summary>
        public int StrikePriceDecimalLocator {
            get
            {
                return _strikePriceDecimalLocator;
            }
            internal set
            {
                if (_strikePriceDecimalLocator == value)
                    return;

                _strikePriceDecimalLocator = value;
                OnPropertyChanged("StrikePriceDecimalLocator");
            }
        }
        /// <summary>
        /// 上市日期
        /// </summary>
        public int BeginDate {
            get
            {
                return _beginDate;
            }
            internal set
            {
                if (_beginDate == value)
                    return;

                _beginDate = value;
                OnPropertyChanged("BeginDate");
            }
        }
        /// <summary>
        /// 下市日期
        /// </summary>
        public int EndDate {
            get
            {
                return _endDate;
            }
            internal set
            {
                if (_endDate == value)
                    return;

                _endDate = value;
                OnPropertyChanged("EndDate");
            }
        }
        /// <summary>
        /// 流程群組
        /// </summary>
        public int FlowGroup {
            get
            {
                return _flowGroup;
            }
            internal set
            {
                if (_flowGroup == value)
                    return;

                _flowGroup = value;
                OnPropertyChanged("FlowGroup");
            }
        }
        /// <summary>
        /// 最後結算日
        /// </summary>
        public int DeliveryDate {
            get
            {
                return _deliveryDate;
            }
            internal set
            {
                if (_deliveryDate == value)
                    return;

                _deliveryDate = value;
                OnPropertyChanged("DeliveryDate");
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
    public class SymbolTaifexList : ConcurrentDictionary<string, SymbolTaifex>
    {
        static SymbolTaifexList()
        {
            AllSymbolTaifexList = new SymbolTaifexList();
        }

        public static SymbolTaifexList AllSymbolTaifexList
        {
            get;
            internal set;
        }

        public static ObservableCollection<SymbolTaifex> GetAllSymbolTaifexCollection()
        {
            return new ObservableCollection<SymbolTaifex>(AllSymbolTaifexList.Values);
        }

        public static void AddSymbolTalfexData(SymbolTaifex data)
        {
            AllSymbolTaifexList.TryAdd(data.SymbolNo , data);
        }

        public static void UpdateSymbolTalfexData(SymbolTaifex data)
        {
            if (AllSymbolTaifexList.ContainsKey(data.SymbolNo))
            {
                AllSymbolTaifexList[data.SymbolNo] = data;
            }
            else
            {
                AllSymbolTaifexList.TryAdd(data.SymbolNo, data);
            }
        }

        public static void SetSymbolTaifexDataList(IDictionary<string, MarketDataApi.PacketTAIFEX.I010> data)
        {
            foreach (var obj in data)
            {
                AllSymbolTaifexList.TryAdd(obj.Key, new SymbolTaifex(obj.Value));
            }
        }

        public void AddTalfexData(MarketDataApi.PacketTAIFEX.I010 data)
        {
            SymbolTaifex temp = new SymbolTaifex(data);
            AllSymbolTaifexList.TryAdd(data.B_ProdId, temp);
        }

        public static ObservableCollection<SymbolTaifex> GetFilterSymbolTalfexList(string symbolno, string symbolName)
        {
            return new ObservableCollection<SymbolTaifex>(AllSymbolTaifexList.Values.Where(x => (string.IsNullOrEmpty(symbolno) ? true : x.SymbolNo.Contains(symbolno) & (string.IsNullOrEmpty(symbolName) ? true : x.SymbolName.Contains(symbolName)))));
        }

        public static ObservableCollection<SymbolTaifex> GetFilterSymbolNo(string symbolno)
        {
            return new ObservableCollection<SymbolTaifex>(AllSymbolTaifexList.Values.Where(x => (string.IsNullOrEmpty(symbolno) ? true : x.SymbolNo.Contains(symbolno))));
        }
    }
}
