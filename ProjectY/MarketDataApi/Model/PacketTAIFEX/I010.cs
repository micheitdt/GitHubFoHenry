using System;
using System.Collections.Generic;
using System.Text;

namespace MarketDataApi.Model.PacketTAIFEX
{
    [Serializable]
    public class I010
    {
        public string H_EscCode { get; set; }
        public string H_TransmissionCode { get; set; }
        public string H_MessageKind { get; set; }
        public int H_InformationTime { get; set; }
        public int H_InformationSeq { get; set; }
        public int H_VersionNo { get; set; }
        public int H_BodyLength { get; set; }
        public string B_ProdId { get; set; }//商品代號
        public int B_RiseLimitPrice1 { get; set; }//第一漲停價
        public int B_ReferencePrice { get; set; }//參考價
        public int B_FallLimitPrice1 { get; set; }//第一跌停價
        public int B_RiseLimitPrice2 { get; set; }//第二漲停價
        public int B_FallLimitPrice2 { get; set; }//第二跌停價
        public int B_RiseLimitPrice3 { get; set; }//第三漲停價
        public int B_FallLimitPrice3 { get; set; }//第三跌停價
        public string B_ProKind { get; set; }//契約種類 I:指數類  R:利率類  B:債券類  C:商品類  S:股票類  E:匯率類
        public int B_DecimalLocator { get; set; }//價格欄位小數位數
        public int B_StrikePriceDecimalLocator { get; set; }//選擇權商品代號之履約價格小數位數
        public int B_BeginDate { get; set; }//上市日期
        public int B_EndDate { get; set; }//下市日期
        public int B_FlowGroup { get; set; }//流程群組
        public int B_DeliveryDate { get; set; }//最後結算日
        //public int T_CheckSum { get; set; }//檢核碼
        //public string T_TerminalCode { get; set; }

        public I010(byte[] stream, int startIndex)
        {

            try
            {
                H_EscCode = Encoding.ASCII.GetString(stream, startIndex, 1);
                H_TransmissionCode = Encoding.ASCII.GetString(stream, startIndex + 1, 1);
                H_MessageKind = Encoding.ASCII.GetString(stream, startIndex + 2, 1);
                H_InformationTime = Functions.ConvertToFormat9(stream, startIndex + 3, 4);
                H_InformationSeq = Functions.ConvertToFormat9(stream, startIndex + 7, 4);
                H_VersionNo = Functions.ConvertToFormat9(stream, startIndex + 11, 1);
                H_BodyLength = Functions.ConvertToFormat9(stream, startIndex + 12, 2);
                if (H_BodyLength > stream.Length - 12)
                {
                    return;
                }

                B_ProdId = Encoding.ASCII.GetString(stream, startIndex + 14, 10).TrimEnd(' ');
                B_RiseLimitPrice1 = Functions.ConvertToFormat9(stream, startIndex + 24, 5);
                B_ReferencePrice = Functions.ConvertToFormat9(stream, startIndex + 29, 5);
                B_FallLimitPrice1 = Functions.ConvertToFormat9(stream, startIndex + 34, 5);
                B_RiseLimitPrice2 = Functions.ConvertToFormat9(stream, startIndex + 39, 5);
                B_FallLimitPrice2 = Functions.ConvertToFormat9(stream, startIndex + 44, 5);
                B_RiseLimitPrice3 = Functions.ConvertToFormat9(stream, startIndex + 49, 5);
                B_FallLimitPrice3 = Functions.ConvertToFormat9(stream, startIndex + 54, 5);
                B_ProKind = Encoding.ASCII.GetString(stream, startIndex + 59, 1);
                B_DecimalLocator = Functions.ConvertToFormat9(stream, startIndex + 60, 1);
                B_StrikePriceDecimalLocator = Functions.ConvertToFormat9(stream, startIndex + 61, 1);
                B_BeginDate = Functions.ConvertToFormat9(stream, startIndex + 62, 4);
                B_EndDate = Functions.ConvertToFormat9(stream, startIndex + 66, 4);
                B_FlowGroup = Functions.ConvertToFormat9(stream, startIndex + 70, 1);
                B_DeliveryDate = Functions.ConvertToFormat9(stream, startIndex + 71, 4);
                //T_CheckSum = Functions.ConvertToFormat9(stream, startIndex + 75, 1);
                //T_TerminalCode = Encoding.ASCII.GetString(stream, startIndex + 76, 2);
            }
            catch (Exception)
            {
            }
        }
    }
}
