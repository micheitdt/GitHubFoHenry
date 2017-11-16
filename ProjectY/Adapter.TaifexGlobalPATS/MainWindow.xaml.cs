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
using NLog;

namespace Adapter.TaifexGlobalPATS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Logger
        /// <summary>
        /// 記錄器
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion Logger

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
            //ClientAPI.Instance.TickDetailEvent += Instance_TickDetailEvent;
            //ClientAPI.Instance.PriceDetailEvent += Instance_PriceDetailEvent;
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
            tbMessage.Text += string.Format("PATS_PRICE_IP={0}", DefaultSettings.Instance.PATS_PRICE_IP) + Environment.NewLine;
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

                    //去除無易盛商品訊息
                    if (ESunnyPATSConverterApi.ESunnyPATSMapConverter.Instance.ContainESunnyCommondityNo(exchange.GetKey() + "." + commodity.GetKey()) == false)
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
                    //byte[] toBytes = PriceUpdateStructGetBytes(pricedata);
                    byte[] toBytes = new byte[73];
                    SetBytes(ref toBytes, 0, pricedata.ExchangeName, 11);
                    SetBytes(ref toBytes, 11, pricedata.CommodityName, 11);
                    SetBytes(ref toBytes, 22, pricedata.ContractDate, 51);
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
                //byte[] endtoBytes = PriceUpdateStructGetBytes(new PriceUpdateStruct() { ExchangeName="End" });
                byte[] endtoBytes = new byte[73];
                SetBytes(ref endtoBytes, 0, "End", 11);
                _socket.SendMoreFrame(prefix).SendFrame(endtoBytes);
            }
        }
        Dictionary<string, string> testData = new Dictionary<string, string>();
        
        /// <summary>
        /// 價格資料接收事件
        /// </summary>
        private void clientAPIPriceDetailEvent(object sender, EventDelegatePriceUpdateArgs e)
        {
            //去除無易盛商品訊息
            //if(ESunnyPATSConverterApi.ESunnyPATSMapConverter.Instance.ContainESunnyCommondityNo(e.ExchangeLookup.ExchangeName + "." + e.ExchangeLookup.CommodityName) == false)
            //{
            //    return;
            //}
            //PatsDepthMD data = new PatsDepthMD();
            //bool isTryParse = true;
            //data.LastHH = e.PriceUpdateObject.Last0.Hour;
            //data.LastMM = e.PriceUpdateObject.Last0.Minute;
            //data.LastSS = e.PriceUpdateObject.Last0.Second;
            //data.BidHH = e.PriceUpdateObject.Bid.Hour;
            //data.BidMM = e.PriceUpdateObject.Bid.Minute;
            //data.BidSS = e.PriceUpdateObject.Bid.Second;
            //isTryParse &= double.TryParse(e.PriceUpdateObject.Last0.Price, out data.LastPrice);
            //data.TotalVolume = e.PriceUpdateObject.Total.Volume;
            //isTryParse &= double.TryParse(e.PriceUpdateObject.High.Price, out data.TagHighPrice);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.Low.Price, out data.TagLowPrice);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM0.Price, out data.BidPrice0);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM1.Price, out data.BidPrice1);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM2.Price, out data.BidPrice2);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM3.Price, out data.BidPrice3);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM4.Price, out data.BidPrice4);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM5.Price, out data.BidPrice5);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM6.Price, out data.BidPrice6);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM7.Price, out data.BidPrice7);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM8.Price, out data.BidPrice8);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.BidDOM9.Price, out data.BidPrice9);
            //data.BidVolume0 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume1 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume2 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume3 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume4 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume5 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume6 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume7 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume8 = e.PriceUpdateObject.BidDOM0.Volume;
            //data.BidVolume9 = e.PriceUpdateObject.BidDOM0.Volume;
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM0.Price, out data.OfferPrice0);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM1.Price, out data.OfferPrice1);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM2.Price, out data.OfferPrice2);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM3.Price, out data.OfferPrice3);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM4.Price, out data.OfferPrice4);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM5.Price, out data.OfferPrice5);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM6.Price, out data.OfferPrice6);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM7.Price, out data.OfferPrice7);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM8.Price, out data.OfferPrice8);
            //isTryParse &= double.TryParse(e.PriceUpdateObject.OfferDOM9.Price, out data.OfferPrice9);
            //data.OfferVolume0 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume1 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume2 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume3 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume4 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume5 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume6 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume7 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume8 = e.PriceUpdateObject.OfferDOM0.Volume;
            //data.OfferVolume9 = e.PriceUpdateObject.OfferDOM0.Volume;
            string HashKey = e.ExchangeLookup.ExchangeName + '.' + e.ExchangeLookup.CommodityName + '.' + e.ExchangeLookup.ContractDate;

            byte[] toBytes = new byte[274];//PatsDepthMDGetBytes(data);
            if (GetBytes(e.PriceUpdateObject, ref toBytes) == false)
            {
                _logger.Debug(string.Format("clientAPIPriceDetailEvent() TryParse Error {0}", HashKey));
                return;
            }
            string prefix = string.Format("6#1#{0}#", HashKey);
            _socket.SendMoreFrame(prefix).SendFrame(toBytes);
        }

        public bool GetBytes(PriceStruct md, ref byte[] data)
        {
            bool isTryParse = true;
            double price = 0;
            data[0] = md.Last0.Hour;
            data[1] = md.Last0.Minute;
            data[2] = md.Last0.Second;
            data[3] = md.Bid.Hour;
            data[4] = md.Bid.Minute;
            data[5] = md.Bid.Second;
            isTryParse &= DoubleTryParse(md.Last0.Price, out price);
            SetBytes(ref data, 6, price);
            SetBytes(ref data, 14, md.Total.Volume);
            isTryParse &= DoubleTryParse(md.High.Price, out price);
            SetBytes(ref data, 18, price);
            isTryParse &= DoubleTryParse(md.Low.Price, out price);
            SetBytes(ref data, 26, price);
            isTryParse &= DoubleTryParse(md.Bid.Price, out price);
            SetBytes(ref data, 34, price);
            SetBytes(ref data, 42, md.Bid.Volume);
            isTryParse &= DoubleTryParse(md.Offer.Price, out price);
            SetBytes(ref data, 46, price);
            SetBytes(ref data, 54, md.Offer.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM1.Price, out price);
            SetBytes(ref data, 58, price);
            SetBytes(ref data, 66, md.BidDOM1.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM1.Price, out price);
            SetBytes(ref data, 70, price);
            SetBytes(ref data, 78, md.OfferDOM1.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM2.Price, out price);
            SetBytes(ref data, 82, price);
            SetBytes(ref data, 90, md.BidDOM2.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM2.Price, out price);
            SetBytes(ref data, 94, price);
            SetBytes(ref data, 102, md.OfferDOM2.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM3.Price, out price);
            SetBytes(ref data, 106, price);
            SetBytes(ref data, 114, md.BidDOM3.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM3.Price, out price);
            SetBytes(ref data, 118, price);
            SetBytes(ref data, 126, md.OfferDOM3.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM4.Price, out price);
            SetBytes(ref data, 130, price);
            SetBytes(ref data, 138, md.BidDOM4.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM4.Price, out price);
            SetBytes(ref data, 142, price);
            SetBytes(ref data, 150, md.OfferDOM4.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM5.Price, out price);
            SetBytes(ref data, 154, price);
            SetBytes(ref data, 162, md.BidDOM5.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM5.Price, out price);
            SetBytes(ref data, 166, price);
            SetBytes(ref data, 174, md.OfferDOM5.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM6.Price, out price);
            SetBytes(ref data, 178, price);
            SetBytes(ref data, 186, md.BidDOM6.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM6.Price, out price);
            SetBytes(ref data, 190, price);
            SetBytes(ref data, 198, md.OfferDOM6.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM7.Price, out price);
            SetBytes(ref data, 202, price);
            SetBytes(ref data, 210, md.BidDOM7.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM7.Price, out price);
            SetBytes(ref data, 214, price);
            SetBytes(ref data, 222, md.OfferDOM7.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM8.Price, out price);
            SetBytes(ref data, 226, price);
            SetBytes(ref data, 234, md.BidDOM8.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM8.Price, out price);
            SetBytes(ref data, 238, price);
            SetBytes(ref data, 246, md.OfferDOM8.Volume);
            isTryParse &= DoubleTryParse(md.BidDOM9.Price, out price);
            SetBytes(ref data, 250, price);
            SetBytes(ref data, 258, md.BidDOM9.Volume);
            isTryParse &= DoubleTryParse(md.OfferDOM9.Price, out price);
            SetBytes(ref data, 262, price);
            SetBytes(ref data, 270, md.OfferDOM9.Volume);
            return isTryParse;
        }
        private bool DoubleTryParse(string orgprice ,out double price)
        {
            if(string.IsNullOrEmpty(orgprice))
            {
                price = 0;
                return true;
            }
            return double.TryParse(orgprice, out price);
        }
        /// <summary>
        /// 轉成Bytes
        /// </summary>
        //private byte[] PatsDepthMDGetBytes(PatsDepthMD data)
        //{
        //    int size = Marshal.SizeOf(data);
        //    byte[] arr = new byte[size];

        //    IntPtr ptr = Marshal.AllocHGlobal(size);
        //    Marshal.StructureToPtr(data, ptr, true);
        //    Marshal.Copy(ptr, arr, 0, size);
        //    Marshal.FreeHGlobal(ptr);
        //    return arr;
        //}
        /// <summary>
        /// 轉成Bytes
        /// </summary>
        //private byte[] PriceUpdateStructGetBytes(PriceUpdateStruct data)
        //{
        //    int size = Marshal.SizeOf(data);
        //    byte[] arr = new byte[size];

        //    IntPtr ptr = Marshal.AllocHGlobal(size);
        //    Marshal.StructureToPtr(data, ptr, true);
        //    Marshal.Copy(ptr, arr, 0, size);
        //    Marshal.FreeHGlobal(ptr);
        //    return arr;
        //}

        public static void SetBytes(ref byte[] data, int offset, double value)
        {
            var array = BitConverter.GetBytes(value);
            for (int i = 0; i < array.Length; i++)
            {
                data[offset + i] = array[i];
            }
        }

        public static void SetBytes(ref byte[] data, int offset, string value, int lengthOfValue)
        {
            var content = value.PadLeft(lengthOfValue);
            var array = Encoding.UTF8.GetBytes(content);
            for (int i = 0; i < array.Length; i++)
            {
                data[offset + i] = array[i];
            }
        }

        public static void SetBytes(ref byte[] data, int offset, int value)
        {
            data[offset] = (byte)value;
            data[offset + 1] = (byte)(value >> 8);
            data[offset + 2] = (byte)(value >> 16);
            data[offset + 3] = (byte)(value >> 24);
        }

        #endregion

        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }
    }
}
