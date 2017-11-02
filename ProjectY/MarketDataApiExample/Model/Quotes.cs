using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarketDataApiExample.Model
{
    [Serializable]
    public class Quotes : INotifyPropertyChanged
    {
        public Quotes()
        { }

        #region Fields
        private long _seq;
        private string _symbolNo;
        private string _market;
        private string _typeNo;
        private int _matchPrice;
        private int _matchQty;
        private int _matchTotalQty;
        private string _time;
        private int _bid1Price;
        private int _bid2Price;
        private int _bid3Price;
        private int _bid4Price;
        private int _bid5Price;
        private int _bid1Qty;
        private int _bid2Qty;
        private int _bid3Qty;
        private int _bid4Qty;
        private int _bid5Qty;
        private int _ask1Price;
        private int _ask2Price;
        private int _ask3Price;
        private int _ask4Price;
        private int _ask5Price;
        private int _ask1Qty;
        private int _ask2Qty;
        private int _ask3Qty;
        private int _ask4Qty;
        private int _ask5Qty;
        #endregion

        #region Properties
        /// <summary>
        /// 序號
        /// </summary>
        public long Seq
        {
            get
            {
                return _seq;
            }
            internal set
            {
                if (_seq == value)
                    return;

                _seq = value;
                OnPropertyChanged("Seq");
            }
        }

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
        /// 市場別代碼(上市、上櫃、日選、夜選、日期、夜期)
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
        /// 類型代碼(I020期權成交、I080期權5檔、6証卷成交)
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
        /// 資訊時間
        /// </summary>
        public string Time
        {
            get
            {
                return _time;
            }
            internal set
            {
                if (_time == value)
                    return;

                _time = value;
                OnPropertyChanged("Time");
            }
        }
        /// <summary>
        /// 撮合價
        /// </summary>
        public int MatchPrice
        {
            get
            {
                return _matchPrice;
            }
            internal set
            {
                if (_matchPrice == value)
                    return;

                _matchPrice = value;
                OnPropertyChanged("MatchPrice");
            }
        }
        /// <summary>
        /// 撮合量
        /// </summary>
        public int MatchQty
        {
            get
            {
                return _matchQty;
            }
            internal set
            {
                if (_matchQty == value)
                    return;

                _matchQty = value;
                OnPropertyChanged("MatchQty");
            }
        }
        /// <summary>
        /// 總量
        /// </summary>
        public int MatchTotalQty
        {
            get
            {
                return _matchTotalQty;
            }
            internal set
            {
                if (_matchTotalQty == value)
                    return;

                _matchTotalQty = value;
                OnPropertyChanged("MatchTotalQty");
            }
        }

        #region 5檔
        public int Bid1Price
        {
            get
            {
                return _bid1Price;
            }
            internal set
            {
                if (_bid1Price == value)
                    return;

                _bid1Price = value;
                OnPropertyChanged("Bid1Price");
            }
        }
        public int Bid2Price
        {
            get
            {
                return _bid2Price;
            }
            internal set
            {
                if (_bid2Price == value)
                    return;

                _bid2Price = value;
                OnPropertyChanged("Bid2Price");
            }
        }
        public int Bid3Price
        {
            get
            {
                return _bid3Price;
            }
            internal set
            {
                if (_bid3Price == value)
                    return;

                _bid3Price = value;
                OnPropertyChanged("Bid3Price");
            }
        }
        public int Bid4Price
        {
            get
            {
                return _bid4Price;
            }
            internal set
            {
                if (_bid4Price == value)
                    return;

                _bid4Price = value;
                OnPropertyChanged("Bid4Price");
            }
        }
        public int Bid5Price
        {
            get
            {
                return _bid5Price;
            }
            internal set
            {
                if (_bid5Price == value)
                    return;

                _bid5Price = value;
                OnPropertyChanged("Bid5Price");
            }
        }
        public int Bid1Qty
        {
            get
            {
                return _bid1Qty;
            }
            internal set
            {
                if (_bid1Qty == value)
                    return;

                _bid1Qty = value;
                OnPropertyChanged("Bid1Qty");
            }
        }
        public int Bid2Qty
        {
            get
            {
                return _bid2Qty;
            }
            internal set
            {
                if (_bid2Qty == value)
                    return;

                _bid2Qty = value;
                OnPropertyChanged("Bid2Qty");
            }
        }
        public int Bid3Qty
        {
            get
            {
                return _bid3Qty;
            }
            internal set
            {
                if (_bid3Qty == value)
                    return;

                _bid3Qty = value;
                OnPropertyChanged("Bid3Qty");
            }
        }
        public int Bid4Qty
        {
            get
            {
                return _bid4Qty;
            }
            internal set
            {
                if (_bid4Qty == value)
                    return;

                _bid4Qty = value;
                OnPropertyChanged("Bid4Qty");
            }
        }
        public int Bid5Qty
        {
            get
            {
                return _bid5Qty;
            }
            internal set
            {
                if (_bid5Qty == value)
                    return;

                _bid5Qty = value;
                OnPropertyChanged("Bid5Qty");
            }
        }
        public int Ask1Price
        {
            get
            {
                return _ask1Price;
            }
            internal set
            {
                if (_ask1Price == value)
                    return;

                _ask1Price = value;
                OnPropertyChanged("Ask1Price");
            }
        }
        public int Ask2Price
        {
            get
            {
                return _ask2Price;
            }
            internal set
            {
                if (_ask2Price == value)
                    return;

                _ask2Price = value;
                OnPropertyChanged("Ask2Price");
            }
        }
        public int Ask3Price
        {
            get
            {
                return _ask3Price;
            }
            internal set
            {
                if (_ask3Price == value)
                    return;

                _ask3Price = value;
                OnPropertyChanged("Ask3Price");
            }
        }
        public int Ask4Price
        {
            get
            {
                return _ask4Price;
            }
            internal set
            {
                if (_ask4Price == value)
                    return;

                _ask4Price = value;
                OnPropertyChanged("Ask4Price");
            }
        }
        public int Ask5Price
        {
            get
            {
                return _ask5Price;
            }
            internal set
            {
                if (_ask5Price == value)
                    return;

                _ask5Price = value;
                OnPropertyChanged("Ask5Price");
            }
        }
        public int Ask1Qty
        {
            get
            {
                return _ask1Qty;
            }
            internal set
            {
                if (_ask1Qty == value)
                    return;

                _ask1Qty = value;
                OnPropertyChanged("Ask1Qty");
            }
        }
        public int Ask2Qty
        {
            get
            {
                return _ask2Qty;
            }
            internal set
            {
                if (_ask2Qty == value)
                    return;

                _ask2Qty = value;
                OnPropertyChanged("Ask2Qty");
            }
        }
        public int Ask3Qty
        {
            get
            {
                return _ask3Qty;
            }
            internal set
            {
                if (_ask3Qty == value)
                    return;

                _ask3Qty = value;
                OnPropertyChanged("Ask3Qty");
            }
        }
        public int Ask4Qty
        {
            get
            {
                return _ask4Qty;
            }
            internal set
            {
                if (_ask4Qty == value)
                    return;

                _ask4Qty = value;
                OnPropertyChanged("Ask4Qty");
            }
        }
        public int Ask5Qty
        {
            get
            {
                return _ask5Qty;
            }
            internal set
            {
                if (_ask5Qty == value)
                    return;

                _ask5Qty = value;
                OnPropertyChanged("Ask5Qty");
            }
        }
        #endregion

        #endregion

        #region func
        public void SetI080Data(long seq, MarketDataApi.PacketTAIFEX.I080 data)
        {
            _seq = seq;
            _symbolNo = data.B_ProdId;
            _typeNo = "I080";
            _market = "期權5檔";
            _bid1Price = data.B_BuyOrderBook[0].MatchPrice;
            _bid1Qty = data.B_BuyOrderBook[0].MatchQuantity;
            _bid2Price = data.B_BuyOrderBook[1].MatchPrice;
            _bid2Qty = data.B_BuyOrderBook[1].MatchQuantity;
            _bid3Price = data.B_BuyOrderBook[2].MatchPrice;
            _bid3Qty = data.B_BuyOrderBook[2].MatchQuantity;
            _bid4Price = data.B_BuyOrderBook[3].MatchPrice;
            _bid4Qty = data.B_BuyOrderBook[3].MatchQuantity;
            _bid5Price = data.B_BuyOrderBook[4].MatchPrice;
            _bid5Qty = data.B_BuyOrderBook[4].MatchQuantity;
            _ask1Price = data.B_SellOrderBook[0].MatchPrice;
            _ask1Qty = data.B_SellOrderBook[0].MatchQuantity;
            _ask2Price = data.B_SellOrderBook[1].MatchPrice;
            _ask2Qty = data.B_SellOrderBook[1].MatchQuantity;
            _ask3Price = data.B_SellOrderBook[2].MatchPrice;
            _ask3Qty = data.B_SellOrderBook[2].MatchQuantity;
            _ask4Price = data.B_SellOrderBook[3].MatchPrice;
            _ask4Qty = data.B_SellOrderBook[3].MatchQuantity;
            _ask5Price = data.B_SellOrderBook[4].MatchPrice;
            _ask5Qty = data.B_SellOrderBook[4].MatchQuantity;
            OnPropertyChanged();
        }

        public void SetI020Data(long seq, MarketDataApi.PacketTAIFEX.I020 data)
        {
            _seq = seq;
            _symbolNo = data.B_ProdId;
            _typeNo = "I020";
            _market = "期權成交";
            _matchPrice = data.B_FirstMatchPrice;
            _matchQty = data.B_FirstMatchQnty;
            _matchTotalQty = data.B_MatchTotalQty;
            _time = data.H_InformationTime.ToString();
            OnPropertyChanged();
        }

        public void SetTpexData(long seq, MarketDataApi.PacketTPEX.Format6 data)
        {
            _seq = seq;
            _symbolNo = data.StockID;
            _typeNo = "6";
            _market = "上櫃";
            _matchPrice = data.LastPrice;
            _matchTotalQty = data.TotalVolume;
            _bid1Price = data.BidData[0].Price;
            _bid1Qty = data.BidData[0].Volume;
            _ask1Price = data.AskData[0].Price;
            _ask1Qty = data.AskData[0].Volume;
            OnPropertyChanged();
        }

        public void SetTseData(long seq, MarketDataApi.PacketTSE.Format6 data)
        {
            _seq = seq;
            _symbolNo = data.StockID;
            _typeNo = "6";
            _market = "上市";
            _matchPrice = data.LastPrice;
            _matchTotalQty = data.TotalVolume;
            _bid1Price = data.BidData[0].Price;
            _bid1Qty = data.BidData[0].Volume;
            _ask1Price = data.AskData[0].Price;
            _ask1Qty = data.AskData[0].Volume;
            OnPropertyChanged();
        }

        //public void SetI010Data(long seq, MarketDataApi.PacketTAIFEX.I010 data)
        //{
        //    _seq = seq;
        //    _symbolNo = data.B_ProdId;
        //    _typeNo = "I010";
        //    _market = "期貨契約";
        //    OnPropertyChanged();
        //}

        //public void SetTpexData(long seq, MarketDataApi.PacketTPEX.Format1 data)
        //{
        //    _seq = seq;
        //    _symbolNo = data.StockID;
        //    _typeNo = "6";
        //    _market = "上櫃";
        //    OnPropertyChanged();
        //}

        //public void SetTseData(long seq, MarketDataApi.PacketTSE.Format1 data)
        //{
        //    _seq = seq;
        //    _symbolNo = data.StockID;
        //    _typeNo = "1";
        //    _market = "上市";
        //    OnPropertyChanged();
        //}
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
}
