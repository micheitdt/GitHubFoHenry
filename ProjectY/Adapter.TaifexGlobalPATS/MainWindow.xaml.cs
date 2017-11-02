using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetMQ;
using NetMQ.Sockets;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Collections;
using Adapter.TaifexGlobalPATS.ApiPATS;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using Newtonsoft.Json;
using CommonLibrary;

namespace Adapter.TaifexGlobalPATS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PublisherSocket _socket;
        private ObservableCollection<PriceUpdateStruct> _symbolList = new ObservableCollection<PriceUpdateStruct>();
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;

            // connecting API handlers
            ClientAPI.Instance.LogonComplete += LogonComplete;//登入完成
            ClientAPI.Instance.PriceLinkStatusEvent += PriceLinkStatusEvent;//價格伺服器連接
            ClientAPI.Instance.PriceDetailEvent += clientAPIPriceDetailEvent;//價格變動
            // -----------------------
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ClientAPIMethods.DoLogoff();
            Environment.Exit(0);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DefaultSettings.Instance.Initialize();
            tbMessage.Text = string.Format("PATS_HOST_IP={0}", DefaultSettings.Instance.PATS_HOST_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_HOST_PORT={0}", DefaultSettings.Instance.PATS_HOST_PORT) + Environment.NewLine;
            tbMessage.Text = string.Format("PATS_PRICE_IP={0}", DefaultSettings.Instance.PATS_PRICE_IP) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_PRICE_PORT={0}", DefaultSettings.Instance.PATS_PRICE_PORT) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_ID={0}", DefaultSettings.Instance.PATS_ID) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_APPID={0}", DefaultSettings.Instance.PATS_APPID) + Environment.NewLine;
            tbMessage.Text += string.Format("PATS_LICENSE={0}", DefaultSettings.Instance.PATS_LICENSE) + Environment.NewLine;
            tbMessage.Text += string.Format("PUB_ADDRESS={0}", DefaultSettings.Instance.PUB_ADDRESS) + Environment.NewLine;

            _socket = new PublisherSocket(DefaultSettings.Instance.PUB_ADDRESS);
            ClientAPI.Instance.Start();
        }
        #region CallPATSMD
        /// <summary>
        /// 登入完成
        /// </summary>
        private void LogonComplete(object sender, EventArgs e)
        {
            //var thread = new Thread(() =>
            //{
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
            //});
            //thread.IsBackground = true;
            //thread.Start();
        }
        /// <summary>
        /// 價格伺服器狀態變更
        /// </summary>
        private void PriceLinkStatusEvent(object sender, EventArgs e)
        {
            if (Convert.ToInt32(sender) == Constants.LinkStatusConnected)
            {
                string prefix = "6#0#all#";
                foreach (PriceUpdateStruct pricedata in _symbolList)
                {
                    //盤前資料
                    byte[] toBytes = PriceUpdateStructGetBytes(pricedata);
                    _socket.SendMoreFrame(prefix).SendFrame(toBytes);
                    //Thread.Sleep(1);

                    //初始值
                    //var eg = new PriceStruct();
                    //if (ClientAPIMethods.DoGetPriceForContract(pricedata, ref eg))
                    //{
                    //}
                    //訂閱
                    ClientAPIMethods.DoSubscribePrice(pricedata.ExchangeName, pricedata.CommodityName, pricedata.ContractDate);
                }
                byte[] endtoBytes = PriceUpdateStructGetBytes(new PriceUpdateStruct() { ExchangeName="End" });
                _socket.SendMoreFrame(prefix).SendFrame(endtoBytes);
            }
        }
        Dictionary<string, string> testData = new Dictionary<string, string>();
        
        /// <summary>
        /// 價格資料接收事件
        /// </summary>
        private void clientAPIPriceDetailEvent(object sender, EventDelegatePriceUpdateArgs e)
        {
            string HashKey = e.ExchangeLookup.ExchangeName + '.' + e.ExchangeLookup.CommodityName + '.' + e.ExchangeLookup.ContractDate;
            
            PATStoProxyFormat data = new PATStoProxyFormat();
            data.SymbolInfo = e.ExchangeLookup;
            data.Bid = e.PriceUpdateObject.Bid;
            data.Offer = e.PriceUpdateObject.Offer;
            data.BidDOM0 = e.PriceUpdateObject.BidDOM0;
            data.BidDOM1 = e.PriceUpdateObject.BidDOM1;
            data.BidDOM2 = e.PriceUpdateObject.BidDOM2;
            data.BidDOM3 = e.PriceUpdateObject.BidDOM3;
            data.BidDOM4 = e.PriceUpdateObject.BidDOM4;
            data.BidDOM5 = e.PriceUpdateObject.BidDOM5;
            data.BidDOM6 = e.PriceUpdateObject.BidDOM6;
            data.BidDOM7 = e.PriceUpdateObject.BidDOM7;
            data.BidDOM8 = e.PriceUpdateObject.BidDOM8;
            data.BidDOM9 = e.PriceUpdateObject.BidDOM9;
            data.High = e.PriceUpdateObject.High;
            data.Low = e.PriceUpdateObject.Low;
            data.Last = e.PriceUpdateObject.Last0;
            data.OfferDOM0 = e.PriceUpdateObject.OfferDOM0;
            data.OfferDOM1 = e.PriceUpdateObject.OfferDOM1;
            data.OfferDOM2 = e.PriceUpdateObject.OfferDOM2;
            data.OfferDOM3 = e.PriceUpdateObject.OfferDOM3;
            data.OfferDOM4 = e.PriceUpdateObject.OfferDOM4;
            data.OfferDOM5 = e.PriceUpdateObject.OfferDOM5;
            data.OfferDOM6 = e.PriceUpdateObject.OfferDOM6;
            data.OfferDOM7 = e.PriceUpdateObject.OfferDOM7;
            data.OfferDOM8 = e.PriceUpdateObject.OfferDOM8;
            data.OfferDOM9 = e.PriceUpdateObject.OfferDOM9;
            data.Total = e.PriceUpdateObject.Total;

            byte[] toBytes = PriceStructGetBytes(data);
            string prefix = string.Format("6#1#{0}#", HashKey);
            _socket.SendMoreFrame(prefix).SendFrame(toBytes);
            
            //App.Current.Dispatcher.Invoke((Action)(() =>
            //{
            //    if(testData.ContainsKey(HashKey))
            //    {
            //        testData[HashKey]=e.PriceUpdateObject.ToJson();
            //    }
            //    else
            //    testData.Add(HashKey, e.PriceUpdateObject.ToJson());
            //}));
            //BitConverter.GetBytes(e.PriceUpdateObject.Bid.Price);

            //string prefix2 = string.Format("6#1#{0}#", HashKey);
            //_socket.SendMoreFrame(prefix2).SendFrame(GetBytes(e.PriceUpdateObject));
        }
        /// <summary>
        /// 轉成Bytes
        /// </summary>
        private byte[] PriceStructGetBytes(PATStoProxyFormat data)
        {
            int size = Marshal.SizeOf(data);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
        /// <summary>
        /// 轉成Bytes
        /// </summary>
        private byte[] PriceUpdateStructGetBytes(PriceUpdateStruct data)
        {
            int size = Marshal.SizeOf(data);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(data, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }

        #endregion

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
