using ServiceStack.Net30.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CommonLibrary.Model.PacketTPEX
{
    public class Format6
    {
        public Header Header;
        public string StockID { get; set; }
        public long MatchTime { get; set; }
        public MatchInfo MatchInfo { get; set; }
        public LimitInfo LimitInfo { get; set; }
        public StatusInfo StatusInfo { get; set; }
        public int TotalVolume { get; set; }
        public int LastPrice { get; set; }
        public int LastVolume { get; set; }
        public List<PriceVolume> BidData { get; set; }
        public List<PriceVolume> AskData { get; set; }
        public Format6(byte[] data)
        {
            Header = new Header(data);
            StockID = Encoding.ASCII.GetString(data, 10, 6).Trim();
            MatchTime = Functions.ConvertToFormat9L(data, 16, 6);
            MatchInfo = new MatchInfo(data[22]);
            LimitInfo = new LimitInfo(data[23]);
            StatusInfo = new StatusInfo(data[24]);
            TotalVolume = Functions.ConvertToFormat9(data, 25, 4);
            var offset = 0;
            if (MatchInfo.WithPriceVolume == true)
            {
                LastPrice = Functions.ConvertToFormat9(data, 29 + offset, 3);
                offset += 3;
                LastVolume = Functions.ConvertToFormat9(data, 29 + offset, 4);
                offset += 4;
            }
            BidData = new List<PriceVolume>();
            for (int i = 0; i < MatchInfo.BidLaders; i++)
            {
                var pv = new PriceVolume();
                pv.Price = Functions.ConvertToFormat9(data, 29 + offset, 3);
                offset += 3;
                pv.Volume = Functions.ConvertToFormat9(data, 29 + offset, 4);
                offset += 4;
                BidData.Add(pv);
            }
            AskData = new List<PriceVolume>();
            for (int i = 0; i < MatchInfo.AskLaders; i++)
            {
                var pv = new PriceVolume();
                pv.Price = Functions.ConvertToFormat9(data, 29 + offset, 3);
                offset += 3;
                pv.Volume = Functions.ConvertToFormat9(data, 29 + offset, 4);
                offset += 4;
                AskData.Add(pv);
            }
        }
    }

    [Serializable]
    public class TpexFormat6List : ConcurrentDictionary<string, Format6>
    {
        static TpexFormat6List()
        {
            AllTpexFormat6List = new TpexFormat6List();
        }

        public static TpexFormat6List AllTpexFormat6List
        {
            get;
            internal set;
        }

        public static ObservableCollection<Format6> GetAllTpexFormat6ListCollection()
        {
            return new ObservableCollection<Format6>(AllTpexFormat6List.Values);
        }

        public static void AddTpexFormat6Data(Format6 data)
        {
            AllTpexFormat6List.TryAdd(data.StockID, data);
        }

        public static void SetTpexFormat6List(IDictionary<string, Format6> data)
        {
            AllTpexFormat6List = new TpexFormat6List();
            foreach (var obj in data)
            {
                AllTpexFormat6List.TryAdd(obj.Key, obj.Value);
            }
        }
    }
}
