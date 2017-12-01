using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Common
{
    public partial class Utility
    {
        public static string TimePath = Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("yyyyMMddHH");

        /// <summary>
        /// 存檔
        /// </summary>
        public static bool SaveData(string FileName, byte[] Data)
        {
            using (FileStream FS = new FileStream(FileName, File.Exists(FileName) ? FileMode.Append : FileMode.OpenOrCreate, FileAccess.Write))
            {
                FS.Write(Data, 0, Data.Length);
                FS.Close();
            }
            return true;
        }

        public static void SaveLog(string value)
        {
            using (StreamWriter sw = File.AppendText(TimePath))
            {
                //開始寫入
                sw.WriteLine(value);
                //清空緩衝區
                sw.Flush();
            }
        }

        /// <summary>
        /// 測試連接
        /// </summary>
        public static bool TestConn(string ip, int port)
        {
            try
            {
                using (System.Net.Sockets.TcpClient tc = new System.Net.Sockets.TcpClient())
                {
                    IAsyncResult result = tc.BeginConnect(ip, port, null, null);
                    DateTime start = DateTime.Now;

                    do
                    {
                        System.Threading.SpinWait.SpinUntil(() => { return false; }, 100);
                        if (result.IsCompleted) break;
                    }
                    while (DateTime.Now.Subtract(start).TotalSeconds < 0.3);

                    if (result.IsCompleted)
                    {
                        tc.EndConnect(result);
                        return true;
                    }

                    tc.Close();

                    if (!result.IsCompleted)
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return false;
        }

        #region Redis
        public static Dictionary<string, byte[]> GetOriginalRedisDB(RedisClient conndb, string hashid)
        {
            if (conndb.GetHashKeys(hashid).Count == 0)
                return null;
            
            return new Dictionary<string, byte[]>(conndb.GetAll<byte[]>(conndb.GetHashKeys(hashid)));
        }
        #endregion

        #region byte control
        public static void SetBytes(ref byte[] data, int offset, double value)
        {
            var array = BitConverter.GetBytes(value);
            for (int i = 0; i < array.Length; i++)
            {
                data[offset + i] = array[i];
            }
        }

        public static void SetBytes(ref byte[] data, int offset, string value, int lengthOfValue)
        {
            var content = value.PadLeft(lengthOfValue);
            var array = Encoding.UTF8.GetBytes(content);
            for (int i = 0; i < array.Length; i++)
            {
                data[offset + i] = array[i];
            }
        }
        /// <summary>
        /// 帶int取得動態byte
        /// </summary>
        public static int SetIntToDynamicBytes(ref byte[] data, int offset, UInt32 value)
        {
            if(value.CompareTo(64) >= 0)
            {
                if(value.CompareTo(16384) >= 0)//(16384~1073741823)
                {
                    if (value >= 1073741824)
                    {
                        return -1;
                    }
                    //方法1
                    //Byte[] byteArray = BitConverter.GetBytes(value);
                    //BitArray bitAry = new BitArray(new byte[] { byteArray[0], byteArray[1], byteArray[2], byteArray[3] });
                    //BitArray retBitAry = new BitArray(new byte[4]);
                    //for (int i = 2; i < bitAry.Count; i++)
                    //{
                    //    retBitAry[i] = bitAry[i - 2];
                    //}
                    ////標示4byte
                    //retBitAry[0] = true;
                    //retBitAry[1] = true;

                    //retBitAry.CopyTo(byteArray, 0);
                    //data[offset] = byteArray[0];
                    //data[offset + 1] = byteArray[1];
                    //data[offset + 2] = byteArray[2];
                    //data[offset + 3] = byteArray[3];

                    //方法2
                    value = value << 2;
                    value |= (uint)(1 << 0 | 1 << 1);
                    Byte[] byteArray = BitConverter.GetBytes(value);
                    data[offset] = byteArray[0];
                    data[offset + 1] = byteArray[1];
                    data[offset + 2] = byteArray[2];
                    data[offset + 3] = byteArray[3];
                    return 4;
                }
                else//(64~16383)
                {
                    //EX：10000001.00000000 => 00100000.10100000(右移2位後。第1bit改1因為使用2byte)
                    //方法1
                    //Byte[] byteArray = BitConverter.GetBytes(value);
                    //BitArray bitAry = new BitArray(new byte[] { byteArray[0], byteArray[1] });
                    //BitArray retBitAry = new BitArray(new byte[2]);
                    //for (int i = 2; i < bitAry.Count; i++)
                    //{
                    //    retBitAry[i] = bitAry[i - 2];
                    //}
                    ////標示2byte
                    //retBitAry[0] = true;
                    //retBitAry[1] = false;

                    //retBitAry.CopyTo(byteArray, 0);
                    //data[offset] = byteArray[0];
                    //data[offset + 1] = byteArray[1];

                    //方法2
                    value = value << 2;
                    value |= (uint)(1 << 0);
                    Byte[] byteArray = BitConverter.GetBytes(value);
                    data[offset] = byteArray[0];
                    data[offset + 1] = byteArray[1];
                    return 2;
                }
            }
            else//(0~63)
            {
                //方法1
                //Byte[] byteArray = BitConverter.GetBytes(value);
                //BitArray bitAry = new BitArray(new byte[] { byteArray[0]});
                //BitArray retBitAry = new BitArray(new byte[1]);
                //for (int i = 2; i < bitAry.Count; i++)
                //{
                //    retBitAry[i] = bitAry[i - 2];
                //}
                ////標示1byte
                //retBitAry[0] = false;
                //retBitAry[1] = false;

                //retBitAry.CopyTo(byteArray, 0);
                //data[offset] = byteArray[0];

                //方法2
                value = value << 2;
                Byte[] byteArray = BitConverter.GetBytes(value);
                data[offset] = byteArray[0];
                return 1;
            }
        }
        /// <summary>
        /// 帶動態byte取得int值
        /// </summary>
        public static uint GetIntToDynamicBytes(byte[] data, int offset)
        {
            //BitArray bitAry = new BitArray(new byte[] { data[offset] });
            //if(bitAry[0] == false)//1 byte
            //{
            //    BitArray retBitAry = new BitArray(new byte[] { data[offset + 0] });
            //    for (int i = 2; i < 8; i++)
            //    {
            //        retBitAry[i - 2] = retBitAry[i];
            //    }
            //    retBitAry[6] = false;
            //    retBitAry[7] = false;
            //    byte[] ret = new byte[1];
            //    retBitAry.CopyTo(ret, 0);
            //    return ret[0];
            //}
            //else
            //{
            //    if (bitAry[1] == false)//2 byte
            //    {
            //        BitArray retBitAry = new BitArray(new byte[] { data[offset + 0], data[offset + 1] });

            //        for(int i = 2; i < 16; i++)
            //        {
            //            retBitAry[i - 2] = retBitAry[i];
            //        }
            //        retBitAry[14] = false;
            //        retBitAry[15] = false;
            //        byte[] ret = new byte[2];
            //        retBitAry.CopyTo(ret, 0);
            //        return ret[0] | (ret[1] << 8);
            //    }
            //    else//4 byte
            //    {
            //        BitArray retBitAry = new BitArray(new byte[] { data[offset + 0], data[offset + 1], data[offset + 2], data[offset + 3] });

            //        for (int i = 2; i < 32; i++)
            //        {
            //            retBitAry[i - 2] = retBitAry[i];
            //        }
            //        retBitAry[30] = false;
            //        retBitAry[31] = false;
            //        byte[] ret = new byte[4];
            //        retBitAry.CopyTo(ret, 0);
            //        return ret[0] | (ret[1] << 8) | ret[2] << 16 | (ret[3] << 32);
            //    }
            //}
            byte b = data[offset];
            var is2Byte = (b >> 0) & 1;
            var is4Byte = (b >> 1) & 1;

            if (is2Byte == 0)
            {
                return (uint)(data[offset] >> 2);
            }
            else if (is2Byte == 1 && is4Byte == 0)
            {
                return (uint)(BitConverter.ToUInt16(data, offset) >> 2);
            }
            else
            {
                return (uint)(BitConverter.ToUInt32(data, offset) >> 2);
            }
        }
        /// <summary>
        /// string to byte array
        /// </summary>
        public static int SetStringToDynamicBytes(ref byte[] data, int offset, string value)
        {
            Byte[] byteArray = Encoding.Default.GetBytes(value);
            byteArray.CopyTo(data, 1);
            return byteArray.Count() + 1;
        }
        /// <summary>
        /// byte array to string
        /// </summary>
        public static string GetStringToDynamicBytes(byte[] data, int offset)
        {
           string retValue = Encoding.Default.GetString(data.Take(data[0]).ToArray());
            return retValue;
        }
        /// <summary>
        /// 帶double取得動態byte
        /// </summary>
        public static int SetDoubleToDynamicBytes(ref byte[] data, int offset, decimal price, decimal baseprice, decimal tagsize,ref uint count)
        {
            count = 0;
            int compartValue = price.CompareTo(baseprice);
            bool isAdd = false;//設最左bit 1=+ or 0=-
            switch (compartValue)
            {
                case 1://實價 > 基價
                    count = (uint)((price - baseprice) / tagsize);
                    isAdd = true;
                    break;
                case -1://實價 < 基價
                    count = (uint)((baseprice - price) / tagsize);
                    break;
                case 0://實價 = 基價
                    data[offset] = 0x00;
                    return 1;
            }
            if (count.CompareTo(32) >= 0)
            {
                if (count.CompareTo(8192) >= 0)//(8192~536870911)
                {
                    if(count >= 536870912)
                    {
                        return -1;
                    }
                    //標示4byte
                    //方法1
                    //Byte[] byteArray = BitConverter.GetBytes(count);
                    //BitArray bitAry = new BitArray(new byte[] { byteArray[0], byteArray[1], byteArray[2], byteArray[3] });
                    //BitArray retBitAry = new BitArray(new byte[4]);
                    //for (int i = 3; i < bitAry.Count; i++)
                    //{
                    //    retBitAry[i] = bitAry[i - 3];
                    //}
                    //retBitAry[0] = isAdd;
                    //retBitAry[1] = true;
                    //retBitAry[2] = true;

                    //retBitAry.CopyTo(byteArray, 0);
                    //data[offset] = byteArray[0];
                    //data[offset + 1] = byteArray[1];
                    //data[offset + 2] = byteArray[2];
                    //data[offset + 3] = byteArray[3];

                    //方法2
                    count = count << 3;
                    count |= (uint)((isAdd ? (1 << 0) : (0 << 0)) | (1 << 1) | (1 << 2));
                    Byte[] byteArray = BitConverter.GetBytes(count);
                    data[offset] = byteArray[0];//(byte)(byteArray[0] | (isAdd ? (1 << 7) : (0 << 7)) | (1 << 6) | (1 << 5));
                    data[offset + 1] = byteArray[1];
                    data[offset + 2] = byteArray[2];
                    data[offset + 3] = byteArray[3];
                    return 4;
                }
                else//(32~8191)
                {
                    //標示2byte

                    //方法1
                    //Byte[] byteArray = BitConverter.GetBytes(count);
                    //BitArray bitAry = new BitArray(new byte[] { byteArray[0], byteArray[1] });
                    //BitArray retBitAry = new BitArray(new byte[2]);
                    //for (int i = 3; i < bitAry.Count; i++)
                    //{
                    //    retBitAry[i] = bitAry[i - 3];
                    //}
                    //retBitAry[0] = isAdd;
                    //retBitAry[1] = true;
                    //retBitAry[2] = false;

                    //retBitAry.CopyTo(byteArray, 0);
                    //data[offset] = byteArray[0];
                    //data[offset + 1] = byteArray[1];

                    //方法2
                    count = count << 3;
                    count |= (uint)((isAdd ? (1 << 0) : (0 << 0)) | (1 << 1));
                    Byte[] byteArray = BitConverter.GetBytes(count);
                    data[offset] = byteArray[0];//(byte)(byteArray[0] | (isAdd ? (1 << 7) : (0 << 7)) | (1 << 6));
                    data[offset + 1] = byteArray[1];
                    return 2;
                }
            }
            else//(1~31)
            {
                //標示1byte
                //Byte[] byteArray = BitConverter.GetBytes(count);
                //BitArray bitAry = new BitArray(new byte[] { byteArray[0] });
                //BitArray retBitAry = new BitArray(new byte[1]);
                //for (int i = 3; i < bitAry.Count; i++)
                //{
                //    retBitAry[i] = bitAry[i - 3];
                //}
                ////標示1byte
                //retBitAry[0] = isAdd;
                //retBitAry[1] = false;
                //retBitAry[2] = false;

                //retBitAry.CopyTo(byteArray, 0);
                //data[offset] = byteArray[0];
                count = count << 3;
                count |= (uint)((isAdd ? (1 << 0) : (0 << 0)));
                Byte[] byteArray = BitConverter.GetBytes(count);
                data[offset] = byteArray[0];
                return 1;
            }
        }
        /// <summary>
        /// 帶動態byte取得double
        /// </summary>
        public static decimal GetDoubleToDynamicBytes(ref byte[] data, int offset, decimal baseprice, decimal tagsize, ref uint count)
        {
            //方法1
            //BitArray bitAry = new BitArray(new byte[] { data[offset] });
            //if (bitAry[1] == false)
            //{
            //    BitArray retBitAry = new BitArray(new byte[] { data[offset + 0] });
            //    for (int i = 3; i < 8; i++)
            //    {
            //        retBitAry[i -3] = retBitAry[i];
            //    }
            //    retBitAry[5] = false;
            //    retBitAry[6] = false;
            //    retBitAry[7] = false;
            //    byte[] ret = new byte[1];
            //    retBitAry.CopyTo(ret, 0);

            //    count = ret[0];
            //    return (bitAry[0])? baseprice + (count * tagsize) : baseprice - (count * tagsize);
            //}
            //else if(bitAry[1] && bitAry[2] == false)
            //{
            //    BitArray retBitAry = new BitArray(new byte[] { data[offset + 0], data[offset + 1] });
            //    for (int i = 3; i < 16; i++)
            //    {
            //        retBitAry[i - 3] = retBitAry[i];
            //    }
            //    retBitAry[15] = false;
            //    retBitAry[14] = false;
            //    retBitAry[13] = false;
            //    byte[] ret = new byte[2];
            //    retBitAry.CopyTo(ret, 0);

            //    count = BitConverter.ToUInt16(ret,0);
            //    return (bitAry[0]) ? baseprice + (count * tagsize) : baseprice - (count * tagsize);
            //}
            //else
            //{
            //    BitArray retBitAry = new BitArray(new byte[] { data[offset + 0], data[offset + 1], data[offset + 2], data[offset + 3] });

            //    for (int i = 3; i < 32; i++)
            //    {
            //        retBitAry[i - 3] = retBitAry[i];
            //    }
            //    retBitAry[29] = false;
            //    retBitAry[30] = false;
            //    retBitAry[31] = false;
            //    byte[] ret = new byte[4];
            //    retBitAry.CopyTo(ret, 0);
            //    count = BitConverter.ToUInt32(ret,0);
            //    return (bitAry[0]) ? baseprice + (count * tagsize) : baseprice - (count * tagsize);
            //}
            byte b = data[offset];
            var isAdd = (b >> 0) & 1;//x0000000
            var mark1 = (b >> 1) & 1;//0x000000
            var mark2 = (b >> 2) & 1;//00x00000
            if(mark1 == 0)
            {
                count = (uint) (data[offset] >> 3);
                return (isAdd==1) ? baseprice + (count * tagsize) : baseprice - (count * tagsize);
            }
            else if (mark1 == 1 && mark2 == 0)
            {
                count = (uint)(BitConverter.ToUInt16(data, offset) >> 3);
                return (isAdd == 1) ? baseprice + (count * tagsize) : baseprice - (count * tagsize);
            }
            else
            {
                count = (uint)(BitConverter.ToUInt32(data, offset) >> 3);
                return (isAdd == 1) ? baseprice + (count * tagsize) : baseprice - (count * tagsize);
            }
        }

        /// <summary>
        /// 裁切Byte array資料
        /// </summary>
        public static byte[] ByteGetSubArray(byte[] input, int index_start, int length)
        {
            byte[] newArray;
            newArray = new byte[length];
            Array.Copy(input, index_start, newArray, 0, length);
            return newArray;
        }
        #endregion
    }
    //--------------------------------------------------------------------
    public static class Extension
    {
        static Extension()
        {
        }

        public readonly static System.Text.RegularExpressions.Regex REG_NUMBER = new System.Text.RegularExpressions.Regex("^[0-9]+(.[0-9]{0,4})?$");

        /// <summary>
        /// 取得說明文字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

            return attribute != null ? attribute.Description : value.ToString();
        }

        /// <summary>
        /// 判斷NullableType基底型別是否為Enum
        /// </summary>
        /// <param name="type"></param>
        /// <returns>基底型別是否為Enum</returns>
        public static bool IsNullableEnum(this Type type)
        {
            Type u = Nullable.GetUnderlyingType(type);
            return (u != null) && u.IsEnum;
        }

        /// <summary>
        /// Toes the raw string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToRawString(this Guid value)
        {
            return value.ToString().Replace("-", "");
        }

        /// <summary>
        /// Hmmssfff 左邊補0補滿9位 HHmmssfff 格式化為 HH:mm:ss.fff
        /// Hmmssffffff 左邊補0補滿12位 HHmmssffffff 格式化為 HH:mm:ss.ffffff
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string RawToFormatTime(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string time = string.Empty;
            switch (value.Length)
            {
                case 8:
                case 9:
                    value = value.PadLeft(9, '0');
                    time = string.Format("{0}:{1}:{2}.{3}", value.Substring(0, 2), value.Substring(2, 2), value.Substring(4, 2), value.Substring(6));
                    break;
                case 11:
                case 12:
                    value = value.PadLeft(12, '0');
                    time = string.Format("{0}:{1}:{2}.{3}", value.Substring(0, 2), value.Substring(2, 2), value.Substring(4, 2), value.Substring(6));
                    break;
                default:
                    break;
            }

            return time;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetBytesBase64String(this string value)
        {
            string result;

            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, value);
                byte[] bs = stream.ToArray();
                result = Convert.ToBase64String(bs);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetJsonStringFromBase64String(this string value)
        {
            string result;
            byte[] mData = Convert.FromBase64String(value);

            using (MemoryStream stream = new MemoryStream(mData))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                stream.Seek(0, SeekOrigin.Begin);
                result = (string)formatter.Deserialize(stream);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digitsAfterPoint"></param>
        /// <returns></returns>
        public static decimal CeilingAfterPoint(this decimal number, int digitsAfterPoint)
        {
            return Math.Ceiling(number * (decimal)Math.Pow(10, digitsAfterPoint)) / (decimal)Math.Pow(10, digitsAfterPoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digitsAfterPoint"></param>
        /// <returns></returns>
        public static decimal FloorAfterPoint(this decimal number, int digitsAfterPoint)
        {
            return Math.Floor(number * (decimal)Math.Pow(10, digitsAfterPoint)) / (decimal)Math.Pow(10, digitsAfterPoint);
        }

        /// <summary>
        /// 檢查是否有全形字
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsFullWord(this string words)
        {
            bool result = false;
            //string pattern = @"^[\u4E00-\u9fa5]+$";

            foreach (char item in words)
            {
                //以16進位值長度判斷是否為全形字
                if (string.Format("{0:X}", Convert.ToInt32(item)).Length != 2)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        #region 數字調整 高於10萬顯示"萬"字

        /// <summary>
        /// 數字調整 高於10萬顯示"萬"字
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToTenThousandString(this decimal value)
        {
            return TenThousandStringParse(value);
        }

        /// <summary>
        /// 數字調整 高於10萬顯示"萬"字
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string ToTenThousandString(this long value)
        {
            return TenThousandStringParse(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string TenThousandStringParse(object value)
        {
            decimal input;
            if (decimal.TryParse(value.ToString(), out input))
            {
                if (input / 100000 < 1)
                {
                    return value.ToString();
                }
                else
                {
                    return (input / 10000).ToString("N1") + "萬";
                }
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion 數字調整 高於10萬顯示"萬"字

        /// <summary>
        /// byte陣列轉結構
        /// </summary>
        /// <param name="byteArray"></param>
        /// <param name="obj"></param>
        public static void ByteArrayToStructure(byte[] byteArray, ref object obj)
        {
            int len = Marshal.SizeOf(obj);
            IntPtr i = Marshal.AllocHGlobal(len);
            try
            {
                Marshal.Copy(byteArray, 0, i, len);
                obj = Marshal.PtrToStructure(i, obj.GetType());
            }
            finally
            {
                Marshal.FreeHGlobal(i);
            }
        }

        /// <summary>
        /// 轉換BCD資料
        /// </summary>
        /// <param name="src"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static long FromBCDToInt64(this byte[] src, int startIndex, int count)
        {
            if ((count + startIndex) > src.Length)
            {
                throw new IndexOutOfRangeException();
            }

            long result = 0;

            for (int i = 0; i < count; i++)
            {
                result *= 100;
                result += (10 * (src[startIndex + i] >> 4));
                result += (src[startIndex + i] & 0xf);
            }

            return result;
        }

        /// <summary>
        /// byte[] 資料複製
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static void CopyDataFromSource(this byte[] dst, int dstOffset, byte[] src, int srcOffset, int count)
        {
            Buffer.BlockCopy(src, srcOffset, dst, dstOffset, count);
        }
    }
}
