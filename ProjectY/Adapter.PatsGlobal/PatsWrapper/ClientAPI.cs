using System;
using System.Globalization;
using System.Collections;
using System.Diagnostics;

using System.IO;

namespace Adapter.PatsGlobal.PatsWrapper
{
    public enum LoaderEnum
    {
        leTraderAccount,
        leLogin,
        leExchange,
        leContracts,
        leOrderTypes,
        leCommodity,
    }

    public partial class ClientAPI : LogonStatusListenerInterface, HostLinkStatusListenerInterface, PriceLinkStatusListenerInterface, DownloadCompleteListenerInterface
    {
        private static volatile ClientAPI _instance;
        private static readonly object _syncRoot = new Object();

        public event EventHandler<EventDelegateArgs> CommsMessage = delegate { };
        public event EventHandler<EventDelegateOrderDetailArgs> OrderDetailEvent = delegate { };
        public event EventHandler<EventDelegateTickerArgs> TickDetailEvent = delegate { };
        public event EventHandler<EventDelegatePriceUpdateArgs> PriceDetailEvent = delegate { };
        public event EventHandler<SslEventArgs> SSLStatusEvent = delegate { };
        public event EventHandler<EventConnectionDelegateArgs> ConnectionStatus = delegate { };
        public event EventHandler<EventDelegateFillDetailArgs> FillUpdateEvent = delegate { };
        //public event EventHandler<EventArgs> DummyPriceUpdate = delegate {};
        public event EventHandler<EventDelegatePriceUpdateArgs> PriceDOMDetailEvent = delegate { };
        public event EventHandler<EventDelegateMessageArgs> MessageEvent = delegate { };
        public event EventHandler LogonComplete = delegate { };
        public event EventHandler PriceLinkStatusEvent = delegate { };

        private const bool isSSL = true;
        public Listeners listeners;
        private Callbacks callbacks;
        public string DefaultTraderAccount { get; private set; }

        private Hashtable exchanges;
        private Hashtable traderAccounts;
        private Hashtable traderOrders;
        private Hashtable traderOrderHistory;

        public static ClientAPI Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new ClientAPI();
                        }
                    }
                }
                return _instance;
            }
        }

        private ClientAPI()
        {
            Init();
        }

        public void Start()
        {
            try
            {
                //isdemo
                if (false)
                {
                    //ClientAPIMethods.DoInitialise();
                    //ClientAPIMethods.DoSetHostAddress("", "");
                    //ClientAPIMethods.DoSetPriceAddress("", "");
                }
                else
                {
                    //初始化
                    bool t1 = ClientAPIMethods.DoInitialise(true, "", "", DefaultSettings.Instance.PATS_APPID, DefaultSettings.Instance.PATS_LICENSE);
                    bool t2 = ClientAPIMethods.DoSetHostAddress(DefaultSettings.Instance.PATS_HOST_IP, DefaultSettings.Instance.PATS_HOST_PORT);
                    bool t3 = ClientAPIMethods.DoSetPriceAddress(DefaultSettings.Instance.PATS_PRICE_IP, DefaultSettings.Instance.PATS_PRICE_PORT);
                }

                callbacks = new Callbacks(this);

                //bool test1 = ClientAPIMethods.DoEnable(255);
                //bool test1 = ClientAPIMethods.DoEnable(2);//255全部；2抓價格log
                //bool test2 = ClientAPIMethods.DoSetMemoryWarning(1);//設記憶體上限警告
                bool test3 = ClientAPIMethods.DoReady();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        /** initialises the clientAPI object, this involves creating objects that will be 
         * need throughout the execution of this App**/
        private void Init()
        {
            //create a log file to store all method calls and errors
            listeners = new Listeners(); // create the class which will reference all listeners
            exchanges = new Hashtable();
            traderAccounts = new Hashtable();
            traderOrders = new Hashtable();
            traderOrderHistory = new Hashtable();
        }

        /** Stops the callback thread, logs off the user and closes the app **/
        public void DestroyClientAPI()
        {
            //Application.Exit();
        }

        /** returns the default trader account received from the Delphi API **/
        public string GetDefaultTraderAccount()
        {
            return DefaultTraderAccount;
        }

        public IDictionaryEnumerator GetTraderAccountEnumerator()
        {
            return traderAccounts.GetEnumerator();
        }

        /** Adds a trader account object to the hash table**/

        private void PutTraderAccount(TraderAccount traderAcccount)
        {
            traderAccounts.Add(traderAcccount.GetKey(), traderAcccount);
        }

        /** returns the exchange object referenced by the key provided **/

        private Exchange GetExchange(string exchangeKey)
        {
            return (Exchange)exchanges[exchangeKey];
        }

        /** adds an exchange object to the hash table **/

        private void PutExchange(Exchange exchange)
        {
            exchanges.Add(exchange.GetKey(), exchange);
        }

        /** returns the enumerator for the exchanges **/

        public IDictionaryEnumerator GetExchangesEnumerator()
        {

            return exchanges.GetEnumerator();
        }

        /** Entry point from the LogonStatusListener **/
        public void LogonStatusAction(int statusInt, string statusStr, string reason, string defaultTraderAccount)
        {
            ConnectionStatus(null, new EventConnectionDelegateArgs(statusInt));

            //log.Write(Constants.logComment, "LogonStatus = " + statusStr + ". " + reason + ". DefaultTrader Account = " + defaultTraderAccount);

            SSLStatusEvent(true, new SslEventArgs(isSSL));

            if (defaultTraderAccount != "")
            {
                DefaultTraderAccount = defaultTraderAccount;
            }

            if (statusInt == Constants.LogonStatusForcedOut)
            {
                ClientAPIMethods.DoLogoff();
            }
            else if (statusInt != Constants.LogonStatusLogonSucceeded)
            {
                CommsMessage(statusStr, new EventDelegateArgs("Error in logon (" + statusStr + ") " + reason, LoaderEnum.leLogin));
            }
            else
            {
                CommsMessage(this, new EventDelegateArgs("Downloading session data...", 0));
            }
        }

        private void StackTrack()
        {
            var stacktrace = new StackTrace(false);
            Console.WriteLine(" Frame Count : {0}", stacktrace.FrameCount.ToString());
            for (int index = 0; index < stacktrace.FrameCount; index++)
            {
                StackFrame stackframe = stacktrace.GetFrame(index);
                Console.WriteLine("\t {0}", stackframe.GetMethod());
            }
        }

        /** Entry point from the HostLinkStatusListener **/
        public void HostLinkStatusAction(int oldStatus, int newStatus)
        {
            if (newStatus == Constants.LinkStatusConnected)
            {
                listeners.logonStatusListener.AddListener(this);
                listeners.priceLinkStatusListener.addListener(this);
                listeners.downloadCompleteListener.AddListener(this);

                CommsMessage(this, new EventDelegateArgs("Logging in...please wait", LoaderEnum.leLogin));
                //isDemo
                if (false)
                {
                    //ClientAPIMethods.DoLogon("67720103", "21GOTRADE", "", 'Y', 'Y');
                }
                else
                {
                    ClientAPIMethods.DoLogon(DefaultSettings.Instance.PATS_ID, DefaultSettings.Instance.PATS_PASSWORD, "", 'Y', 'Y');
                }
            }
        }

        /** Entry point from the PriceLinkStatusListener **/
        public void PriceLinkStatusAction(int oldStatus, int newStatus)
        {
            PriceLinkStatusEvent(newStatus, new EventArgs());
            //log.Write(Constants.logComment, "Price link changed from " + oldStatus + " to " + newStatus);
        }

        /** Entry point for the DownloadCompleteListener **/
        public void DownloadCompleteAction()
        {
            CommsMessage(this, new EventDelegateArgs("Downloading completed...", 0));

            int traderCount = 0;
            ClientAPIMethods.DoCountTraders(ref traderCount);
            for (int x = 0; x < traderCount; x++)
            {
                CommsMessage(this, new EventDelegateArgs(string.Format("Loading Trader Account {0}", x), LoaderEnum.leTraderAccount));
                //var tas = new TraderAccountStruct();
                //if (ClientAPIMethods.DoGetTrader(x, ref tas))
                //{
                //    PutTraderAccount(new TraderAccount(tas));
                //}
            }

            //get the exchange count and cycle through the exchanges storing them in the hashtable
            int exchangeCount = 0;
            ClientAPIMethods.DoCountExchanges(ref exchangeCount);
            for (int x = 0; x < exchangeCount; x++)
            {
                CommsMessage(this, new EventDelegateArgs(string.Format("Loading Trader Account {0}", x), LoaderEnum.leExchange));
                var exchange = new ExchangeStruct();
                if (ClientAPIMethods.DoGetExchange(x, ref exchange))
                {
                    PutExchange(new Exchange(exchange));
                }
            }

            //collect all the commodities
            int commodityCount = 0;
            ClientAPIMethods.DoCommodityCount(ref commodityCount);
            for (int x = 0; x < commodityCount; x++)
            {
                CommsMessage(this, new EventDelegateArgs(string.Format("Loading Commodity {0}", x), LoaderEnum.leCommodity));
                var cs = new CommodityStruct();
                if (ClientAPIMethods.DoGetCommodity(x, ref cs))
                {
                    Exchange exchange = GetExchange(cs.ExchangeName);
                    if (exchange != null)
                    {
                        exchange.PutCommodity(new Commodity(cs));
                    }
                }
            }

            //collect all the contracts
            int contractCount = 0;
            ClientAPIMethods.DoContractCount(ref contractCount);


            for (int x = 0; x < contractCount; x++)
            {
                CommsMessage(this, new EventDelegateArgs(string.Format("Loading Contracts {0} out of {1}", x, contractCount), LoaderEnum.leContracts));
                var extcon = new ExtendedContractStruct();
                if (ClientAPIMethods.DoGetExtendedContract(x, ref extcon))
                {
                    Exchange exchange = GetExchange(extcon.ExchangeName);
                    if (exchange != null)
                    {
                        Commodity commodity = exchange.GetCommodity(extcon.ContractName);
                        if (commodity != null)
                        {
                            commodity.PutContract(new Contract(extcon));
                        }
                    }
                }
            }

            try
            {
                int orderTypeCount = 0;
                ClientAPIMethods.DoOrderTypeCount(ref orderTypeCount);
                for (int x = 0; x < orderTypeCount; x++)
                {
                    CommsMessage(this, new EventDelegateArgs(string.Format("Loading Order Types {0}", x), LoaderEnum.leOrderTypes));
                    var ordertype = new OrderTypeStruct();
                    var amendOrderTypesArray = new AmendOrderTypesArray();
                    if (ClientAPIMethods.DoGetOrderType(x, ref ordertype, ref amendOrderTypesArray))
                    {
                        Exchange exchange = GetExchange(ordertype.ExchangeName);
                        if (exchange != null)
                        {
                            exchange.PutOrderType(new OrderType(ordertype));
                        }
                    }
                }
            }
            catch (Exception)
            {
                StackTrack();
            }
            LogonComplete(this, new EventArgs());
        }

        public IDictionaryEnumerator GetOrderIDEnumerator()
        {
            return traderOrders.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public TraderOrderID GetOrderByID(string ID)
        {
            foreach (TraderOrderID tr in traderOrders.Values)
            {
                if (tr.GetKey() == ID)
                {
                    //Console.WriteLine(Resources.ClientAPI_GetOrderByID_match_found);
                }
            }
            if (traderOrders[ID] != null)
            {
                System.Windows.MessageBox.Show(traderOrders[ID].ToString());
            }

            if (traderOrders[ID] != null)
            {
                return (TraderOrderID)traderOrders[ID];
            }
            return null;
        }

        /*
                /// <summary>
                /// Callback order update store the order in a hashtable.
                /// </summary>
                /// <param name="traderOrderID"></param>
                internal void PutTraderOrders(TraderOrderID traderOrderID)
                {
                    traderOrders.Add(traderOrderID.GetKey(), traderOrderID);
                }
        */

        private void putTickerObject(TickerUpdStruct tickerstruct)
        {
            try
            {
                if (TickDetailEvent != null)
                {
                    TickDetailEvent(this, new EventDelegateTickerArgs(tickerstruct));
                }
            }
            catch (Exception)
            {
                //Console.WriteLine(Resources.ClientAPI_putTickerObject_Exception_In_Ticker_Price_UpDate__0_, e.Message.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void PutTraderHistoryObject(OrderDetailStruct orderstruct)
        {
            if (!traderOrderHistory.Contains(orderstruct.OrderID + '_' + orderstruct.Status))
            {
                traderOrderHistory.Add(orderstruct.OrderID + '_' + orderstruct.Status, orderstruct);
                OrderDetailEvent(this, new EventDelegateOrderDetailArgs(orderstruct));
            }
        }
        /// <summary>
        ///  Callback fillupdate fired
        /// </summary>
        /// <param name="fillstruct"></param>
        private void PutFillObject(FillStruct fillstruct)
        {
            FillUpdateEvent(this, new EventDelegateFillDetailArgs(fillstruct));
            //DummyPriceUpdate(this, new EventArgs());
        }
        /// <summary>
        ///  Callback priceupdate fired
        /// </summary>
        /// <param name="pricestruct"></param>
        /// <param name="exchange"></param>
        private void PutTraderPriceUpdateObject(PriceStruct pricestruct, PriceUpdateStruct exchange)
        {
            try
            {
                PriceDetailEvent(null, new EventDelegatePriceUpdateArgs(pricestruct, exchange));
            }
            catch (NullReferenceException)
            {
                //Console.WriteLine(Resources.ClientAPI_PutTraderPriceUpdateObject_Null_reference_incountered_in_PutTraderPriceUpdateObject);
            }
        }

        private void PutDomUpdate(PriceStruct pricedata, PriceUpdateStruct exchange)
        {
            try
            {
                PriceDOMDetailEvent(this, new EventDelegatePriceUpdateArgs(pricedata, exchange));
                //Console.WriteLine(Resources.ClientAPI_PutDomUpdate_Depth_Market_Callback_issued);
            }
            catch (NullReferenceException)
            {
                //Console.WriteLine(Resources.ClientAPI_PutDomUpdate_Null_reference_incountered_in_PutDomUpdate);
            }
        }

        private void PutMessage(MessageStruct message)
        {
            MessageEvent(this, new EventDelegateMessageArgs(message));
        }
    }
}
