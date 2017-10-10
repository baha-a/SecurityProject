using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Code
{
    public static class helper
    {
        public static bool _DEBUG_ = true;

        public static string makeThisStringXLetterLength(this string str, int leng)
        {
            while (str.Length < leng)
                str += "z";
            return str;
        }
        public static string BytesToString(this byte[] x)
        {
            return Encoding.UTF8.GetString(x);
        }
        public static byte[] StringToBytes(this string x)
        {
            return Encoding.UTF8.GetBytes(x);
        }
        public static string DeletZerosFromEnd(this string x)
        {
            return x.TrimEnd('\0');
        }
        public static string SubStringFromTo(this string x, int startIndex, int endIndex)
        {
            return x.Substring(startIndex, endIndex - startIndex);
        }
        public static string ToSinglLine(this string[] x)
        {
            string z = "";
            foreach (string y in x)
                z += y + "\r\n";
            return z;
        }


        public static byte[] Cute(this byte[] bytes, int startIndex, int count)
        {
            byte[] bytes2 = new byte[count];

            for (int i = 0 ; i < bytes2.Length ; i++)
                bytes2[i] = bytes[i + startIndex];

            return bytes2;
        }

        public static byte[] Marge(this byte[] x, byte[] y)
        {
            byte[] z = new byte[x.Length + y.Length];

            int i = 0;
            for ( ; i < x.Length ; i++)
                z[i] = x[i];
            for (int j = 0 ; j < y.Length ; j++, i++)
                z[i] = y[j];

            return z;
        }
        public static void Unmarge(this byte[] z, out byte[] x, out byte[] y, int startIndex = 128)
        {
            var xt = new byte[startIndex];
            var yt = new byte[z.Length - startIndex];

            int i = 0;
            for ( ; i < startIndex ; i++)
                xt[i] = z[i];

            for (int j = 0 ; j < yt.Length ; j++)
                yt[j] = z[i++];

            x = xt;
            y = yt;
        }


        public static string GenerateRandomString(int length = 32)
        {
            string res ="";
            Random r = new Random();
            for (int i = 0 ; i < length ; i++)
                res += (char)r.Next(40, 127);
            return res;
        }
    }
}