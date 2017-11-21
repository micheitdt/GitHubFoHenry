using ServiceStack.Net30.Collections.Concurrent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CommonLibrary.Model.PacketTSE
{
    public class Format17
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
        public Format17(byte[] data)
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
    public class TseFormat17List : ConcurrentDictionary<string, Format17>
    {
        static TseFormat17List()
        {
            AllTseFormat17List = new TseFormat17List();
        }

        public static TseFormat17List AllTseFormat17List
        {
            get;
            internal set;
        }

        public static ObservableCollection<Format17> GetAllTseFormat17ListCollection()
        {
            return new ObservableCollection<Format17>(AllTseFormat17List.Values);
        }

        public static void AddTseFormat17Data(Format17 data)
        {
            AllTseFormat17List.TryAdd(data.StockID, data);
        }

        public static void SetTseFormat17List(IDictionary<string, Format17> data)
        {
            AllTseFormat17List = new TseFormat17List();
            foreach (var obj in data)
            {
                AllTseFormat17List.TryAdd(obj.Key, obj.Value);
            }
        }
    }
}
