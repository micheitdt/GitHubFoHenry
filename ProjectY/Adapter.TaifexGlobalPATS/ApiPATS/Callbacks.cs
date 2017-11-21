using Adapter.TaifexGlobalPATS.Properties;
using Adapter.TaifexGlobalPATS.TradingObjects;
using CommonLibrary;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    public partial class ClientAPI
    {
        private partial class Callbacks
        {
            private LinkStateChangeAddr hostLinkStateChangeDelegate;
            private LinkStateChangeAddr priceLinkStateChangeDelegate;
            private BasicCallbackAddr dataDLCompleteDelegate;
            private BasicCallbackAddr logonStatusDelegate;
            private BasicCallbackAddr forcedLogoutDelegate;
            private CommodityProcAddr commodityUpdateDelegate;
            private PriceDomProcAddr priceDomUpdateDelegate;
            private OrderUpdateProcAddr orderUpdateDelegate;
            private PriceProcAddr priceUpdateDelegate;
            private FillStructAddr fillStructDelegate;
            private TickerUpdateProcAddr tickerStructDelegate;
            private ContractUpdateProcAddr contractUpdateDelegate;
            private AtBestProcAddr atBestUpdateDelegate;
            private MsgIDProcAddr msgIDDelegate;
            
            private readonly ClientAPI owner;
            private readonly CallbackList callbackList;
            private readonly Thread callbackThread;
            private static readonly AutoResetEvent syncEvent = new AutoResetEvent(false);
            private static bool terminate;

            public Callbacks(ClientAPI owner)
            {
                this.owner = owner;
                callbackList = new CallbackList();
                callbackThread = new Thread(callbackThreadProc) { IsBackground = true };
                callbackThread.Start(this);
                RegisterAllCallbacks();
            }

            public void Terminate()
            {
                terminate = true;
                syncEvent.Set();
            }

            private void RegisterAllCallbacks()
            {
                hostLinkStateChangeDelegate = cbHostLinkStateChange;
                ClientAPIMethods.DoRegisterLinkStateCallback(Constants.ptHostLinkStateChange, hostLinkStateChangeDelegate);

                priceLinkStateChangeDelegate = cbPriceLinkStateChange;
                ClientAPIMethods.DoRegisterLinkStateCallback(Constants.ptPriceLinkStateChange, priceLinkStateChangeDelegate);

                dataDLCompleteDelegate = cbDownloadComplete;
                ClientAPIMethods.DoRegisterCallback(Constants.ptDataDLComplete, dataDLCompleteDelegate);

                logonStatusDelegate = cbLogonStatus;
                ClientAPIMethods.DoRegisterCallback(Constants.ptLogonStatus, logonStatusDelegate);

                priceUpdateDelegate = cbPriceUpdate;
                ClientAPIMethods.DoRegisterPriceCallback(Constants.ptPriceUpdate, priceUpdateDelegate);

                priceDomUpdateDelegate = cbPriceDOMUpdate;
                ClientAPIMethods.DoRegisterDOMCallback(Constants.ptDOMUpdate, priceDomUpdateDelegate);

                orderUpdateDelegate = cbOrderUpdate;
                ClientAPIMethods.DoRegisterOrderCallback(Constants.ptOrder, orderUpdateDelegate);
                
                forcedLogoutDelegate = cbForceLogout;
                ClientAPIMethods.DoRegisterCallback(Constants.ptForcedLogout, forcedLogoutDelegate);

                commodityUpdateDelegate = cbCommodityUpdate;
                ClientAPIMethods.DoRegisterCommodityCallback(Constants.ptCommodityUpdate, commodityUpdateDelegate);

                fillStructDelegate = cbFillUpdate;
                ClientAPIMethods.DoRegisterFillCallback(Constants.ptFill, fillStructDelegate);

                tickerStructDelegate = cbTickerUpdate;
                ClientAPIMethods.DoRegisterTickerCallback(Constants.ptTickerUpdate, tickerStructDelegate);

                contractUpdateDelegate = cbContractUpdate;
                ClientAPIMethods.DoRegisterContractCallback(Constants.ptContractDateUpdate, contractUpdateDelegate);

                atBestUpdateDelegate = cbAtBestUpdate;
                ClientAPIMethods.DoRegisterAtBestCallback(Constants.ptAtBestUpdate, atBestUpdateDelegate);

                msgIDDelegate = cbMessage;
                ClientAPIMethods.DoRegisterMsgCallback(Constants.ptMessage, msgIDDelegate);
            }

            private void cbMessage(ref MsgIDStruct MsgID)
            {
                var cbitem = new callbackItem(Constants.ptMessage, MsgID);
                callbackList.put(cbitem);
                syncEvent.Set();
            }

            private void cbAtBestUpdate(ref AtBestUpdStruct atBestUpdate)
            {
                var cbitem = new callbackItem(Constants.ptAtBestUpdate, atBestUpdate);
                callbackList.put(cbitem);
                syncEvent.Set();
            }

            /** the main thread for callbacks which processes the callback queue **/
            private static async void callbackThreadProc(object data)
            {
                try
                {
                    var _callbacks = (Callbacks)data;

                    while (!terminate)
                    {
                        callbackItem cbItem = _callbacks.callbackList.getNextCallback();

                        while (cbItem != null)
                        {
                            if (terminate)
                            {
                                break;
                            }
                            switch (cbItem.callbackID)
                            {
                                case Constants.ptHostLinkStateChange:
                                    _callbacks.ProcessHostLinkStateChangeCB((LinkStateStruct)cbItem.data);
                                    await Task.Delay(500);
                                    break;
                                case Constants.ptPriceLinkStateChange: _callbacks.ProcessPriceLinkStateChangeCB((LinkStateStruct)cbItem.data); break;
                                case Constants.ptLogonStatus: _callbacks.ProcessLogonStatusCB(); break;
                                case Constants.ptMessage: _callbacks.ProcessMessage((MsgIDStruct)cbItem.data); break;
                                case Constants.ptOrder: _callbacks.ProcessOrderUpdate((OrderUpdateStruct)cbItem.data); break;
                                case Constants.ptForcedLogout: _callbacks.ProcessForcedLogoutCB(); break;
                                case Constants.ptDataDLComplete: _callbacks.ProcessDownloadCompleteCB(); break;
                                case Constants.ptCommodityUpdate: _callbacks.ProcessCommodityUpdate((CommodityUpdStruct)cbItem.data); break;
                                case Constants.ptPriceUpdate: _callbacks.ProcessPriceUpdate((PriceUpdateStruct)cbItem.data); break;
                                case Constants.ptFill: _callbacks.ProcessFillUpdate((FillUpdateStruct)cbItem.data); break;
                                case Constants.ptContractAdded: _callbacks.ProcessContractAddedCB(); break;
                                case Constants.ptAtBestUpdate: _callbacks.ProcessAtBestUpdate((AtBestUpdStruct)cbItem.data); break;
                                case Constants.ptTickerUpdate: _callbacks.ProcessTickerPrice((TickerUpdStruct)cbItem.data); break;
                                case Constants.ptDOMUpdate: _callbacks.ProcessDOMUpdate((PriceUpdateStruct)cbItem.data); break;
                                case Constants.ptContractDateUpdate: _callbacks.ProcessContractUpdate(); break;
                            }
                            cbItem = _callbacks.callbackList.getNextCallback();
                        }
                        if (!terminate)
                        {
                            syncEvent.WaitOne();
                        }
                    }
                }
                catch(Exception )
                {
                }
            }


            private void ProcessContractUpdate()
            {
                Console.WriteLine(Resources.Callbacks_ProcessContractUpdate_Contract_Update);
            }

            private void ProcessTickerPrice(TickerUpdStruct tickerUpdStruct)
            {
                try
                {
                    owner.putTickerObject(tickerUpdStruct);
                }
                catch (Exception e)
                {
                    Console.WriteLine(Resources.Callbacks_ProcessTickerPrice_Ticker_Price_Exception_Thrown__0__, e);
                }
            }

            private void ProcessAtBestUpdate(AtBestUpdStruct atBestUpdStruct)
            {
                throw new Exception("We are not supposed to be here");
            }

            private void ProcessMessage(MsgIDStruct MsgID)
            {
                MessageStruct message = new MessageStruct();
                if (ClientAPIMethods.DoGetUsrMsgByID(MsgID.MsgID, ref message))
                {
                    owner.PutMessage(message);
                }
            }

            private void ProcessFillUpdate(FillUpdateStruct fillStruct)
            {
                try
                {
                    var filldata = new FillStruct();
                    ClientAPIMethods.DoGetFillByID(fillStruct.FillID, ref filldata);
                    owner.PutFillObject(filldata);
                }
                catch (Exception )
                {
                }
            }

            private void ProcessCommodityUpdate(CommodityUpdStruct commodityUpdStruct)
            {
                Console.WriteLine(commodityUpdStruct.CommodityName);
            }

            private void ProcessDOMUpdate(PriceUpdateStruct data)
            {
                var price = new PriceStruct();
                ClientAPIMethods.DoGetPriceForContract(data, ref price);
                owner.PutDomUpdate(price, data);
                Console.WriteLine(Resources.Callbacks_ProcessDOMUpdate__0___1___2_, data.ExchangeName, data.CommodityName, data.ContractDate);
            }

            private void ProcessContractAddedCB()
            {

            }

            private void ProcessOrderUpdate(OrderUpdateStruct orderUpdate)
            {
                try
                {
                    if (orderUpdate.OrderID != "")
                    {
                        var orderstruct = new OrderDetailStruct();
                        ClientAPIMethods.DoGetOrderByID(orderUpdate.OrderID, ref orderstruct);
                        owner.PutTraderHistoryObject(orderstruct);
                    }
                }
                catch (Exception )
                {
                    //System.Windows.Forms.MessageBox.Show(e.Message);
                }
            }

            /** takes in the callback id and returns the callback name*/
            private string callbackName(int callbackID)
            {
                switch (callbackID)
                {
                    case Constants.ptHostLinkStateChange: return "ptHostLinkStateChange";
                    case Constants.ptPriceLinkStateChange: return "ptPriceLinkStateChange";
                    case Constants.ptLogonStatus: return "ptLogonStatus";
                    case Constants.ptMessage: return "ptMessage";
                    case Constants.ptOrder: return "ptOrder";
                    case Constants.ptForcedLogout: return "ptForcedLogout";
                    case Constants.ptDataDLComplete: return "ptDataDLComplete";
                    case Constants.ptPriceUpdate: return "ptPriceUpdate";
                    case Constants.ptFill: return "ptFill";
                    case Constants.ptStatusChange: return "ptStatusChange";
                    case Constants.ptContractAdded: return "ptContractAdded";
                    case Constants.ptContractDeleted: return "ptContractDeleted";
                    case Constants.ptExchangeRate: return "ptExchangeRate";
                    case Constants.ptConnectivityStatus: return "ptConnectivityStatus";
                    case Constants.ptOrderCancelFailure: return "ptOrderCancelFailure";
                    case Constants.ptAtBestUpdate: return "ptAtBestUpdate";
                    case Constants.ptTickerUpdate: return "ptTickerUpdate";
                    case Constants.ptMemoryWarning: return "ptMemoryWarning";
                    case Constants.ptSubscriberDepthUpdate: return "ptSubscriberDepthUpdate";
                    case Constants.ptVTSCallback: return "ptVTSCallback";
                    case Constants.ptDOMUpdate: return "ptDOMUpdate";
                    case Constants.ptSettlementCallback: return "ptSettlementCallback";
                    case Constants.ptStrategyCreateSuccess: return "ptStrategyCreateSuccess";
                    case Constants.ptStrategyCreateFailure: return "ptStrategyCreateFailure";
                    case Constants.ptAmendFailureCallback: return "ptAmendFailureCallback";
                    case Constants.ptGenericPriceUpdate: return "ptGenericPriceUpdate";
                    case Constants.ptBlankPrice: return "ptBlankPrice";
                    case Constants.ptOrderSentFailure: return "ptOrderSentFailure";
                    case Constants.ptOrderQueuedFailure: return "ptOrderQueuedFailure";
                    case Constants.ptOrderBookReset: return "ptOrderBookReset";
                    default: return "unrecognized callback";
                }
            }

            #region Here are the callback method entry points from the Delphi API

            private void cbHostLinkStateChange(ref LinkStateStruct data)
            {
                var cbItem = new callbackItem(Constants.ptHostLinkStateChange, data);
                callbackList.put(cbItem);
                syncEvent.Set();
            }

            private void cbDownloadComplete()
            {
                var cbItem = new callbackItem(Constants.ptDataDLComplete, null);
                callbackList.put(cbItem);
                syncEvent.Set();
            }

            private void cbPriceLinkStateChange(ref LinkStateStruct data)
            {
                var cbItem = new callbackItem(Constants.ptPriceLinkStateChange, data);
                callbackList.put(cbItem);
                syncEvent.Set();
            }

            private void cbPriceDOMUpdate(ref PriceUpdateStruct data)
            {
                var cbItem = new callbackItem(Constants.ptDOMUpdate, data);
                callbackList.put(cbItem);
                syncEvent.Set();
            }

            private void cbOrderUpdate(ref OrderUpdateStruct data)
            {
                var cbItem = new callbackItem(Constants.ptOrder, data);
                callbackList.put(cbItem);
                syncEvent.Set();
            }

            private void cbLogonStatus()
            {
                var cbItem = new callbackItem(Constants.ptLogonStatus, null);
                callbackList.put(cbItem);
                syncEvent.Set();
            }

            private void cbContractUpdate(ref ContractUpdStruct data)
            {
                var cbitem = new callbackItem(Constants.ptContractDateUpdate, data);
                callbackList.put(cbitem);
                syncEvent.Set();
            }

            private void cbTickerUpdate(ref TickerUpdStruct ticker)
            {
                var cbitem = new callbackItem(Constants.ptTickerUpdate, ticker);
                callbackList.put(cbitem);
                syncEvent.Set();
            }

            private void cbCommodityUpdate(ref CommodityUpdStruct commodity)
            {
                var cbitem = new callbackItem(Constants.ptCommodityUpdate, commodity);
                callbackList.put(cbitem);
                syncEvent.Set();
            }

            private void cbFillUpdate(ref FillUpdateStruct fill)
            {
                var cbitem = new callbackItem(Constants.ptFill, fill);
                callbackList.put(cbitem);
                syncEvent.Set();
            }

            private void cbForceLogout()
            {
                var cbItem = new callbackItem(Constants.ptForcedLogout, null);
                callbackList.put(cbItem);
                syncEvent.Set();
            }

            private void cbPriceUpdate(ref PriceUpdateStruct data)
            {
                var cbItem = new callbackItem(Constants.ptPriceUpdate, data);
                callbackList.put(cbItem);
                syncEvent.Set();
            }
            #endregion

            #region Here are the methods for processing the callbacks stored on the callbacklist and executed on the callback thread
            
            private void ProcessHostLinkStateChangeCB(LinkStateStruct data)
            {
                owner.listeners.hostLinkStatusListener.Trigger(data);
            }

            private void ProcessPriceLinkStateChangeCB(LinkStateStruct data)
            {
                owner.listeners.priceLinkStatusListener.Trigger(data);
            }

            private void ProcessLogonStatusCB()
            {
                ClientAPIMethods.DoGetLogonStatus();
            }

            private void ProcessForcedLogoutCB()
            {
                owner.DestroyClientAPI();
            }

            private void ProcessDownloadCompleteCB()
            {
                ClientAPIMethods.DoUnLockUpdates();
                owner.listeners.downloadCompleteListener.Trigger();
            }

            private void ProcessPriceUpdate(PriceUpdateStruct exchangedata)
            {
                var pricedetailstruct = new PriceStruct();
                ClientAPIMethods.DoGetPriceForContract(exchangedata, ref pricedetailstruct);
                owner.PutTraderPriceUpdateObject(pricedetailstruct, exchangedata);
            }
            #endregion
        }
    }
}
