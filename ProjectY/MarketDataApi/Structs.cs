using System.Runtime.InteropServices;

namespace MarketDataApi
{
    public struct PriceVolume
    {
        public int Price;
        public int Volume;
    }

    public struct MatchData
    {
        public string Sign;
        public int MatchPrice;
        public int MatchQuantity;
    }
    
    public struct PriceVolumeS
    {
        public string Price;
        public int Volume;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PriceUpdateStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Parameter.SIZE_OF_EXCHANGENAME + 1)]
        public string ExchangeName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Parameter.SIZE_OF_COMMODITYNAME + 1)]
        public string CommodityName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Parameter.SIZE_OF_CONTRACTDATE + 1)]
        public string ContractDate;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PatsTickerMD
    {
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public string LastPrice;
         [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int LastVolume;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PatsDepthMD
    {
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte HH;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte MM;
        [MarshalAs(UnmanagedType.U1, SizeConst = 1)]
        public byte SS;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int TotalVolume;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double HighPrice;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double LowPrice;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OpeningPrice;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double CloseingPrice;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice0;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume0;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice0;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume0;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice1;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume1;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice1;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume1;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice2;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume2;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice2;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume2;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice3;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume3;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice3;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume3;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice4;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume4;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice4;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume4;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice5;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume5;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice5;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume5;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice6;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume6;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice6;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume6;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice7;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume7;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice7;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume7;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice8;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume8;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice8;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume8;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double BidDOMPrice9;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int BidDOMVolume9;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double OfferDOMPrice9;
        [MarshalAs(UnmanagedType.I4, SizeConst = 4)]
        public int OfferDOMVolume9;
        [MarshalAs(UnmanagedType.R8, SizeConst = 8)]
        public double ReferencePrice;
        
    }
}
