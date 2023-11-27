using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TuyenDungCNTT.Common
{
    public static class StringHelper
    {
        /// <summary>
        /// Chuyển đổi một chuỗi thành chuỗi không dấu, phục vụ cho việc tạo URL thân thiện.
        /// </summary>
        /// <param name="input">Chuỗi đầu vào.</param>
        /// <returns>Chuỗi không dấu và phù hợp cho việc tạo URL.</returns>
        public static string ToUnsignString(string input)
        {
            // Loại bỏ khoảng trắng thừa ở đầu và cuối chuỗi
            input = input.Trim();

            // Loại bỏ các ký tự không in ra từ 0x20 đến 0x2F
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }

            // Thay thế các ký tự đặc biệt bằng dấu gạch ngang
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");

            // Loại bỏ các dấu thanh đọc đồng âm (diacritics) và thay thế 'đ' bằng 'd', 'Đ' bằng 'D'
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');

            // Loại bỏ các dấu hỏi '?' trong chuỗi
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }

            // Loại bỏ các dấu nối kép dư thừa và chuyển thành chữ thường
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }

            return str2;
        }
    }
}
