using System.Runtime.InteropServices;

namespace Adapter.PatsGlobal.PatsWrapper.TradingObjects
{
    /** struct for host and price server callback **/
    public struct LinkStateStruct
    {
        public byte oldState;
        public byte newState;
    }

    /** struct for price update callback**/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PriceUpdateStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGENAME + 1)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_COMMODITYNAME + 1)]
        public string CommodityName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACTDATE + 1)]
        public string ContractDate;
    }

    /** Contains all of the constants used in communicating with the Delphi API**/
    static class Constants
    {
        #region Callback Constants
        /** Callback constants - these are the integer values sent from the Delphi API
         * along with the name corresponding to the callbacks
         * */
        public const int ptHostLinkStateChange = 1;
        public const int ptPriceLinkStateChange = 2;
        public const int ptLogonStatus = 3;
        public const int ptMessage = 4;
        public const int ptOrder = 5;
        public const int ptForcedLogout = 6;
        public const int ptDataDLComplete = 7;
        public const int ptPriceUpdate = 8;
        public const int ptFill = 9;
        public const int ptStatusChange = 10;
        public const int ptContractAdded = 11;
        public const int ptContractDeleted = 12;
        public const int ptExchangeRate = 13;
        public const int ptConnectivityStatus = 14;
        public const int ptOrderCancelFailure = 15;
        public const int ptAtBestUpdate = 16;
        public const int ptTickerUpdate = 17;
        public const int ptMemoryWarning = 18;
        public const int ptSubscriberDepthUpdate = 19;
        public const int ptVTSCallback = 20;
        public const int ptDOMUpdate = 21;
        public const int ptSettlementCallback = 22;
        public const int ptStrategyCreateSuccess = 23;
        public const int ptStrategyCreateFailure = 24;
        public const int ptAmendFailureCallback = 25;
        public const int ptGenericPriceUpdate = 26;
        public const int ptBlankPrice = 27;
        public const int ptOrderSentFailure = 28;
        public const int ptOrderQueuedFailure = 29;
        public const int ptOrderBookReset = 30;
        #endregion

        #region Size of struct constants
        public const int SIZE_OF_CHAR = 1;
        public const int SIZE_OF_INT = 4;
        public const int SIZE_OF_BYTE = 1;
        public const int SIZE_OF_USERNAME = 255;
        public const int SIZE_OF_PASSWORD = 255;
        public const int SIZE_OF_NEWPASSWORD = 255;
        public const int SIZE_OF_STATUS = 1;
        public const int SIZE_OF_REASON = 60;
        public const int SIZE_OF_TRADERACCOUNT = 20;
        public const int SIZE_OF_EXCHANGENAME = 10;
        public const int SIZE_OF_COMMODITYNAME = 10;
        public const int SIZE_OF_CONTRACTDATE = 50;
        public const int SIZE_OF_CURRENCY = 10;
        public const int SIZE_OF_GROUP = 10;
        public const int SIZE_OF_ONEPOINT = 10;
        public const int SIZE_OF_TICKSIZE = 10;
        public const int SIZE_OF_EXPIRY = 8;
        public const int SIZE_OF_LASTTRADEDATE = 8;
        public const int SIZE_OF_MARGIN = 20;
        public const int SIZE_OF_MARKETREF = 16;
        public const int SIZE_OF_CONTRACTTYPE = 10;
        public const int SIZE_OF_STRIKEPRICE = 10;
        public const int SIZE_OF_RATIO = 10;
        public const int SIZE_OF_BACKOFFICEID = 20;
        public const int SIZE_OF_ORDERTYPE = 10;
        public const int SIZE_OF_TICKETTYPE = 2;
        public const int SIZE_OF_PRICE = 20;


        /** all structs contain +1 after their component sizes because the strings are null terminated **/
        #region Logon struct
        public const int SIZE_OF_LOGON_STRUCT =
            SIZE_OF_USERNAME +
            SIZE_OF_PASSWORD +
            SIZE_OF_NEWPASSWORD +
            SIZE_OF_CHAR + // reports  
            SIZE_OF_CHAR + 1111;  // reset
        #endregion

        #region Logon Status Struct
        public const int SIZE_OF_LOGONSTATUS_STRUCT =
            SIZE_OF_STATUS +
            SIZE_OF_REASON +1 +
            SIZE_OF_TRADERACCOUNT+1;
        #endregion

        #region Trader Account Struct
        public const int SIZE_OF_TRADERACCOUNT_STRUCT =
            SIZE_OF_TRADERACCOUNT + 1 +
            SIZE_OF_BACKOFFICEID + 1 +
            SIZE_OF_CHAR; // tradable or not (Y or N)

        #endregion

        #region Exchange Struct
        public const int SIZE_OF_EXCHANGE_STRUCT =
            SIZE_OF_EXCHANGENAME +
            SIZE_OF_CHAR + // query enabled
            SIZE_OF_CHAR + // Amend enabled
            SIZE_OF_INT +  // Stategy tool
            SIZE_OF_CHAR + // Custom decimals
            SIZE_OF_INT + // number of decimal places
            SIZE_OF_CHAR + // ticket type
            SIZE_OF_CHAR + // RFQ accept
            SIZE_OF_CHAR + // if supports RFQ tickdown 
            SIZE_OF_CHAR + //enable block trades
            SIZE_OF_CHAR + // enable basis trades
            SIZE_OF_CHAR + // supports against actuals
            SIZE_OF_CHAR;  // supports cross trades            
        #endregion

        #region Commodity Struct
        public const int SIZE_OF_COMMODITY_STRUCT =
            SIZE_OF_EXCHANGENAME + 1 +
            SIZE_OF_COMMODITYNAME + 1 +
            SIZE_OF_CURRENCY + 1 +
            SIZE_OF_GROUP + 1 +
            SIZE_OF_ONEPOINT + 1 +
            SIZE_OF_INT + // ticks per point
            SIZE_OF_TICKSIZE + 1;
        #endregion

        #region Leg Struct
        public const int SIZE_OF_LEGSTRUCT =
            SIZE_OF_CONTRACTTYPE + 1 +
            SIZE_OF_CONTRACTDATE + 1 +
            SIZE_OF_PRICE + 1 +
            SIZE_OF_INT + 1 +
            SIZE_OF_COMMODITYNAME;    
           // SIZE_OF_COMMODITYNAME + 1 +
           // SIZE_OF_EXPIRY + 1 +
           // SIZE_OF_STRIKEPRICE + 1 +
           // SIZE_OF_RATIO + 1;
        #endregion

        #region Contract Struct
        public const int SIZE_OF_CONTRACT_STRUCT =
            SIZE_OF_COMMODITYNAME  + 1 +
            SIZE_OF_CONTRACTDATE  + 1 +
            SIZE_OF_EXCHANGENAME  + 1 +
            SIZE_OF_EXPIRY + 1 +
            SIZE_OF_LASTTRADEDATE + 1 +
            SIZE_OF_INT + 1 + // number of legs
            SIZE_OF_INT + 1 + // ticks per point
            SIZE_OF_TICKSIZE + 1 +
            SIZE_OF_CHAR + // Tradable or not
            SIZE_OF_INT + // GT status
            SIZE_OF_MARGIN + 1 +
            SIZE_OF_CHAR + // ESA Template
            SIZE_OF_MARKETREF + 1 +
            16 * SIZE_OF_LEGSTRUCT;
        #endregion

        #region Order type struct
        public const int SIZE_OF_ORDERTYPE_STRUCT =
            SIZE_OF_ORDERTYPE + 1 +
            SIZE_OF_EXCHANGENAME + 1 +
            SIZE_OF_INT + //order type ID
            SIZE_OF_BYTE + //number of prices required
            SIZE_OF_BYTE + //number of volumes required
            SIZE_OF_BYTE + //number of dates required
            SIZE_OF_CHAR + //Delphi API auto created or not
            SIZE_OF_CHAR + //time triggered or not
            SIZE_OF_CHAR + //exchange synthetic order type or not
            SIZE_OF_CHAR + //GTC or not
            SIZE_OF_TICKETTYPE;


        #endregion

        #region subscribe to contract struct
        public const int SIZE_OF_SUBSCRIBE_STRUCT =
            SIZE_OF_EXCHANGENAME + 1 +
         //   SIZE_OF_COMMODITYNAME + 1 +
            SIZE_OF_COMMODITYNAME + 1;
        #endregion

        #endregion end of size of structs region

        #region Logon Status constants
        public const int LogonStatusLogonFailed = 0;
        public const int LogonStatusLogonSucceeded = 1;
        public const int LogonStatusForcedOut = 2;
        public const int LogonStatusObsoleteVers = 3;
        public const int LogonStatusWrongEnv = 4;
        public const int LogonStatusDatabaseErr = 5;
        public const int LogonStatusInvalidUser = 6;
        public const int LogonStatusLogonRejected = 7;
        public const int LogonStatusInvalidAppl = 8;
        public const int LogonStatusLoggedOn = 9;
        public const int LogonStatusInvalidLogonState = 10;
        #endregion
        
        #region Link status constants
        public const int LinkStatusOpened = 1;
        public const int LinkStatusConnecting = 2;
        public const int LinkStatusConnected = 3;
        public const int LinkStatusClosed = 4;
        public const int LinkStatusInvalid = 5;
        #endregion

        #region PriceDetail Struct
            public const int SIZE_OF_PRICEDETAILSTRUCT = 
                SIZE_OF_PRICE +1 +
                SIZE_OF_INT + //Volume
                SIZE_OF_BYTE + //AgeCounter
                SIZE_OF_BYTE + //direction 
                SIZE_OF_BYTE + //hour
                SIZE_OF_BYTE + //minute
                SIZE_OF_BYTE; //second
        #endregion

        #region Price Struct
        public const int SIZE_OF_PRICESTRUCT =
            SIZE_OF_PRICEDETAILSTRUCT * 79 +
            SIZE_OF_INT + //status
            SIZE_OF_INT; //bit mask
        #endregion

        #region logging constants
        public const int logComment = 0;
        public const int logError = 1;
        #endregion


    }

}


