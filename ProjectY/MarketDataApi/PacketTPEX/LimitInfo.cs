
namespace MarketDataApi.PacketTPEX
{
    public class LimitInfo
    {
        public LimitStatus MatchStatus { get; set; }
        public LimitStatus BestBidStatus { get; set; }
        public LimitStatus BestAskStatus { get; set; }
        public PriceTrend Trend { get; set; }
        public LimitInfo(byte data)
        {
            MatchStatus = (LimitStatus)Functions.BitsToInt(Functions.GetBit(data, 7), Functions.GetBit(data, 6));
            BestBidStatus = (LimitStatus)Functions.BitsToInt(Functions.GetBit(data, 5), Functions.GetBit(data, 4));
            BestAskStatus = (LimitStatus)Functions.BitsToInt(Functions.GetBit(data, 3), Functions.GetBit(data, 2));
            Trend = (PriceTrend)Functions.BitsToInt(Functions.GetBit(data, 1), Functions.GetBit(data, 0));
        }

        public enum LimitStatus
        {
            Normal = 0,
            Lower = 1,
            Upper = 2
        }

        public enum PriceTrend
        {
            Normal = 0,
            StopAndDown = 1,
            StopAndUp = 2,
            Unknown = 3
        }
    }
}
