using Adapter.PatsGlobal.PatsWrapper;
using ESunnyPATSConverterApi;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Adapter.PatsGlobal
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        PublisherSocket _socket;
        private List<PriceUpdateStruct> _symbolList = new List<PriceUpdateStruct>();
        private byte[] _dataMD = new byte[287];
        private string _instrumentKeyMD;
        private string _prefixMD;
        private byte[] _dataTD = new byte[12];
        private string _instrumentKeyTD;
        private string _prefixTD;
        public MainWindow()
        {
            InitializeComponent();

            _symbolList = new List<PriceUpdateStruct>();
            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;            
            ClientAPI.Instance.LogonComplete += Instance_LogonComplete;
            ClientAPI.Instance.PriceLinkStatusEvent += Instance_PriceLinkStatusEvent;
            ClientAPI.Instance.PriceDetailEvent += Instance_PriceDetailEvent;
            ClientAPI.Instance.TickDetailEvent += Instance_TickDetailEvent;
        }

        private void Instance_TickDetailEvent(object sender, EventDelegateTickerArgs e)
        {
            _instrumentKeyTD = e.tickerData.ExchangeName + '.' + e.tickerData.ContractName + '.' + e.tickerData.ContractDate;
            if (GetBytes(e.tickerData, ref _dataTD) == false) { return; }
            _prefixTD = string.Format("6#2#{0}#", _instrumentKeyTD);
            _socket.SendMoreFrame(_prefixTD).SendFrame(_dataTD);
        }

        private void Instance_PriceDetailEvent(object sender, EventDelegatePriceUpdateArgs e)
        {
            _instrumentKeyMD = e.ExchangeLookup.ExchangeName + '.' + e.ExchangeLookup.CommodityName + '.' + e.ExchangeLookup.ContractDate;
            if (GetBytes(e.PriceUpdateObject, ref _dataMD) == false) { return; }
            _prefixMD = string.Format("6#1#{0}#", _instrumentKeyMD);
            _socket.SendMoreFrame(_prefixMD).SendFrame(_dataMD);
        }

        private void Instance_PriceLinkStatusEvent(object sender, EventArgs e)
        {
            if (Convert.ToInt32(sender) == Constants.LinkStatusConnected)
            {
                string prefix = "6#0#all#";
                foreach (PriceUpdateStruct pricedata in _symbolList)
                {
                    //盤前資料
                    byte[] toBytes = new byte[73];
                    SetBytes(ref toBytes, 0, pricedata.ExchangeName, 11);
                    SetBytes(ref toBytes, 11, pricedata.CommodityName, 11);
                    SetBytes(ref toBytes, 22, pricedata.ContractDate, 51);
                    _socket.SendMoreFrame(prefix).SendFrame(toBytes);
                    //訂閱
                    ClientAPIMethods.DoSubscribePrice(pricedata.ExchangeName, pricedata.CommodityName, pricedata.ContractDate);
                }
                byte[] endtoBytes = new byte[73];
                SetBytes(ref endtoBytes, 0, "End", 11);
                _socket.SendMoreFrame(prefix).SendFrame(endtoBytes);
            }
        }

        private void Instance_LogonComplete(object sender, EventArgs e)
        {
            //訂閱商品資料列表
            int exchangeCount = 0;
            ClientAPIMethods.DoCountExchanges(ref exchangeCount);
            IDictionaryEnumerator exchangeIterator = ClientAPI.Instance.GetExchangesEnumerator();
            while (exchangeIterator.MoveNext())
            {
                var exchange = (Exchange)exchangeIterator.Value;
                IDictionaryEnumerator commodityIterator = exchange.GetCommoditiesEnumerator();
                while (commodityIterator.MoveNext())
                {
                    var commodity = (Commodity)commodityIterator.Value;
                    IDictionaryEnumerator contractIterator = commodity.GetContractsEnumerator();

                    //去除無易盛商品訊息
                    if (ESunnyPATSMapConverter.Instance.ContainESunnyCommondityNo(exchange.GetKey() + "." + commodity.GetKey()) == false)
                    {
                        continue;
                    }
                    while (contractIterator.MoveNext())
                    {
                        var contract = (Contract)contractIterator.Value;

                        var priceUpdateStruct = new PriceUpdateStruct
                        {
                            ExchangeName = exchange.GetKey(),
                            CommodityName = commodity.GetKey(),
                            ContractDate = contract.GetKey()
                        };
                        _symbolList.Add(priceUpdateStruct);
                    }
                }
            }
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientAPIMethods.DoLogoff();
            Environment.Exit(0);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text = string.Format("PATS_HOST_IP={0}", DefaultSettings.Instance.PATS_HOST_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_HOST_PORT={0}", DefaultSettings.Instance.PATS_HOST_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_PRICE_IP={0}", DefaultSettings.Instance.PATS_PRICE_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_PRICE_PORT={0}", DefaultSettings.Instance.PATS_PRICE_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_ID={0}", DefaultSettings.Instance.PATS_ID) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_APPID={0}", DefaultSettings.Instance.PATS_APPID) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_LICENSE={0}", DefaultSettings.Instance.PATS_LICENSE) + Environment.NewLine;
            tbMessage.Text += string.Format("PUB_ADDRESS={0}", DefaultSettings.Instance.PUB_ADDRESS) + Environment.NewLine;

            _socket = new PublisherSocket(DefaultSettings.Instance.PUB_ADDRESS);
            ClientAPI.Instance.Start();
        }

        /// <summary>
        /// 轉換PatsDepthMD結構的byte array
        /// </summary>
        private bool GetBytes(PriceStruct md, ref byte[] data)
        {
            bool isTryParse = true;
            double price = 0;
            data[0] = md.Offer.Hour;
            data[1] = md.Offer.Minute;
            data[2] = md.Offer.Second;
            SetBytes(ref data, 3, md.Total.Volume);
            isTryParse &= DoubleTryParse(md.High.Price, out price);
            SetBytes(ref data, 7, price);
            isTryParse &= DoubleTryParse(md.Low.Price, out price);
            SetBytes(ref data, 15, price);
            isTryParse &= DoubleTryParse(md.Opening.Price, out price);
            SetBytes(ref data, 23, price);
            isTryParse &= DoubleTryParse(md.Closing.Price, out price);
            SetBytes(ref data, 31, price);
            isTryParse &= DoubleTryParse(md.BidDOM0.Price, out price);
            SetBytes(ref data, 39, price);
            SetBytes(ref data, 47, md.BidDOM0.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM0.Price, out price);
            SetBytes(ref data, 51, price);
            SetBytes(ref data, 59, md.OfferDOM0.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM1.Price, out price);
            SetBytes(ref data, 63, price);
            SetBytes(ref data, 71, md.BidDOM1.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM1.Price, out price);
            SetBytes(ref data, 75, price);
            SetBytes(ref data, 83, md.OfferDOM1.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM2.Price, out price);
            SetBytes(ref data, 87, price);
            SetBytes(ref data, 95, md.BidDOM2.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM2.Price, out price);
            SetBytes(ref data, 99, price);
            SetBytes(ref data, 107, md.OfferDOM2.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM3.Price, out price);
            SetBytes(ref data, 111, price);
            SetBytes(ref data, 119, md.BidDOM3.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM3.Price, out price);
            SetBytes(ref data, 123, price);
            SetBytes(ref data, 131, md.OfferDOM3.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM4.Price, out price);
            SetBytes(ref data, 135, price);
            SetBytes(ref data, 143, md.BidDOM4.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM4.Price, out price);
            SetBytes(ref data, 147, price);
            SetBytes(ref data, 155, md.OfferDOM4.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM5.Price, out price);
            SetBytes(ref data, 159, price);
            SetBytes(ref data, 167, md.BidDOM5.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM5.Price, out price);
            SetBytes(ref data, 171, price);
            SetBytes(ref data, 179, md.OfferDOM5.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM6.Price, out price);
            SetBytes(ref data, 183, price);
            SetBytes(ref data, 191, md.BidDOM6.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM6.Price, out price);
            SetBytes(ref data, 195, price);
            SetBytes(ref data, 203, md.OfferDOM6.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM7.Price, out price);
            SetBytes(ref data, 207, price);
            SetBytes(ref data, 215, md.BidDOM7.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM7.Price, out price);
            SetBytes(ref data, 219, price);
            SetBytes(ref data, 227, md.OfferDOM7.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM8.Price, out price);
            SetBytes(ref data, 231, price);
            SetBytes(ref data, 239, md.BidDOM8.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM8.Price, out price);
            SetBytes(ref data, 243, price);
            SetBytes(ref data, 251, md.OfferDOM8.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM9.Price, out price);
            SetBytes(ref data, 255, price);
            SetBytes(ref data, 263, md.BidDOM9.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM9.Price, out price);
            SetBytes(ref data, 267, price);
            SetBytes(ref data, 275, md.OfferDOM9.Volume);
            isTryParse &= DoubleTryParse(md.ReferencePrice.Price, out price);
            SetBytes(ref data, 279, price);
            return isTryParse;
        }

        private bool GetBytes(TickerUpdStruct md, ref byte[] data)
        {
            bool isTryParse = true;
            double price = 0;
            isTryParse &= DoubleTryParse(md.LastPrice, out price);
            SetBytes(ref data, 0, price);
            SetBytes(ref data, 8, md.LastVolume);
            return isTryParse;
        }

        private static void SetBytes(ref byte[] data, int offset, double value)
        {
            var array = BitConverter.GetBytes(value);
            for (int i = 0; i < array.Length; i++)
            {
                data[offset + i] = array[i];
            }
        }

        private static void SetBytes(ref byte[] data, int offset, int value)
        {
            data[offset] = (byte)value;
            data[offset + 1] = (byte)(value >> 8);
            data[offset + 2] = (byte)(value >> 16);
            data[offset + 3] = (byte)(value >> 24);
        }

        private static void SetBytes(ref byte[] data, int offset, string value, int lengthOfValue)
        {
            var content = value.PadLeft(lengthOfValue);
            var array = Encoding.UTF8.GetBytes(content);
            for (int i = 0; i < array.Length; i++)
            {
                data[offset + i] = array[i];
            }
        }

        private bool DoubleTryParse(string orgprice, out double price)
        {
            if (string.IsNullOrEmpty(orgprice))
            {
                price = 0;
                return true;
            }
            return double.TryParse(orgprice, out price);
        }
    }
}
