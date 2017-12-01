using System.Collections.Generic;
using System.Text;

namespace MarketDataApi.PacketTAIFEX
{
    public class I020
    {
        public string H_EscCode { get; set; }
        public string H_TransmissionCode { get; set; }
        public string H_MessageKind { get; set; }
        public int H_InformationTime { get; set; }
        public int H_InformationSeq { get; set; }
        public int H_VersionNo { get; set; }
        public int H_BodyLength { get; set; }
        public string B_ProdId { get; set; }
        public int B_MatchTime { get; set; }
        public string B_Sign { get; set; }
        public int B_FirstMatchPrice { get; set; }
        public int B_FirstMatchQnty { get; set; }
        public int B_MatchDisplayItem { get; set; } //128正常  <128?
        public List<MatchData> B_MatchData { get; set; }
        public int B_MatchTotalQty { get; set; }
        public int B_MatchBuyCnt { get; set; }
        public int B_MatchSellCnt { get; set; }
        public int B_StatusCode { get; set; }
        public int T_CheckSum { get; set; }
        public string T_TerminalCode { get; set; }

        public I020(byte[] stream, int startIndex)
        {
            H_EscCode = Encoding.ASCII.GetString(stream, startIndex, 1);
            H_TransmissionCode = Encoding.ASCII.GetString(stream, startIndex + 1, 1);
            H_MessageKind = Encoding.ASCII.GetString(stream, startIndex + 2, 1);
            H_InformationTime = Functions.ConvertToFormat9(stream, startIndex + 3, 4);
            H_InformationSeq = Functions.ConvertToFormat9(stream, startIndex + 7, 4);
            H_VersionNo = Functions.ConvertToFormat9(stream, startIndex + 11, 1);
            H_BodyLength = Functions.ConvertToFormat9(stream, startIndex + 12, 2);
            B_ProdId = Encoding.ASCII.GetString(stream, startIndex + 14, 20).TrimEnd(' ');
            B_MatchTime = Functions.ConvertToFormat9(stream, startIndex + 34, 4);
            B_Sign = Encoding.ASCII.GetString(stream, startIndex + 38, 1);
            B_FirstMatchPrice = Functions.ConvertToFormat9(stream, startIndex + 39, 5);
            B_FirstMatchQnty = Functions.ConvertToFormat9(stream, startIndex + 44, 4);
            B_MatchDisplayItem = stream[startIndex + 48] - 128;
            int pointer = 0;
            B_MatchData = GetMatchData(stream, startIndex + 49, B_MatchDisplayItem, ref pointer);
            B_MatchTotalQty = Functions.ConvertToFormat9(stream, startIndex + 49 + pointer, 4);
            B_MatchBuyCnt = Functions.ConvertToFormat9(stream, startIndex + 53 + pointer, 4);
            B_MatchSellCnt = Functions.ConvertToFormat9(stream, startIndex + 57 + pointer, 4);
            B_StatusCode = Functions.ConvertToFormat9(stream, startIndex + 61 + pointer, 1);
            T_CheckSum = Functions.ConvertToFormat9(stream, startIndex + 62 + pointer, 1);
            T_TerminalCode = Encoding.ASCII.GetString(stream, startIndex + 63 + pointer, 2);
        }

        private List<MatchData> GetMatchData(byte[] stream, int startIndex, int matchItem, ref int pointer)
        {
            var output = new List<MatchData>();
            if (matchItem == 0)
            {
                //Do nothing
            }
            else if (matchItem > 0)
            {
                for (int i = 1; i <= matchItem; i++)
                {
                    var md = new MatchData();
                    md.Sign = Encoding.ASCII.GetString(stream, startIndex + pointer, 1);
                    pointer += 1;
                    md.MatchPrice = Functions.ConvertToFormat9(stream, startIndex + pointer, 5);
                    pointer += 5;
                    md.MatchQuantity = Functions.ConvertToFormat9(stream, startIndex + pointer, 2);
                    pointer += 2;
                    output.Add(md);
                }
            }
            else
            {
                //Something wrong here
            }
            return output;
        }
    }
}
