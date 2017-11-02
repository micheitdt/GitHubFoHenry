using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public partial class Utility
    {
        public static bool SaveData(string FileName, byte[] Data)
        {
            using (FileStream FS = new FileStream(FileName, File.Exists(FileName) ? FileMode.Append : FileMode.OpenOrCreate, FileAccess.Write))
            {
                FS.Write(Data, 0, Data.Length);
                FS.Close();
            }
            return true;
        }
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
