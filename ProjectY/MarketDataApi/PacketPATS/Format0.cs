using System;
using System.Text;

namespace MarketDataApi.PacketPATS
{
    /// <summary>
    /// 盤前資料格式
    /// </summary>
    public class Format0
    {
        public string Exchange { get; set; }
        public string Commodity { get; set; }
        public string Contract { get; set; }

        public Format0(byte[] stream)
        {
            try
            {
                Exchange = Encoding.UTF8.GetString(stream, 0, 11).TrimStart();
                Commodity = Encoding.UTF8.GetString(stream, 11, 11).TrimStart();
                Contract = Encoding.UTF8.GetString(stream, 22, 51).TrimStart();
            }
            catch (Exception)
            {
            }
        }
    }
}
