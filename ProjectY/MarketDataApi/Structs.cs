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
}
