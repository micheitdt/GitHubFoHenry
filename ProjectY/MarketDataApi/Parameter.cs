using System;

namespace MarketDataApi
{
    public static class Parameter
    {
        public const string TSE_FORMAT1_HASH_KEY = "0#1";
        public const string TPEX_FORMAT1_HASH_KEY = "1#1";
        public const string I010_HASH_KEY = "11";
        public const string TSE_FORMAT6_HASH_KEY = "0#6";
        public const string TPEX_FORMAT6_HASH_KEY = "1#6";
        public const string TSE_FORMAT17_HASH_KEY = "0#17";
        public const string TPEX_FORMAT17_HASH_KEY = "1#17";
        public const string I022_HASH_KEY = "27";
        public const string I082_HASH_KEY = "28";
        public const string I020_HASH_KEY = "21";
        public const string I080_HASH_KEY = "22";

        public const string ORIGINAL_MD_HASH_KEY = "OriginalMD";

        public const int SIZE_OF_EXCHANGENAME = 10;
        public const int SIZE_OF_COMMODITYNAME = 10;
        public const int SIZE_OF_CONTRACTDATE = 50;
    }
}
