using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace CommonLibrary
{
    public class GlobalModel
    {
        #region format
        public const int SIZE_OF_INT = 4;
        public const int SIZE_OF_BYTE = 1;
        public const int SIZE_OF_ARRAY20 = 21;
        public const int SIZE_OF_PRICE = 21;
        public const int SIZE_OF_PRICEDETAILSTRUCT =
            SIZE_OF_PRICE +
            SIZE_OF_INT + //Volume
            SIZE_OF_BYTE + //AgeCounter
            SIZE_OF_BYTE + //direction 
            SIZE_OF_BYTE + //hour
            SIZE_OF_BYTE + //minute
            SIZE_OF_BYTE; //second
        public const int SIZE_OF_EXCHANGENAME = 10;
        public const int SIZE_OF_COMMODITYNAME = 10;
        public const int SIZE_OF_CONTRACTDATE = 50;

        /** struct for price update callback**/
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct PriceUpdateStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SIZE_OF_EXCHANGENAME + 1)]
            public string ExchangeName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SIZE_OF_COMMODITYNAME + 1)]
            public string CommodityName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SIZE_OF_CONTRACTDATE + 1)]
            public string ContractDate;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct PriceDetailStruct
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SIZE_OF_ARRAY20)]
            public string Price;       // FP number
            [MarshalAs(UnmanagedType.I4, SizeConst = SIZE_OF_INT)]
            public int Volume;       // does not apply to all price types
            [MarshalAs(UnmanagedType.U1, SizeConst = SIZE_OF_BYTE)]
            public byte AgeCounter;          // if zero, price is "expired"
            [MarshalAs(UnmanagedType.U1, SizeConst = SIZE_OF_BYTE)]
            public byte Direction;          // 0=Same, 1=Rise, 2=Fall
            [MarshalAs(UnmanagedType.U1, SizeConst = SIZE_OF_BYTE)]
            public byte Hour;
            [MarshalAs(UnmanagedType.U1, SizeConst = SIZE_OF_BYTE)]
            public byte Minute;
            [MarshalAs(UnmanagedType.U1, SizeConst = SIZE_OF_BYTE)]
            public byte Second;          // Timestamp

        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct PATStoProxyFormat
        {
            [MarshalAs(UnmanagedType.Struct, SizeConst = 73)]
            public PriceUpdateStruct SymbolInfo;
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
        #endregion
    }
}