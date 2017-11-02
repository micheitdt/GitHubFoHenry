using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Adapter.TaifexGlobalPATS.ApiPATS
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void BasicCallbackAddr();
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void LinkStateChangeAddr(ref LinkStateStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PriceProcAddr(ref PriceUpdateStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void OrderUpdateProcAddr(ref OrderUpdateStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void PriceDomProcAddr(ref PriceUpdateStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void CommodityProcAddr(ref CommodityUpdStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void FillStructAddr(ref FillUpdateStruct datat);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void TickerUpdateProcAddr(ref TickerUpdStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void ContractUpdateProcAddr(ref ContractUpdStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void AtBestProcAddr(ref AtBestUpdStruct data);
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void MsgIDProcAddr(ref MsgIDStruct data);
    
    public enum fillType
    {
        ptNormalFill,
        ptExternalFill,
        ptNettedFill,
        ptRetainedFill,
        ptBlockLegFill
    }

    public enum ptPositionType
    {
        po_Contract,
        po_NetPos,
        po_Buys,
        po_Sells,
        po_Average,
        po_With_Margin_Buys_Avg_Price,
        po_With_Margin_Sells_avg_price,
        po_Last,
        po_BuyingPowerRemaining,
        po_Open_Pl,
        po_Cum_Pl,
        po_Total_Pl,
        po_Commission,
        po_Currency,
        po_MarginPerLot,
        po_PLBurnRate,
        po_OpenPositionExposure,
        po_CashBuyingPower,
        po_SupressPlValue,
        po_MarginPaid,
        po_Equity,
        po_ContractType,
        po_Format,
        po_Position_End
    }
   
    /* Live Quote Layout 
     * */
    public enum lqLayout  
    {
        lq_Contract,
        lq_Hit,
        lq_BidVol,
        lq_Bid,
        lq_Offer,
        lq_OfferVol,
        lq_Take,
        lq_Last,
        lq_LastVol,
        lq_Low,
        lq_High,
        lq_Opening,
        lq_Closing,
        lq_TotalVol,
        lq_Change,
        lq_ChangeYDSP,
        lq_ChangeClosing,
        lq_MarketStatus,
        lq_Net,
        lq_BuyOrders,
        lq_SellOrders,
        lq_Ref,
        lq_End 
    }
    /** order status enum /
     * 
     * 
     **/
    public enum OrderStatus: byte
    {
        ptNone,
        ptQueued,
        ptSent,
        ptWorking,
        ptRejected,
        ptCancelled,
        ptBalCancelled,
        ptPartFilled,
        ptFilled,
        ptCancelPending,
        ptAmendPending,
        ptUnconfirmedFilled,
        ptUnconfirmedPartFilled,
        ptHeldOrder,
        ptCancelHeldOrder,
        ptTransferred,
        ptExternalCancelled// added for GT
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ContractUpdStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_NAME)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct AtBestUpdStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_NAME)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct OrderIDStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string OrderID;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MsgIDStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string MsgID;
    }

    /// <summary>
    /// Ticker Price update struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TickerUpdStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_NAME)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_BID_PRICE)]
        public string BidPrice;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int BidVolume;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_OFFER_PRICE)]
        public string OfferPrice;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int OfferVolume;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LAST_PRICE)]
        public string LastPrice;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int LastVolume;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Bid;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Offer;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Last;

    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FillStruct
    {
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Index;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 71)]
        public string FillID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 51)]
        public string ContractDate;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char BuyOrSell;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Lots;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Price;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDER_ID)]
        public string OrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DATESENT)]
        public string DateFilled;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TIMESENT)]
        public string TimeFilled;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DATESENT)]
        public string DateHostRecd;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TIMESENT)]
        public string TimeHostRecd;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXHANGEID)]
        public string ExchOrderID;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte FillType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20 + 1)]
        public string TraderAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string UserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXECUTIONID)]
        public string ExchangeFillID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string ExchangeRawPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXECUTIONID)]
        public string ExecutionID;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int GTStatus;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int SubType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string CounterParty;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY2)]
        public string Leg;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct AmendOrderTypesArray
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY500)]
        public string OrderTypeAmend;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct OrderTypeStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string OrderType;      
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ExchangeName;      
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int OrderTypeID;   
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte NumPricesReqd;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte NumVolumesReqd;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte NumDatesReqd;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char AutoCreated;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char TimeTriggered;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char RealSynthetic;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char GTCFlag;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY2)]
        public string TicketType;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char PatsOrderType;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int AmendOTCount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY50)]
        public string AlgoXML;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1),Serializable]
    public struct LogonStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_USERNAME)]
        public string UserID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PASSWORD)]
        public string Password;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PASSWORD)]
        public string NewPassword;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_SHOWREASON)]
        public char Reset;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_SHOWREASON)]
        public char Reports;
    }
    /// <summary>
    /// 
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct LogonStatusStruct
    {
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Status;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = /*Constants.SIZE_OF_REASON*/101)]
        public string Reason;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DEFAULT_TRADER_ACCOUNT)]
        public string DefaultTraderAccount;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_SHOWREASON)]
        public char ShowReason;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_SHOWREASON)]
        public char DOMEnabled;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_SHOWREASON)]
        public char PostTradeAmend;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_USERNAME)]
        public string UserName;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_SHOWREASON)]
        public char GTEnabled;
    }

    /** struct for host and price server callback **/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct LinkStateStruct
    {
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte oldState;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte newState;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi,  Pack = 1 )]
    public struct SubArray
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string contractType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string contractCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string contractDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string strikePrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ratio;


    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi , Pack = 1 )]
    public struct ExternalID 
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
        public SubArray[] Leg;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ContractStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXPIRY)]
        public string ExpiryDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LASTTRADEDATE)]
        public string LastTradeDate;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int NumberOfLegs;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int TicksPerPoint;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string TickSize;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Tradable;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int GTStatus;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string Margin;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char ESATemplate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY16)]
        public string MarketRef;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_NAME)]
        public string lnExhcangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string lnContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string lnContractDate;
 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2 )]
        public ExternalID[] LegStruct;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ExtendedContractStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXPIRY)]
        public string ExpiryDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LASTTRADEDATE)]
        public string LastTradeDate;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int NumberOfLegs;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int TicksPerPoint;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string TickSize;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Tradable;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int GTStatus;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string Margin;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char ESATemplate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY16)]
        public string MarketRef;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_NAME)]
        public string lnExhcangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string lnContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string lnContractDate;
 
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16 )]
        public ExternalID[] LegStruct;
    }

    /** struct for price update callback**/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CommodityUpdStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_COMMODITY_NAME)]
        public string CommodityName;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FillUpdateStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDER_ID)]
        public string OrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_FILLID)]
        public string FillID;
      
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct OrderUpdateStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst =  Constants.SIZE_OF_ORDER_ID)]
        public string OrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst =  Constants.SIZE_OF_ORDER_ID)]
        public string OldOrderID;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte OrderStatus;
        [MarshalAs(UnmanagedType.I4, SizeConst =  Constants.SIZE_OF_INT)]
        public int OFSeqNumber;
        [MarshalAs(UnmanagedType.I4, SizeConst =  Constants.SIZE_OF_INT)]
        public int OrderTypeId ; // future reference
    }

    /** struct for price update callback**/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ExchangeStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char QueryEnabled;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char AmendEnabled;

        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Strategy;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char CustomDecs;

        [MarshalAs(UnmanagedType.I4, SizeConst =  Constants.SIZE_OF_INT)]
        public int Decimals;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char TicketType;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char RFQA;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char RFQT;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char EnableBlock;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char EnableBasis;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char EnableAA;

        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char EnableCross;

        [MarshalAs(UnmanagedType.I4, SizeConst =  Constants.SIZE_OF_INT)]
        public int GTStatus;
    }

    [StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi , Pack = 1 )]
    public struct PriceDetailStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string Price;       // FP number
        [MarshalAs(UnmanagedType.I4,SizeConst= Constants.SIZE_OF_INT) ]
        public int Volume;       // does not apply to all price types
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte AgeCounter;          // if zero, price is "expired"
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Direction;          // 0=Same, 1=Rise, 2=Fall
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Hour;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Minute;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Second;          // Timestamp

    }
    //* Price detail structure **/

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PriceStruct
    {
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Bid;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Offer;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct ImpliedBid;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct ImpliedOffer;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct RFQ;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last0;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last1;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last2;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last3;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last4;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last5;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last6;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last7;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last8;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last9;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last10;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last11;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last12;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last13;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last14;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last15;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last16;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last17;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last18;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Last19;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Total;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct High;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Low;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Opening;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct Closing;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM0;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM1;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM2;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM3;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM4;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM5;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM6;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM7;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM8;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM9;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM10;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM11;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM12;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM13;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM14;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM15;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM16;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM17;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM18;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct BidDOM19;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM0;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM1;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM2;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM3;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM4;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM5;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM6;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM7;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM8;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM9;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM10;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM11;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM12;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM13;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM14;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM15;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM16;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM17;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM18;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct OfferDOM19;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct LimitUp;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct LimitDown;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct ExecutionUp;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct ExecutionDown;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct ReferencePrice;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct pvCurrStl;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct pvSODStl;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct pvNewStl;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct pvIndBid;
        [MarshalAs(UnmanagedType.Struct, SizeConst = Constants.SIZE_OF_PRICEDETAILSTRUCT)]
        public PriceDetailStruct pvIndOffer;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Status;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int ChangeMask;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int PriceStatus;

    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PATStoProxyFormat
    {
        [MarshalAs(UnmanagedType.Struct, SizeConst = 73)]
        public PriceUpdateStruct SymbolInfo;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct Bid;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct Offer;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct Last;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct Total;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct High;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct Low;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM0;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM1;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM2;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM3;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM4;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM5;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM6;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM7;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM8;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct BidDOM9;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM0;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM1;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM2;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM3;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM4;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM5;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM6;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM7;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM8;
        [MarshalAs(UnmanagedType.Struct, SizeConst = 30)]
        public PriceDetailStruct OfferDOM9;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct GenericMarginFigure
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string data;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PLburnRate
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string PlBurnRate;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MarginPaid
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Margin;
    }

    /// <summary>
    /// Open position exposure struct
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct OpenPositionStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Profit;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Buys;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Sells;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct Exposure
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string exposure;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct BPremainingStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string BPremaining;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ExchangeRateStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)] 
        public string ExchangeRate;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TraderAccountNameStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string TraderAccount;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct TraderAccountStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string TraderAccount;
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string BackOffice;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Tradable;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int LossLimit;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PositionStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string Profit;     // FP number
        [MarshalAs(UnmanagedType.I4,SizeConst = Constants.SIZE_OF_INT)]
        public int Buys;
        [MarshalAs(UnmanagedType.I4,SizeConst = Constants.SIZE_OF_INT)]
        public int Sells;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct CommodityStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_NAME)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CURRENCY)]
        public string Currency;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_GROUP)]
        public string Group;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ONEPOINT)]
        public string OnePoint;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int TicksPerPoint;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TICKSIZE)]
        public string TickSize;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int GTStatus;
     }
 
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MarginExp
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Margin;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MarginPerLotStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Margin;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct GetAveragePriceStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 21)]
        public string Price;
    }

    ///** struct for price update callback**/
    //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    //public struct GenericExchangeName
    //{
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME + 1)]
    //    public string ExchangeName;
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_COMMODITY_NAME + 1)]
    //    public string CommodityName;
    //    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE + 1)]
    //    public string ContractDate;

    //}
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct AveragePriceStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string AveragePrice;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1 )]
    public struct AtBestPriceStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_BID_PRICE)]
        public string BidPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_OFFER_PRICE)]
        public string OfferPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LAST_BUYER)]
        string LastBuyer;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LAST_SELLER)]
        string LastSeller;

    }
    
    /** struct for price update callback**/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PriceUpdateStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_COMMODITY_NAME)]
        public string CommodityName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;

    }
   /** Struct for order amendment **/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct AmendOrderStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst= Constants.SIZE_OF_PRICE)]
        public string Price;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string Price2;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int lots;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LINKEDORDERS)]
        public string LinkedOrder;
        [MarshalAs(UnmanagedType.I1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char OpenOrClose;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRADERACCOUNT)]
        public string TraderAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_REFERENCE)]
        public string Reference; // Persistent field from the NewOrderStruct for 
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Priority;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRIGGERDATE)]
        public string Triggerdate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRIGGERTIME)]
        public string TriggerTime;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int SubState;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchType;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int BatchCount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchStatus;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ParentID;
        [MarshalAs(UnmanagedType.I1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char DoneForDay;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_BIGREF)]
        public string BigRefField;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public string TimeOut;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string RawPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string RawPrice2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXECUTIONID)]
        public string ExecutionID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CLIENTID)]
        public string ClientID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ESAREF)]
        public string ESARef;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_YDSPAUDIT)]
        public string YDSPAudit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ICSNEARLEGPRICE)]
        public string ICSNearLegPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ICSFARLEGPRICE)]
        public string ICSFarLegPrice;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int MinClipSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalUserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string LocalTrader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string LocalBOF;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalOrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalExAcct;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string RoutingID1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string RoutingID2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TARGETUSERNAME)]
        public string AmendOrderType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TARGETUSERNAME)]
        public string TargetUserName;
    }


    /** struct for Order update callback**/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct NewOrderStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRADERACCOUNT)]
        public string TraderAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDERTYPE)]
        public string OrderType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_NAME)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;
        [MarshalAs(UnmanagedType.I1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char BuyOrSell;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string Price;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string Price2;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Lots;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LINKEDORDERS)]
        public string LinkedOrder;
        [MarshalAs(UnmanagedType.I1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char OpenOrClose;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Xref;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int XrefP;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_GOODTILLDATE)]
        public string GoodTillDate;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char TriggerNow;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_REFERENCE)]
        public string Reference;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ESAREF)]
        public string ESARef;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Priority;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRIGGERDATE)]
        public string TriggerDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRIGGERTIME)]
        public string TriggerTime;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchType;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int BatchCount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchStatus;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ParentID;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char DoneForDay;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_BIGREF)]
        public string BigRefField;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_SENDER_LOCATION)]
        public string SenderLocation;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string RawPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string RawPrice2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXECUTIONID)]
        public string ExecutionID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CLIENTID)]
        public string ClientID;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char APIM;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string APIMUser;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string YDSPAudit;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ICSNEARLEGPRICE)]
        public string ICSNearLegPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ICSFARLEGPRICE)]
        public string ICSFarLegPrice;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int MinClipSize;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int MaxClipSize;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Randomise;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY2)]
        public string TicketType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TICKETVERSION)]
        public string TicketVersion;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ExchangeField;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string BOFID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY05)]
        public string Badge;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalUserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string LocalTrader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string LocalBOF;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalOrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalExAcct;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string RoutingID1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string RoutingID2;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Inactive;

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct MessageStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string MsgID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY500)]
        public string MsgText;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char IsAlert;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Status;
    }
    
    /** struct for Order update callback**/
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct OrderDetailStruct
    {
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Index;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Historic;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Checked;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDER_ID)]
        public string OrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDER_ID)]
        public string DisplayID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXHANGEID)]
        public string ExchOrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDERDETAIL_USERNAME)]
        public string UserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRADERACCOUNT)]
        public string TraderAccount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDERTYPE)]
        public string OrderType;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHANGE_NAME)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ORDERDETAIL_CONTACTNAME)]
        public string ContractName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CONTRACT_DATE)]
        public string ContractDate;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char BuyOrSell;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string Price;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string Price2;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Lots;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_LINKEDORDERS)]
        public string LinkedOrder;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int AmountFilled;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int NoOfFills;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_AVERAGEPRICE)]
        public string AveragePrice;
        [MarshalAs(UnmanagedType.I1, SizeConst = Constants.SIZE_OF_BYTE)]
        public byte Status;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char OpenOrClose;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DATESENT)]
        public string DateSent;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TIMESENT)]
        public string TimeSent;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DATESENT)]
        public string DateHostRecd;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TIMESENT)]
        public string TimeHostRecd;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DATESENT)]
        public string DateExchRecd;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TIMESENT)]
        public string TimeExchRecd;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHDATE)]
        public string DateExchAckn;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXCHTIME)]
        public string TimeExchAckn;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_NONEXECREASON)]
        public string NonExecReason;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Xref;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int XrefP;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int UpdateSeq;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_GOODTILLDATE)]
        public string GoodTillDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_REFERENCE)]
        public string Reference;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Priority;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRIGGERDATE)]
        public string TriggerDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TRIGGERTIME)]
        public string TriggerTime;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int SubState;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchType;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int BatchCount;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string BatchStatus;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ParentID;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char DoneForDay;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_BIGREF)]
        public string BigRefField;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int Timeout;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_QUOTEID)]
        public string QuoteID;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_LOTSPOSTED)] 
        public int LotsPosted; 
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)] 
        public int ChildCount; 
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)] 
        public int ActLots; 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_SENDER_LOCATION)]
        public string SenderLocation; 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)] 
        public string RawPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_PRICE)]
        public string RawPrice2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_EXECUTIONID)]
        public string ExecutionID; 
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_CLIENTID)]
        public string ClientID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ESAREF)]
        public string ESARef;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string ISINCode;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string CashPrice;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public byte Methodology;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string BasisRef;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DATEARRAY8)]
        public string EntryDate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_TIMEARRAY6)]
        public string EntryTime;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char APIM;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string APIMUser;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ICSNEARLEGPRICE)]
        public string ICSNearLegPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ICSFARLEGPRICE)]
        public string ICSFarLegPrice;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_DATEARRAY8)]
        public string CreationDate;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int OrderHistorySeq;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int MinClipSize;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int MaxClipSize;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Randomise;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char ProfitLevel;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int OFSeqNumber;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string ExchangeField;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string BOFID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY05)]
        public string Badge;
        [MarshalAs(UnmanagedType.I4, SizeConst = Constants.SIZE_OF_INT)]
        public int GTStatus;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalUserName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string LocalTrader;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string LocalBOF;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalOrderID;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string LocalExAcct;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string RoutingID1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY10)]
        public string RoutingID2;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string FreeTextField1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.SIZE_OF_ARRAY20)]
        public string FreeTextField2;
        [MarshalAs(UnmanagedType.U1, SizeConst = Constants.SIZE_OF_CHAR)]
        public char Inactive;
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
        public const int ptCommodityUpdate = 32;
        public const int ptContractDateUpdate = 33;
        #endregion

        #region Size of struct constants
        public const int SIZE_OF_CHAR = 1;
        public const int SIZE_OF_INT = 4;
        public const int SIZE_OF_BYTE = 1;
        public const byte SIZE_OF_8UBYTE = 8;
        public const int SIZE_OF_USERNAME = 256;
        public const int SIZE_OF_PASSWORD = 256;
        public const int SIZE_OF_NEWPASSWORD = 256;
        public const int SIZE_OF_STATUS = 1;
        public const int SIZE_OF_REASON = 61;
        public const int SIZE_OF_TRADERACCOUNT = 21;
        public const int SIZE_OF_EXCHANGE_NAME = 11;
        public const int SIZE_OF_CONTRACT_NAME = 11;
        public const int SIZE_OF_COMMODITY_NAME = 11;
        public const int SIZE_OF_CONTRACTNAME = 51;
        public const int SIZE_OF_50 = 51;
        public const int SIZE_OF_ORDER_ID = 11;
        public const int SIZE_OF_FILLID = 71;
        public const int SIZE_OF_OLD_ORDER_ID = 11;
        public const int SIZE_OF_ORDER_STATUS = 1;
        public const int SIZE_OF_OFSeqNumber = 2;
        public const int SIZE_OF_CONTRACT_DATE = 51;
        public const int SIZE_OF_CURRENCY = 11;
        public const int SIZE_OF_GROUP = 11;
        public const int SIZE_OF_ONEPOINT = 11;
        public const int SIZE_OF_TICKSIZE = 11;
        public const int SIZE_OF_EXPIRY = 9;
        public const int SIZE_OF_LASTTRADEDATE = 9;
        public const int SIZE_OF_MARGIN = 21;
        public const int SIZE_OF_MARKETREF = 17;
        public const int SIZE_OF_CONTRACTTYPE = 11;
        public const int SIZE_OF_STRIKEPRICE = 11;
        public const int SIZE_OF_RATIO = 11;
        public const int SIZE_OF_BACKOFFICEID = 21;
        public const int SIZE_OF_ORDERTYPE = 11;
        public const int SIZE_OF_TICKETTYPE = 3;
        public const int SIZE_OF_PRICE = 21;
        //public const int SIZE_OF_INACTIVE = 870;
        public const int SIZE_OF_EXHANGEID = 31;
        public const int SIZE_OF_LINKEDORDERS = 11;
        public const int SIZE_OF_AVERAGEPRICE = 21;
        public const int SIZE_OF_DATESENT = 9;
        public const int SIZE_OF_TIMESENT = 7;
        public const int SIZE_OF_EXCHTIME = 7;
        public const int SIZE_OF_EXCHDATE = 9;
        public const int SIZE_OF_NONEXECREASON = 61;
        public const int SIZE_OF_GOODTILLDATE = 9;
        public const int SIZE_OF_REFERENCE = 26;
        public const int SIZE_OF_TRIGGERDATE = 9;
        public const int SIZE_OF_TIGGERTIME = 7;
        public const int SIZE_OF_ARRAY10 = 11;
        public const int SIZE_OF_ARRAY5  = 6;
        public const int SIZE_OF_BIGREF = 256;
        public const int SIZE_OF_SENDERLOCATION = 33;
        public const int SIZE_OF_QUOTEID = 121;
        public const int SIZE_OF_LOTSPOSTED = 4;
        public const int SIZE_OF_SENDER_LOCATION = 33;
        public const int SIZE_OF_EXECUTIONID = 71; 
        public const int SIZE_OF_CLIENTID = 21; 
        public const int SIZE_OF_ESAREF = 51;
        public const int SIZE_OF_ISINCODE = 21;
        public const int SIZE_OF_CASHPRICE = 21;
        public const int SIZE_OF_METHODOLOGY = 1;
        public const int SIZE_OF_BASISREF = 21;
        public const int SIZE_OF_ARRAY20 = 21;
        public const int SIZE_OF_ARRAY30  = 31;
        public const int SIZE_OF_ARRAY16 = 17;
        public const int SIZE_OF_ARRAY05 = 6;
        public const int SIZE_OF_TRIGGERTIME = 7;
        public const int SIZE_OF_DATEARRAY8 = 9;
        public const int SIZE_OF_TIMEARRAY6 = 7;
        public const int SIZE_OF_ICSNEARLEGPRICE = 11;
        public const int SIZE_OF_ICSFARLEGPRICE = 11;
        public const int SIZE_OF_SEQUENCE_NUMBER = 1;
        public const int SIZE_OF_ORDERDETAIL_USERNAME = 11;
        public const int SIZE_OF_ORDERDETAIL_CONTACTNAME = 11;
        public const int SIZE_OF_DEFAULT_TRADER_ACCOUNT = 21;
        public const int SIZE_OF_SHOWREASON = 1;
        public const int SIZE_OF_YDSPAUDIT = 21;
        public const int SIZE_OF_AMENDORDERTYPE = 11;
        public const int SIZE_OF_TARGETUSERNAME = 11;
        public const int SIZE_OF_EXCHANGEFIELD = 11;
        public const int SIZE_OF_TICKETVERSION = 4;
        public const int SIZE_OF_ARRAY2 = 3;
        public const int SIZE_OF_ARRAY50 = 51;
        public const int SIZE_OF_ARRAY500 = 501;
        public const int SIZE_OF_MARGIN_PER_LOT = 21;
        public const int SIZE_OF_BID_PRICE = 21;
        public const int SIZE_OF_OFFER_VOLUME = 21;
        public const int SIZE_OF_OFFER_PRICE = 21;
        public const int SIZE_OF_LAST_PRICE = 21;
        public const int SIZE_OF_LAST_BUYER = 4 ;
        public const int SIZE_OF_LAST_SELLER = 4;

        /** all structs contain +1 after their component sizes because the strings are null terminated **/
        #region Logon struct
        public const int SIZE_OF_LOGON_STRUCT =
            SIZE_OF_USERNAME +
            SIZE_OF_PASSWORD +
            SIZE_OF_NEWPASSWORD +
            SIZE_OF_CHAR + // reports  
            SIZE_OF_CHAR;  // reset
        #endregion

        #region Logon Status Struct
        public const int SIZE_OF_LOGONSTATUS_STRUCT =
            SIZE_OF_STATUS +
            SIZE_OF_REASON +
            SIZE_OF_TRADERACCOUNT;
        #endregion

        #region Trader Account Struct
        public const int SIZE_OF_TRADERACCOUNT_STRUCT =
            SIZE_OF_TRADERACCOUNT +
            SIZE_OF_BACKOFFICEID +
            SIZE_OF_CHAR; // tradable or not (Y or N)

        #endregion

        #region Exchange Struct
        public const int SIZE_OF_EXCHANGE_STRUCT =
            SIZE_OF_EXCHANGE_NAME +
            SIZE_OF_CHAR + // query enabled
            SIZE_OF_CHAR + // Amend enabled
            SIZE_OF_INT +  // Stategy tool
            SIZE_OF_CHAR + // Custom decimals
            SIZE_OF_INT +  // number of decimal places
            SIZE_OF_CHAR + // ticket type
            SIZE_OF_CHAR + // RFQ accept
            SIZE_OF_CHAR + // if supports RFQ tickdown 
            SIZE_OF_CHAR + //enable block trades
            SIZE_OF_CHAR + // enable basis trades
            SIZE_OF_CHAR + // supports against actuals
            SIZE_OF_CHAR + // supports cross trades   
            SIZE_OF_INT;   // GTStatus 
        #endregion

        #region Commodity Struct
        public const int SIZE_OF_COMMODITY_STRUCT =
            SIZE_OF_EXCHANGE_NAME +
            SIZE_OF_COMMODITY_NAME +
            SIZE_OF_CURRENCY +
            SIZE_OF_GROUP +
            SIZE_OF_ONEPOINT +
            SIZE_OF_INT + // ticks per point
            SIZE_OF_TICKSIZE;
        #endregion

        #region Leg Struct
        public const int SIZE_OF_LEGSTRUCT =
            SIZE_OF_CONTRACTTYPE +
            SIZE_OF_CONTRACT_DATE +
            SIZE_OF_PRICE +
            SIZE_OF_INT +
            SIZE_OF_COMMODITY_NAME;
           // SIZE_OF_COMMODITYNAME + 1 +
           // SIZE_OF_EXPIRY + 1 +
           // SIZE_OF_STRIKEPRICE + 1 +
           // SIZE_OF_RATIO + 1;
        #endregion

        #region Contract Struct
        public const int SIZE_OF_CONTRACT_STRUCT =
            SIZE_OF_COMMODITY_NAME +
            SIZE_OF_CONTRACT_DATE +
            SIZE_OF_EXCHANGE_NAME +
            SIZE_OF_EXPIRY +
            SIZE_OF_LASTTRADEDATE +
            SIZE_OF_INT + // number of legs
            SIZE_OF_INT + // ticks per point
            SIZE_OF_TICKSIZE +
            SIZE_OF_CHAR + // Tradable or not
            SIZE_OF_INT + // GT status
            SIZE_OF_MARGIN +
            SIZE_OF_CHAR + // ESA Template
            SIZE_OF_MARKETREF +
            SIZE_OF_EXCHANGE_NAME +
            SIZE_OF_ARRAY10 +
            SIZE_OF_CONTRACT_DATE +
            2 * (5 * SIZE_OF_ARRAY10);
        #endregion

        #region Order type struct
        public const int SIZE_OF_ORDERTYPE_STRUCT =
            SIZE_OF_ORDERTYPE +
            SIZE_OF_EXCHANGE_NAME +
            SIZE_OF_INT + //order type ID
            SIZE_OF_BYTE + //number of prices required
            SIZE_OF_BYTE + //number of volumes required
            SIZE_OF_BYTE + //number of dates required
            SIZE_OF_CHAR + //Delphi API auto created or not
            SIZE_OF_CHAR + //time triggered or not
            SIZE_OF_CHAR + //exchange synthetic order type or not
            SIZE_OF_CHAR + //GTC or not
            SIZE_OF_TICKETTYPE +
            SIZE_OF_CHAR +
            SIZE_OF_INT +
            SIZE_OF_50;
            

        #endregion

        #region subscribe to contract struct
        public const int SIZE_OF_SUBSCRIBE_STRUCT =
            SIZE_OF_EXCHANGE_NAME +
         //   SIZE_OF_COMMODITYNAME + 1 +
            SIZE_OF_COMMODITY_NAME;
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
                SIZE_OF_PRICE +
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


