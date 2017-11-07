
namespace CommonLibrary.Model.PacketTSE
{
    public class MatchInfo
    {
        public bool WithPriceVolume { get; set; }
        public int BidLaders { get; set; }
        public int AskLaders { get; set; }
        public bool IsOnlyTick { get; set; }
        public MatchInfo(byte data)
        {
            WithPriceVolume = Functions.GetBit(data, 7);
            IsOnlyTick = Functions.GetBit(data, 0);
            BidLaders = Functions.BitsToInt(Functions.GetBit(data, 6), Functions.GetBit(data, 5), Functions.GetBit(data, 4));
            AskLaders = Functions.BitsToInt(Functions.GetBit(data, 3), Functions.GetBit(data, 2), Functions.GetBit(data, 1));
        }
    }
}
