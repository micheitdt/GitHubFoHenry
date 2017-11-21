
namespace MarketDataApi.Model.PacketTPEX
{
    public class StatusInfo
    {
        public bool IsPreMatch { get; set; }
        public bool IsLateOpen { get; set; }
        public bool IsLateClose { get; set; }
        public bool IsContinuousBidding { get; set; }
        //public bool IsOpen { get; set; }
        //public bool IsClose { get; set; }
        public StatusInfo(byte data)
        {
            IsPreMatch = Functions.GetBit(data, 7);
            IsLateOpen = Functions.GetBit(data, 6);
            IsLateClose = Functions.GetBit(data, 5);
            IsContinuousBidding = Functions.GetBit(data, 4);
            //IsOpen = Functions.GetBit(data, 3);
            //IsClose = Functions.GetBit(data, 2);
        }
    }
}
