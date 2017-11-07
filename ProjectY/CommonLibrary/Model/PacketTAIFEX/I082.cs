using System.Text;

namespace CommonLibrary.Model.PacketTAIFEX
{
    public class I082
    {
        public string H_EscCode { get; set; }
        public string H_TransmissionCode { get; set; }
        public string H_MessageKind { get; set; }
        public int H_InformationTime { get; set; }
        public int H_InformationSeq { get; set; }
        public int H_VersionNo { get; set; }
        public int H_BodyLength { get; set; }

        public string B_ProdId { get; set; }
        public MatchData[] B_BuyOrderBook { get; set; }
        public MatchData[] B_SellOrderBook { get; set; }

        public I082(byte[] stream, int startIndex)
        {
            H_EscCode = Encoding.ASCII.GetString(stream, startIndex, 1);
            H_TransmissionCode = Encoding.ASCII.GetString(stream, startIndex + 1, 1);
            H_MessageKind = Encoding.ASCII.GetString(stream, startIndex + 2, 1);
            H_InformationTime = Functions.ConvertToFormat9(stream, startIndex + 3, 4);
            H_InformationSeq = Functions.ConvertToFormat9(stream, startIndex + 7, 4);
            H_VersionNo = Functions.ConvertToFormat9(stream, startIndex + 11, 1);
            H_BodyLength = Functions.ConvertToFormat9(stream, startIndex + 12, 2);
            B_ProdId = Encoding.ASCII.GetString(stream, startIndex + 14, 20).TrimEnd(' ');
            B_BuyOrderBook = GetMatchData(stream, startIndex + 34);
            B_SellOrderBook = GetMatchData(stream, startIndex + 84);
        }

        //Todo:Check if 委託不足檔數?
        private MatchData[] GetMatchData(byte[] stream, int startIndex)
        {
            var output = new MatchData[5];
            var pointer = 0;
            for (int i = 0; i < 5; i++)
            {
                var md = new MatchData();
                md.Sign = Encoding.ASCII.GetString(stream, startIndex + pointer, 1);
                pointer += 1;
                md.MatchPrice = Functions.ConvertToFormat9(stream, startIndex + pointer, 5);
                pointer += 5;
                md.MatchQuantity = Functions.ConvertToFormat9(stream, startIndex + pointer, 4);
                pointer += 4;
                output[i] = md;
            }
            return output;
        }
    }
}
