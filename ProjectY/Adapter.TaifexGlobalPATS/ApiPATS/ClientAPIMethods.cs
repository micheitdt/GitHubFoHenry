using Adapter.TaifexGlobalPATS.TradingObjects;
using CommonLibrary;
using System;
using System.Runtime.InteropServices;

namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    public static class ClientAPIMethods
    {
        #region Delphi API Delegates
        
        /// <summary>
        /// ptInitialise determines which environment the client is going to connect to.
        /// Env = "D"  which will demonstrates order processing and price updates, no TCP/IP network communication.
        /// This setting uses local reference data only.
        /// Env = "T"  The client will attempt to establish a TCP/IP connection to our test server. Credentials are
        /// provided by GSD.
        /// </summary>
        /// <param name="Env"></param>
        /// <param name="APIversion"></param>
        /// <param name="ApplicID"></param>
        /// <param name="ApplicVersion"></param>
        /// <param name="License"></param>
        /// <param name="InitReset"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptInitialise([MarshalAs(UnmanagedType.U1)] char Env,
                                              [MarshalAs(UnmanagedType.LPStr)] string APIversion,
                                              [MarshalAs(UnmanagedType.LPStr)] string ApplicID,
                                              [MarshalAs(UnmanagedType.LPStr)] string ApplicVersion,
                                              [MarshalAs(UnmanagedType.LPStr)] string License,
                                              [MarshalAs(UnmanagedType.U1)] bool InitReset);

        /// <summary>
        /// ptGetErrorMessage will provide a human readable description of the error message.
        /// </summary>
        /// <param name="Error"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        [return: MarshalAs(UnmanagedType.LPStr)]
        private static extern string ptGetErrorMessage(int Error);

        /// <summary>
        /// The ptSetHostHandshake routine defines the time between handsakes and the length of wait before the API will assume that connection to the Transaction server has been lost.
        /// Interval Pass the interval in seconds by immediate value. A maximum of 900 seconds is imposed;
        /// Timeout Pass the length of time to wait before connection is assumed to be lost. A minimum of twice the interval is imposed. A maximum of 1800 seconds is imposed.
        /// </summary>
        /// <param name="Interval"></param>
        /// <param name="TimeOut"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetHostHandShake(int Interval, int TimeOut);

        /// <summary>
        /// ptEnable will initiliase a debug trace log, and generate a log file investigation.
        /// Example ptEnable(255) will generate full TCP/IP com`s and order / price processing.
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptEnable(int Code);

        /// <summary>
        /// ptSetHostAddress enter the IPAddress of the host server, this information is provided by our Global Support Team
        /// for more information contact DevZoneSupport@Patsystems.com for registered users only.
        /// Addtional Information:
        /// When calling this method you should also set the host Handshake period "ptSetHostHandShake" this will keep the
        /// connection alive.
        /// </summary>
        /// <param name="IPaddress"></param>
        /// <param name="IPsocket"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetHostAddress([MarshalAs(UnmanagedType.LPStr)] string IPaddress,
                                                   [MarshalAs(UnmanagedType.LPStr)] string IPsocket);

        /// <summary>
        /// ptSetPriceAddress enter the IPAddress of the host server, this information is provided by our Global Support Team
        /// for more information contact DevZoneSupport@Patsystems.com for registered users only.
        /// Addtional Information:
        /// When calling this method you should also set the Price Handshake period "ptSetPriceHandShake" this will keep the
        /// connection alive.
        /// </summary>
        /// <param name="IPaddress"></param>
        /// <param name="IPsocket"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetPriceAddress([MarshalAs(UnmanagedType.LPStr)] string IPaddress,
                                                    [MarshalAs(UnmanagedType.LPStr)] string IPsocket);

        /// <summary>
        /// ptReaady tells the API you are ready to start TCP/IP communication.
        /// </summary>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptReady();

        /// <summary>
        /// ptLogon validate your credentials, if successfully logged in, the delta download will start. 
        /// The download process could take a number of minutes depending on the number of contract you
        /// have assigned to your user-role. Your user-tole is managed by GSD, to amend your user-role and improve your
        /// download performance contact DevZoneSupport@Patsystems.com and provide your application ID with your request.
        /// </summary>
        /// <param name="LogonDetails"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptLogOn(ref LogonStruct LogonDetails);

        /// <summary>
        /// ptLogonStatus reports the current state of your connection, refer to the ADK for a more detailed description.
        /// </summary>
        /// <param name="LogonStatus"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetLogonStatus(ref LogonStatusStruct LogonStatus);

        /// <summary>
        /// ptLogOff shutdowns the API
        /// </summary>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptLogOff();

        /// <summary>
        /// ptTraderAccount returns the number of trader account allocated in the API session 
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCountTraders(ref int Count);

        /// <summary>
        /// ptGetTrader will return the trader name based on the index, this method is normally used 
        /// inconjunction with ptCountTrader method.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="TraderDetails"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetTrader(int Index, ref TraderAccountStruct TraderDetails);

        /// <summary>
        /// ptCountExchanges returns the number of exchanges available within the API session
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCountExchanges(ref int Count);

        /// <summary>
        /// ptGetExchange returns the name of the exchange based on the index, this method is used inconjunction
        /// with ptCountExchanges.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="ExchangeDetails"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetExchange(int Index, ref ExchangeStruct ExchangeDetails);

        /// <summary>
        /// ptCountCommodities return the number of commodities available within the API session.
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCountCommodities(ref int Count);

        /// <summary>
        /// ptGetCommodities returns the name of the commodity based on the index, this method is used inconjunction
        /// with ptCountCommodities
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Commodity"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetCommodity(int Index, ref CommodityStruct Commodity);

        /// <summary>
        /// The ptCountContracts routine returns the total number of contracts (a.k.a. “contract dates? 
        /// known to the API at the time. The returned count may be used to control a loop to read all of the contracts.
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCountContracts(ref int Count);

        /// <summary>
        /// ptGetContract returns the contract name based on the index, this method is used inconjuction
        /// with ptGetContract.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="Contract"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetContract(int Index, ref ContractStruct Contract);

        /// <summary>
        /// ptGetContract returns the contract based on the index, this method is used inconjunction
        /// with ptCountContracts.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="ExtContract"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetExtendedContract(int Index, ref ExtendedContractStruct ExtContract);

        /// <summary>
        /// ptSetHandShakePeriod keeps the connection alive between the API and the host server. 
        /// </summary>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetHandShakePeriod(int Seconds);

         /// <summary>
         /// ptCountOrderTypes return the total number of order types available within the API session.
         /// </summary>
         /// <param name="Count"></param>
         /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCountOrderTypes(ref int Count);

        /// <summary>
        /// ptGetOrderTypes returns the order type structure, which contains the order type information,
        /// used inconjection with ptCountOrderTypes.
        /// </summary>
        /// <param name="Index"></param>
        /// <param name="OrderTypeRec"></param>
        /// <param name="AmendOrderTypes"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetOrderType(int Index, ref OrderTypeStruct OrderTypeRec, ref AmendOrderTypesArray AmendOrderTypes);

        /// <summary>
        /// ptSubscribePrice tells the API to report any price changes which include composite prices, this does not 
        /// include depth of market. registration of the method ptRegisterPriceCallBack is required to receive the updates.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSubscribePrice([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                   [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                   [MarshalAs(UnmanagedType.LPStr)] String ContractDate);

        /// <summary>
        /// ptUnsubscribePrice tells the API to unregister the callback, price updates won`t be received.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptUnsubscribePrice([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                     [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                     [MarshalAs(UnmanagedType.LPStr)] String ContractDate);

        /// <summary>
        /// ptGetpriceForContract returns the most recent price update, which is populated in the price detail
        /// structure.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="Prices"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetPriceForContract([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                        [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                        [MarshalAs(UnmanagedType.LPStr)] String ContractDate,
                                                        ref PriceStruct Prices);

        /// <summary>
        /// ptAddOrder creates an order based on the NewOrderStruct data, and returns the orderid for the newly created
        /// order.
        /// </summary>
        /// <param name="NewOrder"></param>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptAddOrder(ref NewOrderStruct NewOrder, ref OrderIDStruct OrderID);

        /// <summary>
        /// ptGetPrice returns the most recent price and depth of market if available.
        /// And used inconjunction with ptRegisterDOMCallBack for client side updates.
        /// Addtional Information:
        /// The price detailed struct is used for both depth of market and price updates, only some
        /// fields are filled depending on which message have been recived from the price server.
        /// </summary>
        /// <param name="Index"></param>
        /// /// <param name="Prices"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetPrice(int Index, ref PriceStruct Prices);

        /// <summary>
        /// The ptCancelAll routine submits cancellations for all orders for the specified trader account, in any contract.
        /// Completion of the routine does not imply that the cancellations have been successful, just that the cancels have been submitted to the host.
        /// Cancels for orders nearest to the market are submitted first (by comparing limit price to current last traded price). 
        ///If the application has made a call to ptSetUserIDFilter to enable filtering, a call to ptCancelAll will only cancel orders
        ///that have been entered by the currently logged in user. 
        ///The orders may be cancelled when they are in any of the following states:
        ///ptWorking, ptHeldOrder, ptPartFilled, ptAmendPending, ptUnconfirmedPartFilled. 
        ///Cancels will not be submitted for orders in completed states (such as filled) or in transition states (such as amend pending). 
        ///TraderAccount Address of a string[20] variable containing
        /// </summary>
        /// <param name="TraderAccount"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCancelAll([MarshalAs(UnmanagedType.LPStr)] String TraderAccount);

        /// <summary>
        /// The ptCancelBuys routine submits cancellations for all buy orders for the trader,
        /// For the Exchange-ContractName-ContractDate combination supplied. 
        /// Completion of the routine does not imply that the cancellations have been successful,
        /// just that the cancels have been submitted to the host. Cancels for orders nearest to the market are submitted first
        /// (by comparing limit price to current last traded price). 
        ///If the application has made a call to ptSetUserIDFilter to enable filtering, a call to ptCancelBuys will only cancel buy 
        ///orders that have been entered by the currently logged in user.  The orders may be cancelled when they are in any 
        ///of the following states: ptWorking, ptHeldOrder, ptPartFilled, ptAmendPending, ptUnconfirmedPartFilled. 
        ///Cancels will not be submitted for orders in completed states (such as filled) or in transition states (such as amend pending)
        /// </summary>
        /// <param name="TraderAccount"></param>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCancelBuys([MarshalAs(UnmanagedType.LPStr)] String TraderAccount, 
                                               [MarshalAs(UnmanagedType.LPStr)] String ExchangeName, 
                                               [MarshalAs(UnmanagedType.LPStr)] String ContractName, 
                                               [MarshalAs(UnmanagedType.LPStr)] String ContractDate);

        /// <summary>
        /// ptCancelSells will cancel sells only.
        /// </summary>
        /// <param name="TraderAccount"></param>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCancelSells([MarshalAs(UnmanagedType.LPStr)] String TraderAccount,
                                                [MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                [MarshalAs(UnmanagedType.LPStr)] String ContractDate);

        /// <summary>
        /// OrderID Address of a string[10] variable containing the Patsystems order ID of the order to query. This value is returned by the ptOrder callback and uniquely identifies the order on PATS. Synthetic orders managed by the API have Ids starting eith “S?
        /// OrderDetail Address of a structure of type OrderDetailStruct where the API will write the result. See ptGetOrder for details of OrderDetailStruct.
        /// OFSequence The index of the order update within the list of order updates for that order ID, where 1 is the first order update. Value is defaulted to zero for backwards compatibility.
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="OrderDetailStruct"></param>
        /// <param name="OFSequenceNumber"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetOrderByID([MarshalAs(UnmanagedType.LPStr)] String OrderID, 
                                                 ref OrderDetailStruct OrderDetailStruct, 
                                                 int OFSequenceNumber);

        /// <summary>
        /// ptAmendOrder will amended an existsing order based on the orderid provided
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="NewDetails"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptAmendOrder([MarshalAs(UnmanagedType.LPStr)] string OrderID, 
                                               ref AmendOrderStruct NewDetails);

        /// <summary>
        /// ptSetSSL tells the API that a secure connection is required.
        /// </summary>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetSSL([MarshalAs(UnmanagedType.U1)] char Enabled);

        /// <summary>
        /// ptSetPDDSSL tells the API that a secure price 
        /// </summary>
        /// <param name="Enabled"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetPDDSSL([MarshalAs(UnmanagedType.U1)] char Enabled);

        /// <summary>
        /// ptSetSSLCertificateName sets the certificate name which is provided by the SSL provider
        /// </summary>
        /// <param name="CertName"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetSSLCertificateName([MarshalAs(UnmanagedType.LPStr)] String CertName);

        /// <summary>
        /// ptSetPDDSSLCertificateName sets the certificate name for the price server.
        /// </summary>
        /// <param name="CertName"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetPDDSSLCertificateName([MarshalAs(UnmanagedType.LPStr)] String CertName);

        /// <summary>
        /// ptSetClientAuthName sets the Author name not currently used.
        /// </summary>
        /// <param name="CertName"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetSSLClientAuthName([MarshalAs(UnmanagedType.LPStr)] String CertName);

        /// <summary>
        /// ptSetDDDSSLClientAuthName sets the price server Author Name not currently used.
        /// </summary>
        /// <param name="CertName"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetPDDSSLClientAuthName([MarshalAs(UnmanagedType.LPStr)] String CertName);

        /// <summary>
        /// ptSetSuperTAS tells the API to connect to the super transaction server.
        /// </summary>
        /// <param name="Enabled"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetSuperTAS([MarshalAs(UnmanagedType.U1)] char Enabled);

        /// <summary>
        /// ptgetMarhinPerLot returns the margin allocated to the lot, configuartion is setup
        /// by GSD, for more information on margin configuration contact GSD.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="Margin"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetMarginPerLot([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                    [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                    [MarshalAs(UnmanagedType.LPStr)] String ContractDate,
                                                    [MarshalAs(UnmanagedType.LPStr)] String TraderAccount, 
                                                    ref MarginPerLotStruct Margin);

        /// <summary>
        /// ptLockUpdates tells the API to queue all incoming price and order related messages while the API
        /// complete`s its delta download, used in conjuction with ptUnLockUpdates.
        /// </summary>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptLockUpdates();

        /// <summary>
        /// ptLockUpdates queues the updates received from the STAS. 
        /// This is used to prevent the loss of data if updates are received and the client collects the session
        /// data while more contracts arrive, therefore the extra information is not collected as the client does
        /// not process callbacks until initialisation is complete.
        /// </summary>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptUnlockUpdates();

        /// <summary>
        /// The ptGetOpenPosition routine returns the current open position of the trader for a given contract.
        /// To evaluate this open position as the market moves, the application should call ptGetAveragePrice to obtain the average price of these open fills.
        /// Profit is reported in contract currency.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetOpenPosition([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                    [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                    [MarshalAs(UnmanagedType.LPStr)] String ContractDate,
                                                    [MarshalAs(UnmanagedType.LPStr)] String TraderAccount, 
                                                    ref OpenPositionStruct Position);

        /// <summary>
        /// The ptPLBurnRate routine is used to retrieve the buying power burn rate for a trader account for a given Contract.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="BurnRate"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptPLBurnRate([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                               [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                               [MarshalAs(UnmanagedType.LPStr)] String ContractDate,
                                               [MarshalAs(UnmanagedType.LPStr)] String TraderAccount,
                                               ref PLburnRate BurnRate);

        /// <summary>
        /// The ptTotalMarginPaid routine is used to retrieve the total margin for a trader account or for a trader account on a given Contract.
        /// If no exchange name or contract name or contract date is given, the routine will calculate total margin for the given trader account.
        /// However, if the contract details are specified then total margin will be for the specified account and contract.
        /// TraderAccount parameter is mandatory and must be a valid account name.
        /// To get total margin for a contract, ExchangeName, ContractName and ContractDate must be specified. If any of the three parameters are
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="MarginReqd"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptTotalMarginPaid([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                    [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                    [MarshalAs(UnmanagedType.LPStr)] String ContractDate,
                                                    [MarshalAs(UnmanagedType.LPStr)] String TraderAccount,
                                                    ref MarginPaid MarginReqd);
       
        /// <summary>
        /// The ptGetFillByID routine returns fill details for the fill with the given Fill ID.
        /// This provides an easy mechanism to find the fill details for a fill triggered by the ptFill callback.
        /// The callback will provide a Fill ID, which can be passed to this query function.
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Fill"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetFillByID([MarshalAs(UnmanagedType.LPStr)] String ID, 
                                                ref FillStruct Fill);

        /// <summary>
        /// The ptGetContractAtBest routine returns the appropriate At Best data for the At Best bid and offer price for a given contract.
        /// This can be supplied if and only if the exchange supports At Best price data.
        /// Most exchange interfaces used by Patsystems do not support At Best data.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="AtBest"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetContractAtBestPrices([MarshalAs(UnmanagedType.LPStr)] String ExchangeName,
                                                            [MarshalAs(UnmanagedType.LPStr)] String ContractName,
                                                            [MarshalAs(UnmanagedType.LPStr)] String ContractDate, 
                                                            ref AtBestPriceStruct AtBest);

        /// <summary>
        /// The ptGetAveragePrice routine returns the average price of the open fills for the trader in a given contract. 
        /// This can be used to show how the open profit or loss fluctuates with market movement.
        /// This function is not enabled for gateway applications. 
        /// It is expected that gateway applications will remove orders and fills during processing and this invalidates the position calculation used by this routine.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="Price"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetAveragePrice([MarshalAs(UnmanagedType.LPStr)] string ExchangeName,
                                                    [MarshalAs(UnmanagedType.LPStr)] string ContractName,
                                                    [MarshalAs(UnmanagedType.LPStr)] string ContractDate,
                                                    [MarshalAs(UnmanagedType.LPStr)] string TraderAccount, 
                                                    ref AveragePriceStruct Price);

        /// <summary>
        /// The ptGetCommodityByName routine returns the commodity data for a specified commodity. 
        /// This routine does not require an index to access the data ?it will scan the known commodities until the specified one is matched and then return the data.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="Commodity"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetCommodityByName([MarshalAs(UnmanagedType.LPStr)] string ExchangeName,
                                                       [MarshalAs(UnmanagedType.LPStr)] string ContractName, 
                                                       ref CommodityStruct Commodity);
       
        /// <summary>
        /// The ptGetContractPosition routine returns the current total position of the trader for a given contract.
        /// This includes both the open and closed position. Profit is reported in contract currency.
        /// This function is not enabled for gateway applications.
        /// It is expected that gateway applications will remove orders and fills during processing and this invalidates the position calculation used by this routine.      
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetContractPosition([MarshalAs(UnmanagedType.LPStr)] string ExchangeName,
                                                        [MarshalAs(UnmanagedType.LPStr)] string ContractName,
                                                        [MarshalAs(UnmanagedType.LPStr)] string ContractDate,
                                                        [MarshalAs(UnmanagedType.LPStr)] string TraderAccount, 
                                                        ref PositionStruct Position);
        
        /// <summary>
        /// The ptGetExchangeRate routine returns the exchange rate for converting a currency into the system local currency. 
        /// The local currency is defined by the Host Administrator and is fixed for each transaction server.
        /// </summary>
        /// <param name="Currency"></param>
        /// <param name="ExchRate"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetExchangeRate([MarshalAs(UnmanagedType.LPStr)] string Currency,
                                                    ref ExchangeRateStruct ExchRate);
    
        /// <summary>
        /// The ptBuyingPowerRemaining routine is used to retrieve the buying power remaining for a trader account for a given contract.
        /// If an invalid contract is passed, then the total buying power remaining for the given trader account is returned.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="BPRemaining"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptBuyingPowerRemaining([MarshalAs(UnmanagedType.LPStr)] string ExchangeName,
                                                         [MarshalAs(UnmanagedType.LPStr)] string ContractName,
                                                         [MarshalAs(UnmanagedType.LPStr)] string ContractDate,
                                                         [MarshalAs(UnmanagedType.LPStr)] string TraderAccount,
                                                         ref BPremainingStruct BPRemaining);

        /// <summary>
        /// The ptOpenPositionExposure routine is used to retrieve the buying power exposure for a trader account for a given contract.
        /// If an invalid contract is passed, then the total exposure for the given trader account is returned.
        /// </summary>
        /// <param name="ExchangeName"></param>
        /// <param name="ContractName"></param>
        /// <param name="ContractDate"></param>
        /// <param name="TraderAccount"></param>
        /// <param name="Exposure"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptOpenPositionExposure([MarshalAs(UnmanagedType.LPStr)] string ExchangeName,
                                                         [MarshalAs(UnmanagedType.LPStr)] string ContractName,
                                                         [MarshalAs(UnmanagedType.LPStr)] string ContractDate,
                                                         [MarshalAs(UnmanagedType.LPStr)] string TraderAccount, 
                                                         ref Exposure Exposure);

        /// <summary>
        /// The ptGetTotalPosition routine returns the current total overall position of the trader over all contracts.
        /// This includes the open and closed position. Profit is reported in the system currency.
        /// This function is not enabled for gateway applications. 
        /// It is expected that gateway applications will remove orders and fills during processing and this invalidates the position calculation used by this routine.
        /// </summary>
        /// <param name="TraderAccount"></param>
        /// <param name="Position"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetTotalPosition([MarshalAs(UnmanagedType.LPStr)] string TraderAccount, 
                                                     ref PositionStruct Position);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetTraderByName([MarshalAs(UnmanagedType.LPStr)] string TraderAccount,
                                                    ref TraderAccountStruct TraderDetails);
        
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCountFills(ref int Count);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetFill(int Index, ref FillStruct FillDetails);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetMemoryWarning(int MemAmount);

        /// <summary>
        /// The ptCancelOrder routine submits a cancellation for the specified order. Completion of the routine does not imply that the cancellation has been successful,
        /// just that the cancel has been submitted to the host. This function will return an error code if there is no connection to
        /// a transaction server. The orders may be cancelled when they are in any of the following states: ptWorking, ptHeldOrder, ptPartFilled, ptUnconfirmedPartFilled. 
        /// Cancels will not be submitted for orders in completed states (such as filled) or in transition states (such as amend pending, queued).
        /// The order will transition to ptCancelPending state. Further cancels for this order must not be considered unless the order reverts to one of the working states listed above. Do not submit further cancels for an order already in the pending cancel state
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCancelOrder([MarshalAs(UnmanagedType.LPStr)] string OrderID);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern void ptSetClientPath([MarshalAs(UnmanagedType.LPStr)] string Path);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptCountOrders(ref int Count);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetOrder(int Index, ref OrderDetailStruct OrderDetail);
        
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterTickerCallback(int CallbackID, TickerUpdateProcAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterCallback(int CallbackID, BasicCallbackAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterDOMCallback(int CallbackID, PriceDomProcAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterLinkStateCallback(int CallbackID, LinkStateChangeAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterPriceCallback(int CallbackID, PriceProcAddr CBackProc);
        
        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterOrderCallback(int CallbackID, OrderUpdateProcAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterCommodityCallback(int CallbackID, CommodityProcAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterFillCallback(int CallbackID, FillStructAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterContractCallback(int CallbackID, ContractUpdateProcAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetConsolidatedPosition([MarshalAs(UnmanagedType.LPStr)] string ExchangeName,
                                                            [MarshalAs(UnmanagedType.LPStr)] string ContractName,
                                                            [MarshalAs(UnmanagedType.LPStr)] string ContractDate,
                                                            [MarshalAs(UnmanagedType.LPStr)] string TraderAccount,
                                                            int PositionType,
                                                            ref FillStruct Fill);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterAtBestCallback(int CallbackID, AtBestProcAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptRegisterMsgCallback(int CallbackID, MsgIDProcAddr CBackProc);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptNotifyAllMessages([MarshalAs(UnmanagedType.U1)] char Enabled);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptGetUsrMsgByID([MarshalAs(UnmanagedType.LPStr)] string MsgID,
                                                  ref MessageStruct UserMsg);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetEncryptionCode([MarshalAs(UnmanagedType.U1)] char Code);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetOrderCancelFailureDelay(int Delay);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetOrderQueuedFailureDelay(int Delay);

        [DllImport("PATSAPI.DLL", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern int ptSetOrderSentFailureDelay(int Delay);

        #endregion
        

        /// <summary>
        /// CheckSucceeded is a generic call that reports if the call was sucessful or failed when processing
        /// within the API.
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="functionName"></param>
        /// <returns></returns>
        private static bool CheckSucceeded(int ret, string functionName)
        {
            try
            {
                if (ret == 0)
                    return true;

                string test = ptGetErrorMessage(ret);

                Console.WriteLine(functionName + " failed to execute because " + ptGetErrorMessage(ret));
                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(" error: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// The DoSetHostHandShake routine defines the time between handsakes and the length of wait before the API will assume that connection to the Transaction server has been lost.
        ///  Interval Pass the interval in seconds by immediate value. A maximum of 900 seconds is imposed;
        ///  Timeout Pass the length of time to wait before connection is assumed to be lost. A minimum of twice the interval
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="timeout"></param>
        public static bool DoSetHostHandShake(int interval, int timeout)
        {
            return CheckSucceeded(ptSetHostHandShake(interval, timeout), "DoSetHostHandShake");
        }

        /// <summary>
        /// DoInitialise method setups the API environment, there are number of modes available 
        /// 'D' = Demo 'T' = Test, by default the enviroment is set to Demo, when the customer
        /// is ready to test for conformance, the environment should be set to 'T' 
        /// </summary>
        public static bool DoInitialise()
        {
            return CheckSucceeded(ptInitialise('D', "v2.8.3", "MyApp", "1.0", "211252", true), "DoInitialise");
        }

        /// <summary>
        /// DoSetHandShakePeriod informs the API to issue a handshake which is a send / recv
        /// this process keeps the connection alive, sometimes called a heart-beat.
        /// </summary>
        /// <param name="seconds"></param>
        public static bool DoSetHandShakePeriod(int seconds)
        {
            return CheckSucceeded(ptSetHandShakePeriod(seconds), "DoSetHandShakePeriod");
        }

        /// <summary>
        /// DoSetSuperTAS tells the API to use the super transaction server.
        /// </summary>
        /// <param name="enabled"></param>
        public static bool DoSetSuperTAS(Char enabled)
        {
            return CheckSucceeded(ptSetSuperTAS(enabled), "DoSetSuperTAS");
        }

        /// <summary>
        /// DoDiagnostic put`s the API in debug mode and will generate a diagnostic log
        /// of processing within the API depending on the bitmask set, for full logging
        /// the butmask is set to 255 an output file is generated.
        /// </summary>
        /// <param name="diagnosticLevel"></param>
        public static bool DoEnable(int diagnosticLevel)
        {
            return CheckSucceeded(ptEnable(diagnosticLevel), "DoDiagnostics");
        }

        /// <summary>
        /// DoSetHostAddress setups the API IP address for the host connection.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static bool DoSetHostAddress(string ip, string port)
        {
            return CheckSucceeded(ptSetHostAddress(ip, port), "DoSetHostAddress");
        }

        /// <summary>
        ///  DoSetPriceAddress setups the API IP address for the price connection.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public static bool DoSetPriceAddress(string ip, string port)
        {
            return CheckSucceeded(ptSetPriceAddress(ip, port), "DoSetPriceAddress");
        }

        /// <summary>
        /// DoReady informs the API the client is ready to start communication and processing.
        /// </summary>
        public static bool DoReady()
        {
            ClientAPI.Instance.listeners.hostLinkStatusListener.addListener(ClientAPI.Instance);
            return CheckSucceeded(ptReady(), "DoReady");
        }

        /// <summary>
        /// DoSetPDDSSLCertificateName only used is secure SSL is required.
        /// </summary>
        /// <param name="certName"></param>
        public static bool DoSetPDDSSLCertificateName(string certName)
        {
            return CheckSucceeded(ptSetPDDSSLCertificateName(certName), "DoSetSSLCertName");
        }

        /// <summary>
        /// DoSetSSLCertName set the certificate name only required on a secure connection.
        /// </summary>
        /// <param name="certName"></param>
        /// <returns></returns>
        public static bool DoSetSSLCertName(string certName)
        {
            return CheckSucceeded(ptSetSSLCertificateName(certName), "DoSetSSLCertName");
        }

        /// <summary>
        /// DoSetSSLClientAuthName only required on a secure connection.
        /// </summary>
        /// <param name="certName"></param>
        public static bool DoSetSSLClientAuthName(string certName)
        {
            return CheckSucceeded(ptSetSSLClientAuthName(certName), "DoSetSSLClientAuthName");
        }

        /// <summary>
        /// DoSetSLL only required on a secure connection.
        /// </summary>
        /// <param name="enabled"></param>
        public static bool DoSetSSL(char enabled)
        {
            return CheckSucceeded(ptSetSSL(enabled), "DoSetSSL");
        }
       
        /// <summary>
        /// DoLogon will attempt to authenticate your credentials provided by GSD, and sign you in if
        /// successful.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="newPassword"></param>
        /// <param name="reports"></param>
        /// <param name="reset"></param>
        public static bool DoLogon(string userName, string password, string newPassword, char reports, char reset)
        {
            var logon = new LogonStruct { UserID = userName, Password = password, Reports = reports, Reset = reset, NewPassword = newPassword };
            return CheckSucceeded(ptLogOn(ref logon), "DoLogon");
        }

        /// <summary>
        ///  DoGetLogonStatus returns the state of the connection, for example "OPEN" ,"CLOSED"
        /// </summary>
        public static bool DoGetLogonStatus()
        {
            var lss = new LogonStatusStruct();
            bool result = CheckSucceeded(ptGetLogonStatus(ref lss), "DoGetLogonStatus");
            ClientAPI.Instance.listeners.logonStatusListener.Trigger(lss);
            return result;
        }

        /// <summary>
        /// DoLogOff when the client application terminates it should call this method
        /// which shutsdown the API and unloads it from memory.
        /// </summary>
        public static bool DoLogoff()
        {
            return CheckSucceeded(ptLogOff(), "DoLogoff");
        }

        /// <summary>
        /// DoCountExchanges return the number of exchanges available within the API.
        /// </summary>
        /// <returns></returns>
        public static bool DoCountExchanges(ref int count)
        {
            return CheckSucceeded(ptCountExchanges(ref count), "DoCountExchanges");
        }

        /// <summary>
        /// DoGetExchange returns the buffer containing the exchange name.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="exchange"></param>
        public static bool DoGetExchange(int index, ref ExchangeStruct exchange)
        {
            return CheckSucceeded(ptGetExchange(index, ref exchange), "DoGetExchange");
        }

        /// <summary>
        /// DoCommodityCount returns the number of accounts available.
        /// </summary>
        /// <returns></returns>
        public static bool DoCommodityCount(ref int count)
        {
            return CheckSucceeded(ptCountCommodities(ref count), "DoCommodityCount");
        }

        public static bool DoGetCommodityByName(ref PriceUpdateStruct exchangeinfo, ref CommodityStruct commodity)
        {
            return CheckSucceeded(ptGetCommodityByName(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, ref commodity), "DoGetCommodity");
        }

        /// <summary>
        /// DoGetCommodity returns the commodity name based on the index provided.
        /// used in conjuction ptGetCommodityCount
        /// </summary>
        /// <param name="index"></param>
        public static bool DoGetCommodity(int index, ref CommodityStruct commodity)
        {
            return CheckSucceeded(ptGetCommodity(index, ref commodity), "DoGetCommodity");
        }

        /// <summary>
        /// DoContractCount returns the number of contracts available used in conjunction 
        /// with ptgetContract.
        /// </summary>
        /// <returns></returns>
        public static bool DoContractCount(ref int count)
        {
            return CheckSucceeded(ptCountContracts(ref count), "DoContractCount");
        }

        /// <summary>
        /// DoGetExtendedContract returns the contract based on the index provided and 
        /// used in conjunction ptGetContractCount
        /// </summary>
        /// <param name="index"></param>
        /// <param name="contract"></param>
        public static bool DoGetExtendedContract(int index, ref  ExtendedContractStruct contract)
        {
            return CheckSucceeded(ptGetExtendedContract(index, ref contract), "DoGetExtendedContract");
        }

        /// <summary>
        /// DoCountTraders returns the number of traders available within the API session.
        /// </summary>
        /// <returns></returns>
        public static bool DoCountTraders(ref int count)
        {
            return CheckSucceeded(ptCountTraders(ref count), "DoCountTraders");
        }

        /// <summary>
        /// DoGetTrader returns the trader based on the index provided and used in conjunction 
        /// ptGetTraderCount.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="traderDetails"></param>
        /// passes a byte buffer to the Delphi API which populates it with the trader information**/
        public static bool DoGetTrader(int index, ref TraderAccountStruct traderDetails)
        {
            return CheckSucceeded(ptGetTrader(index, ref traderDetails), "DoGetTrader");
        }

        /// <summary>
       /// DoOrderTypeCount returns the number of orders available within the current API session.
       /// </summary>
       /// <returns></returns>
        public static bool DoOrderTypeCount(ref int count)
        {
            return CheckSucceeded(ptCountOrderTypes(ref count), "DoOrderTypeCount");
        }

        /// <summary>
        /// DoGetOrderType returns the order based on the index provided and used in conjunction
        /// ptCountOrderTypes.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="orderTypeRec"></param>
        /// <param name="amendOrderTypes"></param>
        public static bool DoGetOrderType(int index, ref OrderTypeStruct orderTypeRec, ref AmendOrderTypesArray amendOrderTypes)
        {
            return CheckSucceeded(ptGetOrderType(index, ref orderTypeRec, ref amendOrderTypes), "DoGetOrderType");
        }

        /// <summary>
        /// DoSubscribePrice informs the API to issue a composite price update callback message to the client.
        /// a callback method registration required for this function, for example " ptRegisterPriceCallback".
        /// Once the exchange receives a price update the client will receive the update.
        /// </summary>
        /// <returns></returns>
        public static bool DoSubscribePrice(String exchangeName, String contractName, String contractDate)
        {
            return CheckSucceeded(ptSubscribePrice(exchangeName, contractName, contractDate), "DoSubscribePrice");
        }

        /// <summary>
        /// DoUnsubscribePrice will cancel the callback request for a specific contract,
        /// price update messages will be terminated.
        /// </summary>
        public static bool DoUnsubscribePrice(String exchangeName, String contractName, String contractDate)
        {
            return CheckSucceeded(ptUnsubscribePrice(exchangeName, contractName, contractDate), "DoUnsubscribePrice");
        }

        /// <summary>
        /// DoAmendOrder will amend an existing order, if the exchange allows an amendment.
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="newOrder"></param>
        public static bool DoAmendOrder(string orderID, ref AmendOrderStruct newOrder)
        {
            return CheckSucceeded(ptAmendOrder(orderID, ref newOrder), "DoAmendOrder");
        }

        /// <summary>
        /// DoGetPriceForContract will return the most recent price.
        /// </summary>
        /// <param name="exchangeDetails"></param>
        /// <param name="priceDetail"></param>
        public static bool DoGetPriceForContract(PriceUpdateStruct exchangeDetails, ref PriceStruct priceDetail)
        {
            return CheckSucceeded(ptGetPriceForContract(exchangeDetails.ExchangeName, exchangeDetails.CommodityName, exchangeDetails.ContractDate, ref priceDetail), "DoGetPriceForContract");
        }

        /// <summary>
        /// DoGetOrderByID will return the order based on the orderID,can be used in conjunction
        /// ptAmendOrder.
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="orderDetailStruct"></param>
        public static bool DoGetOrderByID(string orderID, ref OrderDetailStruct orderDetailStruct)
        {
            return CheckSucceeded(ptGetOrderByID(orderID, ref orderDetailStruct, 0), "DoGetOrderByID");
        }

        /// <summary>
        /// addorder will generate a buy or sell order.
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="commodity"></param>
        /// <param name="contract"></param>
        /// <param name="orderType"></param>
        /// <param name="account"></param>
        /// <param name="price"></param>
        /// <param name="lots"></param>
        /// <param name="buy"></param>
        /// <param name="active"></param>
        public static bool DoAddOrder(string exchange, string commodity, string contract, string orderType, string account, string price, int lots, bool buy, bool active)
        {
            var ord = new NewOrderStruct
                      {
                          ExchangeName = exchange,
                          ContractName = commodity,
                          TraderAccount = account,
                          ContractDate = contract,
                          OrderType = orderType,
                          Price = price,
                          Price2 = price,
                          Lots = lots,
                          LocalTrader = account
                      };

            var orderid = new OrderIDStruct();

            ord.Inactive = !active ? 'Y' : 'N';

            ord.BuyOrSell = buy ? 'B' : 'S';
            
            return CheckSucceeded(ptAddOrder(ref ord, ref orderid), "DoAddOrder");
        }

        /// <summary>
        /// DoTestInitialise implemented for a direct test access only and implementation for
        /// secure socket layer connectivity.
        /// </summary>
        /// <param name="ssl"></param>
        /// <param name="certname"></param>
        /// <param name="certauthorname"></param>
        public static bool DoInitialise(Boolean ssl, string certname, string certauthorname, string appid="", string license="")
        {
            return CheckSucceeded(ptInitialise('G', "v2.8.3", appid, "", license, true), "DoInitialise");
        }

        /// <summary>
        /// DoCancelALL cancel all orders within a API session,implemented but not used 
        /// </summary>
        /// <param name="traderAccount"></param>
        public static bool DoCancelALL(string traderAccount)
        {
            return CheckSucceeded(ptCancelAll(traderAccount), "DoCancelALL");
        }

        /// <summary>
        /// DoCancelBuySells cancel a specific type of buy or sell order
        /// </summary>
        /// <param name="account"></param>
        /// <param name="exchange"></param>
        /// <param name="commodity"></param>
        /// <param name="contract"></param>
        /// <param name="buyorsell"></param>
        public static bool DoCancelBuySells(string account, string exchange, string commodity, string contract, bool buyorsell)
        {
            return CheckSucceeded(
                buyorsell
                    ? ptCancelBuys(account, exchange, commodity, contract)
                    : ptCancelSells(account, exchange, commodity, contract), "DoCancelBuySells");
        }

        /// <summary>
        /// DoGetMarginPerLot returns the margin per lot, configured by GSD and only required if risk is management is required.
        /// </summary>
        /// <returns></returns>
        public static bool DoGetMarginPerLot(PriceUpdateStruct exchangeinfo, string traderaccount, ref MarginPerLotStruct margin)
        {
            return CheckSucceeded(ptGetMarginPerLot(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, traderaccount, ref margin), "DoGetMarginPerLot");
        }

        /// <summary>
        /// DoLockUpdates tells the API session to lock any incoming messages whilethe download is processing
        /// all queued messages will be released, by calling doUnlockUpdates.
        /// </summary>
        public static bool DoLockUpdates()
        {
            return CheckSucceeded(ptLockUpdates(), "DoLockUpdates");
        }

        /// <summary>
        /// DoUnLockUpdates release any message queued after download has completed.
        /// </summary>
        public static bool DoUnLockUpdates()
        {
            return CheckSucceeded(ptUnlockUpdates(), "DoUnLockUpdates");
        }

        public static bool DoGetAveragePrice(PriceUpdateStruct exchangeInfo, string traderaccount, ref AveragePriceStruct price) 
        {
            return CheckSucceeded(ptGetAveragePrice(exchangeInfo.ExchangeName, exchangeInfo.CommodityName, exchangeInfo.ContractDate, traderaccount, ref price), "DoGetAveragePrice");
        }

        public static bool DoGetOpenPositionExposure(PriceUpdateStruct exchangeinfo, string traderaccount, ref Exposure exposure)
        {
            return CheckSucceeded(ptOpenPositionExposure(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, traderaccount, ref exposure), "DoGetOpenPositionExposure");
        }
        
        /// <summary>
        /// The DoGetPLBurnRate routine is used to retrieve the buying power burn rate for a trader account for a given Contract.
        /// </summary>
        /// <param name="exchangeinfo"></param>
        /// <param name="traderaccount"></param>
        /// <returns></returns>
        public static bool DoGetPLBurnRate(PriceUpdateStruct exchangeinfo, string traderaccount, ref PLburnRate burnRate)
        {
            return CheckSucceeded(ptPLBurnRate(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, traderaccount, ref burnRate), "DoGetPLBurnRate");
        }

        /// <summary>
        /// The DoTotalMarginPaid routine is used to retrieve the total margin for a trader account or for a trader account on a given Contract.
        /// If no exchange name or contract name or contract date is given, the routine will calculate total margin for the given trader account.
        /// However, if the contract details are specified then total margin will be for the specified account and contract.
        /// </summary>
        /// <param name="exchangeinfo"></param>
        /// <param name="traderaccount"></param>
        /// <returns></returns>
        public static bool DoTotalMarginPaid(PriceUpdateStruct exchangeinfo, string traderaccount, ref MarginPaid marginPaid)
        {
            return CheckSucceeded(ptTotalMarginPaid(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, traderaccount, ref marginPaid), "DoTotalMarginPaid");
        }

        /// <summary>
        /// The DoBuyingPowerRemaining routine is used to retrieve the buying power remaining for a trader account for a given contract.
        /// If an invalid contract is passed, then the total buying power remaining for the given trader account is returned.
        /// </summary>
        /// <param name="exchangeinfo"></param>
        /// <param name="traderaccount"></param>
        /// <returns></returns>
        public static bool DoBuyingPowerRemaining(PriceUpdateStruct exchangeinfo, string traderaccount, ref BPremainingStruct bpRemaining)
        {
            return CheckSucceeded(ptBuyingPowerRemaining(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, traderaccount, ref bpRemaining), "DoBuyingPowerRemaining");
        }

        /// <summary>
        /// The DoGetFillByID routine returns fill details, indexed by the Index parameter.
        /// There is no facility to filter or index data by contract or Patsystems order ID. 
        /// The application must read the list in index order and discard any records it does not require. 
        /// For example, in order to obtain all fills for an order, the entire list of fills is read and fills for other orders ignored.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="fill"></param>
        public static bool DoGetFillByID(string id, ref FillStruct fill)
        {
            return CheckSucceeded(ptGetFillByID(id, ref fill), "DoGetFillByID");
        }

        /// <summary>
        /// The DoGetOpenPosition routine returns the current open position of the trader for a given contract. 
        /// To evaluate this open position as the market moves, the application should call ptGetAveragePrice to obtain the average price of these open fills. 
        /// Profit is reported in contract currency.
        /// </summary>
        /// <param name="exchangedata"></param>
        /// <param name="traderaccount"></param>
        /// <returns></returns>
        public static bool DoGetOpenPosition(PriceUpdateStruct exchangedata, string traderaccount, ref OpenPositionStruct position)
        {
            return CheckSucceeded(ptGetOpenPosition(exchangedata.ExchangeName, exchangedata.CommodityName, exchangedata.ContractDate, traderaccount, ref position), "DoGetOpenPosition");
        }

        /// <summary>
        /// The DoGetContractAtBestPrices routine returns the appropriate At Best price Bid/Offer for a given contract.
        /// At Best prices are available (if supported by the exchange) once a call to ptSubscribePrice has been made.
        /// </summary>
        /// <param name="exchangendata"></param>
        /// <returns></returns>
        public static bool DoGetContractAtBestPrices(PriceUpdateStruct exchangendata, ref AtBestPriceStruct atBest)
        {
            return CheckSucceeded(ptGetContractAtBestPrices(exchangendata.ExchangeName, exchangendata.CommodityName, exchangendata.ContractDate, ref atBest), "DoGetContractAtBestPrices");
        }

        public static bool DoCountContracts(ref int count)
        {
            return CheckSucceeded(ptCountContracts(ref count), "DoCountContracts");
        }

        public static bool DoGetPriceForContract(PriceUpdateStruct exchangeinfo, string traderaccount, ref PriceStruct prices)
        {
            return CheckSucceeded(ptGetPriceForContract(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, ref prices), "DoGetPriceForContract");
        }

        public static bool DoGetContractPosition(PriceUpdateStruct exchangeinfo, string traderaccount, ref PositionStruct position)
        {
            return CheckSucceeded(ptGetContractPosition(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, traderaccount, ref position), "DoGetContractPosition");
        }

        public static bool DoGetTraderByName(string trader, ref TraderAccountStruct traderAccount)
        {
            return CheckSucceeded(ptGetTraderByName(trader, ref traderAccount), "DoGetTraderByName");
        }

        public static bool DoGetExchangeRate(string currency, ref ExchangeRateStruct rate)
        {
            return CheckSucceeded(ptGetExchangeRate(currency, ref rate), "DoGetExchangeRate");
        }

        public static bool DoGetTotalPosition(string trader, ref PositionStruct totalPosition)
        {
            return CheckSucceeded(ptGetTotalPosition(trader, ref totalPosition), "DoGetTotalPosition");
        }

        public static bool DoGetFill(int index, ref FillStruct fillDetails)
        {
            return CheckSucceeded(ptGetFill(index, ref fillDetails), "DoGetFill");
        }

        public static bool DoCountFills(ref int count)
        {
            return CheckSucceeded(ptCountFills(ref count), "DoCountFills");
        }

        public static bool DoCancelOrder(string orderID)
        {
            return CheckSucceeded(ptCancelOrder(orderID), "DoCancelOrder");
        }

        public static void DoSetClientPath(string path)
        {
            ptSetClientPath(path);
        }

        public static bool DoGetContract(int index, ref ContractStruct contract)
        {
            return CheckSucceeded(ptGetContract(index, ref contract), "DoGetContract");
        }

        public static bool DoGetPrice(int index, ref PriceStruct prices)
        {
            return CheckSucceeded(ptGetPrice(index, ref prices), "DoGetPrice");
        }

        public static bool DoSetPDDSSL(char enabled)
        {
            return CheckSucceeded(ptSetPDDSSL(enabled), "DoSetPDDSSL");
        }

        public static bool DoSetPDDSSLClientAuthName(string certName)
        {
            return CheckSucceeded(ptSetPDDSSLClientAuthName(certName), "DoSetPDDSSLClientAuthName");
        }

        public static bool DoCountOrders(ref int count)
        {
            return CheckSucceeded(ptCountOrders(ref count), "DoCountOrders");
        }

        public static bool DoGetOrder(int index, ref OrderDetailStruct orderDetail)
        {
            return CheckSucceeded(ptGetOrder(index, ref orderDetail), "DoGetOrder");
        }

        public static bool DoSetMemoryWarning(int MemAmount)
        {
            return CheckSucceeded(ptSetMemoryWarning(MemAmount), "DoSetMemoryWarning");
        }

        public static bool DoRegisterLinkStateCallback(int callbackID, LinkStateChangeAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterLinkStateCallback(callbackID, cBackProc), "DoRegisterLinkStateCallback");
        }

        public static bool DoRegisterCallback(int callbackID, BasicCallbackAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterCallback(callbackID, cBackProc), "DoRegisterCallback");
        }

        public static bool DoRegisterTickerCallback(int callbackID, TickerUpdateProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterTickerCallback(callbackID, cBackProc), "DoRegisterTickerCallback");
        }

        public static bool DoRegisterDOMCallback(int callbackID, PriceDomProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterDOMCallback(callbackID, cBackProc), "DoRegisterDOMCallback");
        }

        public static bool DoRegisterPriceCallback(int callbackID, PriceProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterPriceCallback(callbackID, cBackProc), "DoRegisterPriceCallback");
        }

        public static bool DoRegisterOrderCallback(int callbackID, OrderUpdateProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterOrderCallback(callbackID, cBackProc), "DoRegisterOrderCallback");
        }

        public static bool DoRegisterCommodityCallback(int callbackID, CommodityProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterCommodityCallback(callbackID, cBackProc), "DoRegisterCommodityCallback");
        }

        public static bool DoRegisterFillCallback(int callbackID, FillStructAddr cbackProc)
        {
            return CheckSucceeded(ptRegisterFillCallback(callbackID, cbackProc), "DoRegisterFillCallback");
        }

        public static bool DoRegisterContractCallback(int callbackID, ContractUpdateProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterContractCallback(callbackID, cBackProc), "DoRegisterContractCallback");
        }

        public static bool DoGetConsolidatedPosition(PriceUpdateStruct exchangeinfo, string traderaccount, int positionType, ref FillStruct Fill)
        {
            return CheckSucceeded(ptGetConsolidatedPosition(exchangeinfo.ExchangeName, exchangeinfo.CommodityName, exchangeinfo.ContractDate, traderaccount, positionType, ref Fill), "DoGetConsolidatedPosition");
        }

        public static bool DoRegisterAtBestCallback(int callbackID, AtBestProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterAtBestCallback(callbackID, cBackProc), "DoRegisterMsgCallback");
        }

        public static bool DoRegisterMsgCallback(int callbackID, MsgIDProcAddr cBackProc)
        {
            return CheckSucceeded(ptRegisterMsgCallback(callbackID, cBackProc), "DoRegisterMsgCallback");
        }

        public static bool DoNotifyAllMessages(char enabled)
        {
            return CheckSucceeded(ptNotifyAllMessages(enabled), "DoNotifyAllMessages");
        }

        public static bool DoGetUsrMsgByID(string msgID, ref MessageStruct message)
        {
            return CheckSucceeded(ptGetUsrMsgByID(msgID, ref message), "DoGetUsrMsgByID");
        }

        public static bool DoSetEncryptionCode(char code)
        {
            return CheckSucceeded(ptSetEncryptionCode(code), "DotSetEncryptionCode");
        }

        public static bool DoSetOrderCancelFailureDelay(int delay)
        {
            return CheckSucceeded(ptSetOrderCancelFailureDelay(delay), "DotSetOrderCancelFailureDelay");
        }

        public static bool DoSetOrderQueuedFailureDelay(int delay)
        {
            return CheckSucceeded(ptSetOrderQueuedFailureDelay(delay), "DotSetOrderQueuedFailureDelay");
        }

        public static bool DoSetOrderSentFailureDelay(int delay)
        {
            return CheckSucceeded(ptSetOrderSentFailureDelay(delay), "DotSetOrderSentFailureDelay");
        }
    }
}
