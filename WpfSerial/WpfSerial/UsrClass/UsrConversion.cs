using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsrMethod
{
    class UsrConversion
    {
        /// <summary>
        /// 字符串转Hex字符串,"1234"->"31 32 33 34"
        /// </summary>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static string String2Hex(string str1)
        {
            Byte[] data = Encoding.Default.GetBytes(str1);
            if (str1 != null)
            {
                return BitConverter.ToString(data).Replace("-", " ");
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Hex字符串转字符串,"31 32 33 34"->"1234"
        /// </summary>
        /// <param name="Hex"></param>
        /// <returns></returns>
        public static string Hex2String(string Hex)
        {

            if (Hex != null)
            {
                byte[] data = HexString2Byte(Hex);
                return Encoding.Default.GetString(data);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 16进制格式string 转byte[],"31 32 33 34"->{0x31,0x32,0x33,0x34}
        /// </summary>
        /// <param name="Hex"></param>
        /// <returns></returns>
        public static byte[] HexString2Byte(string Hex)
        {
            string str1 = Hex.Replace(" ", "");
            int length = 0;
            if (length % 2 == 0)
            {
                length = str1.Length / 2;
            }
            else
            {
                length = str1.Length / 2 + 1;
                str1 = str1.Insert(length - 1, "0");
                Console.WriteLine(str1);
            }
            byte[] data = new byte[length];
            for (int i = 0; i < length; i++)
            {
                data[i] = Convert.ToByte(str1.Substring(i * 2, 2), 16);
            }
            return data;
        }
        /// <summary>
        /// byte[]转16进制string,{0x31,0x32,0x33,0x34}->"31 32 33 34"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Byte2HexString(byte[] data)
        {
            if (data != null)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < data.Length; i++)

                {

                    sb.Append(data[i].ToString("X2")+" ");

                }
                string hexString = sb.ToString();
                return hexString;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// byte[]转string,{0x31,0x32,0x33,0x34}->"1234"
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Byte2String(byte[] data)
        {
            return System.Text.Encoding.Default.GetString(data);
        }

    }
}
