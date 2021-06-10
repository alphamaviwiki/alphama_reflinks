using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphamaConverter
{
    public static class StringHelper
    {
        // It should be noted that this method is expecting UTF-8 input only,
        // so you probably should give it a more fitting name.
        public static byte[] ToUTF8ByteArray(this string str)
        {
            Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

       

        public static string ConvertUTF8ToWin1252(string source)
        {
            Encoding utf8 = new UTF8Encoding();
            Encoding win1252 = Encoding.GetEncoding(1252);

            byte[] input = source.ToUTF8ByteArray();  // Note the use of my extension method
            byte[] output = Encoding.Convert(utf8, win1252, input);

            return win1252.GetString(output);
        }

        public static string ConvertWin1252ToUTF8(string source)
        {

            Encoding wind1252 = Encoding.GetEncoding(1252);
            Encoding utf8 = Encoding.UTF8;  
            byte[] wind1252Bytes = wind1252.GetBytes(source);
            byte[] utf8Bytes = Encoding.Convert(wind1252, utf8, wind1252Bytes);
            string utf8String = Encoding.UTF8.GetString(utf8Bytes);
            return utf8String;
        }
    }

   
}
