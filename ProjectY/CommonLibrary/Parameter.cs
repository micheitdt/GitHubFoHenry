using System;

namespace CommonLibrary
{
    public static class Parameter
    {
        public const string TSE_HASH_KEY = "TseFormat1";
        public const string TPEX_HASH_KEY = "TpexFormat1";
        public const string TAIFEX_HASH_KEY = "I010";

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
