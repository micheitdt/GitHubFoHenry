using System;

namespace MarketDataApi
{
    public static class Parameter
    {
        public const string TSE_FORMAT1_HASH_KEY = "TseFormat1";
        public const string TPEX_FORMAT1_HASH_KEY = "TpexFormat1";
        public const string I010_HASH_KEY = "I010";
        public const string TSE_FORMAT6_HASH_KEY = "TseFormat6";
        public const string TPEX_FORMAT6_HASH_KEY = "TpexFormat6";
        public const string TSE_FORMAT17_HASH_KEY = "TseFormat17";
        public const string TPEX_FORMAT17_HASH_KEY = "TpexFormat17";
        public const string I020_HASH_KEY = "I020";
        public const string I080_HASH_KEY = "I080";
        public const string ORIGINAL_MD_HASH_KEY = "OriginalMD";

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
    }
}
