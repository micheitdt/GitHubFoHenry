using CommonLibrary.Model;
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
using MarketDataApi;
using System.Collections;

namespace CommonLibrary
{
    public partial class Utility
    {
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

        public static void SaveLog(string value, string path = "")
        {
            if(string.IsNullOrEmpty( path))
            {
                path = Environment.CurrentDirectory + "\\" + DateTime.Today.ToString("yyyyMMddHH");
            }

            using (StreamWriter sw = File.AppendText(path))
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
        /// <summary>
        /// 存RedisDB資料
        /// </summary>
        public static void SetRedisDB(RedisClient conndb, string hashid, string key, object data)
        {
            //方法1
            var switchTypeAction = new Dictionary<Type, Action>
            {
                { typeof(MarketDataApi.Model.PacketTSE.Format1), () => { conndb.Set<MarketDataApi.Model.PacketTSE.Format1>(key, data as MarketDataApi.Model.PacketTSE.Format1); conndb.SetEntryInHash(hashid, key, ""); } },
                { typeof(MarketDataApi.Model.PacketTPEX.Format1), () => {  conndb.Set<MarketDataApi.Model.PacketTPEX.Format1>(key, data as MarketDataApi.Model.PacketTPEX.Format1); conndb.SetEntryInHash(hashid, key, ""); }  },
                { typeof(MarketDataApi.Model.PacketTAIFEX.I010), () => { conndb.Set<MarketDataApi.Model.PacketTAIFEX.I010>(key, data as MarketDataApi.Model.PacketTAIFEX.I010); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTSE.Format6), () => { conndb.Set<MarketDataApi.Model.PacketTSE.Format6>(key, data as MarketDataApi.Model.PacketTSE.Format6); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTSE.Format17), () => { conndb.Set<MarketDataApi.Model.PacketTSE.Format17>(key, data as MarketDataApi.Model.PacketTSE.Format17); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTPEX.Format6), () => { conndb.Set<MarketDataApi.Model.PacketTPEX.Format6>(key, data as MarketDataApi.Model.PacketTPEX.Format6); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTPEX.Format17), () => { conndb.Set<MarketDataApi.Model.PacketTPEX.Format17>(key, data as MarketDataApi.Model.PacketTPEX.Format17); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTAIFEX.I020), () => { conndb.Set<MarketDataApi.Model.PacketTAIFEX.I020>(key, data as MarketDataApi.Model.PacketTAIFEX.I020); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTAIFEX.I080), () => { conndb.Set<MarketDataApi.Model.PacketTAIFEX.I080>(key, data as MarketDataApi.Model.PacketTAIFEX.I080); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTAIFEX.I022), () => { conndb.Set<MarketDataApi.Model.PacketTAIFEX.I022>(key, data as MarketDataApi.Model.PacketTAIFEX.I022); conndb.SetEntryInHash(hashid, key, ""); }},
                { typeof(MarketDataApi.Model.PacketTAIFEX.I082), () => { conndb.Set<MarketDataApi.Model.PacketTAIFEX.I082>(key, data as MarketDataApi.Model.PacketTAIFEX.I082); conndb.SetEntryInHash(hashid, key, ""); }},
            };
            switchTypeAction[data.GetType()]();

            //方法2
            //Type type = data.GetType();
            //System.Reflection.PropertyInfo[] propertyInfos = type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            //foreach (System.Reflection.PropertyInfo p in propertyInfos)
            //{
            //    string saveData = (p.GetValue(data) == null) ? "" : p.GetValue(data).ToString();
            //    _client.SetEntryInHash(hashid, key + "_" + p.Name, saveData);
            //}
        }
        /// <summary>
        /// 取RedisDB資料
        /// </summary>
        public static void GetRedisDB(RedisClient conndb, string hashid)
        {
            //方法1
            var switchTypeAction = new Dictionary<string, Action>
            {
                { Parameter.TSE_FORMAT1_HASH_KEY, () => { SymbolTseList.SetSymbolTseDataList(conndb.GetAll<MarketDataApi.Model.PacketTSE.Format1>(conndb.GetHashKeys(hashid))); } },
                { Parameter.TPEX_FORMAT1_HASH_KEY, () => { SymbolTpexList.SetSymbolTpexDataList(conndb.GetAll<MarketDataApi.Model.PacketTPEX.Format1>(conndb.GetHashKeys(hashid))); } },
                { Parameter.I010_HASH_KEY, () => { SymbolTaifexList.SetSymbolTaifexDataList(conndb.GetAll<MarketDataApi.Model.PacketTAIFEX.I010>(conndb.GetHashKeys(hashid))); }},
            };
            switchTypeAction[hashid]();
            //方法2-string to int error
            //Dictionary<string, string> hashData = _client.GetAllEntriesFromHash(hashid);//所有key/value
            //var keyList = _client.GetHashKeys(hashid).GroupBy(x => x.Split('_')[0]);
            //foreach (var obj in keyList.ToList())
            //{
            //    SymbolTse ret = new SymbolTse();
            //    System.Reflection.PropertyInfo[] propertyInfos = ret.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            //    foreach (System.Reflection.PropertyInfo p in propertyInfos)
            //    {
            //        p.SetValue(ret, _client.GetValueFromHash(hashid, (obj.Key + "_" + p.Name)));
            //    }
            //    SymbolTseList.AddSymbolTseData(ret);
            //    SymbolTseDictionary = SymbolTseList.AllSymbolTseList;
            //}
        }

        public static bool GetTSERedisDB(RedisClient conndb, string hashid)
        {
            if (conndb.GetHashKeys(hashid).Count == 0)
                return false;
            SymbolTseList.SetSymbolTseDataList(conndb.GetAll<MarketDataApi.Model.PacketTSE.Format1>(conndb.GetHashKeys(hashid)));
            return true;
        }
        public static bool GetTPEXRedisDB(RedisClient conndb, string hashid)
        {
            if (conndb.GetHashKeys(hashid).Count == 0)
                return false;
            SymbolTpexList.SetSymbolTpexDataList(conndb.GetAll<MarketDataApi.Model.PacketTPEX.Format1>(conndb.GetHashKeys(hashid)));
            return true;
        }
        public static bool GetTAIFEXRedisDB(RedisClient conndb, string hashid)
        {
            if (conndb.GetHashKeys(hashid).Count == 0)
                return false;
            SymbolTaifexList.SetSymbolTaifexDataList(conndb.GetAll<MarketDataApi.Model.PacketTAIFEX.I010>(conndb.GetHashKeys(hashid)));
            return true;
        }

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
                if(value.CompareTo(16384) >= 0)//(16384~1073741824)
                {
                    Byte[] byteArray = BitConverter.GetBytes(value);
                    BitArray bitAry = new BitArray(new byte[] { byteArray[0], byteArray[1], byteArray[2], byteArray[3] });
                    BitArray retBitAry = new BitArray(new byte[4]);
                    for (int i = 2; i < bitAry.Count; i++)
                    {
                        retBitAry[i] = bitAry[i - 2];
                    }
                    //標示4byte
                    retBitAry[0] = true;
                    retBitAry[1] = true;

                    retBitAry.CopyTo(byteArray, 0);
                    data[offset] = byteArray[0];
                    data[offset + 1] = byteArray[1];
                    data[offset + 2] = byteArray[2];
                    data[offset + 3] = byteArray[3];
                    return 4;
                }
                else//(64~16383)
                {
                    //EX：10000001.00000000 => 00100000.10100000(右移2位後。第1bit改1因為使用2byte)
                    Byte[] byteArray = BitConverter.GetBytes(value);
                    BitArray bitAry = new BitArray(new byte[] { byteArray[0], byteArray[1] });
                    BitArray retBitAry = new BitArray(new byte[2]);
                    for (int i = 2; i < bitAry.Count; i++)
                    {
                        retBitAry[i] = bitAry[i - 2];
                    }
                    //標示2byte
                    retBitAry[0] = true;
                    retBitAry[1] = false;

                    retBitAry.CopyTo(byteArray, 0);
                    data[offset] = byteArray[0];
                    data[offset + 1] = byteArray[1];
                    return 2;
                }
            }
            else//(0~63)
            {
                Byte[] byteArray = BitConverter.GetBytes(value);
                BitArray bitAry = new BitArray(new byte[] { byteArray[0]});
                BitArray retBitAry = new BitArray(new byte[1]);
                for (int i = 2; i < bitAry.Count; i++)
                {
                    retBitAry[i] = bitAry[i - 2];
                }
                //標示1byte
                retBitAry[0] = false;
                retBitAry[1] = false;

                retBitAry.CopyTo(byteArray, 0);
                data[offset] = byteArray[0];
                return 1;
            }
        }
        /// <summary>
        /// 帶動態byte取得int值
        /// </summary>
        public static int GetIntToDynamicBytes(byte[] data, int offset)
        {
            BitArray bitAry = new BitArray(new byte[] { data[offset] });
            if(bitAry[0] == false)//1 byte
            {
                BitArray retBitAry = new BitArray(new byte[] { data[offset + 0] });
                for (int i = 2; i < 8; i++)
                {
                    retBitAry[i - 2] = retBitAry[i];
                }
                retBitAry[6] = false;
                retBitAry[7] = false;
                byte[] ret = new byte[1];
                retBitAry.CopyTo(ret, 0);
                return ret[0];
            }
            else
            {
                if (bitAry[1] == false)//2 byte
                {
                    BitArray retBitAry = new BitArray(new byte[] { data[offset + 0], data[offset + 1] });

                    for(int i = 2; i < 16; i++)
                    {
                        retBitAry[i - 2] = retBitAry[i];
                    }
                    retBitAry[14] = false;
                    retBitAry[15] = false;
                    byte[] ret = new byte[2];
                    retBitAry.CopyTo(ret, 0);
                    return ret[0] | (ret[1] << 8);
                }
                else//4 byte
                {
                    BitArray retBitAry = new BitArray(new byte[] { data[offset + 0], data[offset + 1], data[offset + 2], data[offset + 3] });

                    for (int i = 2; i < 32; i++)
                    {
                        retBitAry[i - 2] = retBitAry[i];
                    }
                    retBitAry[30] = false;
                    retBitAry[31] = false;
                    byte[] ret = new byte[4];
                    retBitAry.CopyTo(ret, 0);
                    return ret[0] | (ret[1] << 8) | ret[2] << 16 | (ret[3] << 32);
                }
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
