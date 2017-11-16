using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace CommonLibrary.Model.PacketPATS
{
    /// <summary>
    /// 盤前資料格式
    /// </summary>
    [Serializable]
    public class Format0
    {
        #region Logger
        /// <summary>
        /// 記錄器
        /// </summary>
        private static Logger _logger = LogManager.GetCurrentClassLogger();
        #endregion Logger
        
        public string Exchange { get; set; }
        public string Commodity { get; set; }
        public string Contract { get; set; }
        //public PriceUpdateStruct PriceData { get; set; }

        public Format0(byte[] stream)
        {
            try
            {
                //PriceUpdateStruct priceData = PriceUpdateStructFromBytes(stream);
                Exchange = Encoding.UTF8.GetString(stream, 0, 11).TrimStart();
                Commodity = Encoding.UTF8.GetString(stream, 11, 11).TrimStart();
                Contract = Encoding.UTF8.GetString(stream, 22, 51).TrimStart();
            }
            catch (Exception err)
            {
                _logger.Error(err, string.Format("Format0(): ErrMsg = {0}.", err.Message));
            }
        }

        PriceUpdateStruct PriceUpdateStructFromBytes(byte[] arr)
        {
            PriceUpdateStruct str = new PriceUpdateStruct();

            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            str = (PriceUpdateStruct)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }
    }
}
