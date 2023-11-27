



using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace TuyenDungCNTT.Common
{
    public class Encryptor
    {
        /// <summary>
        /// Tính toán giá trị băm (hash) MD5 của chuỗi đầu vào.
        /// </summary>
        /// <param name="text">Chuỗi đầu vào cần mã hóa.</param>
        /// <returns>Chuỗi băm MD5 dưới dạng chuỗi hexa.</returns>
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider(); // Tạo một đối tượng MD5 để thực hiện mã hóa MD5.

            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text)); // Tính toán giá trị băm của chuỗi đầu vào.

            byte[] result = md5.Hash; // Lưu trữ kết quả băm trong một mảng byte.

            StringBuilder strBuilder = new StringBuilder(); // Tạo một StringBuilder để xây dựng chuỗi băm.

            // Duyệt qua mảng byte và chuyển mỗi byte thành chuỗi hexa hai ký tự, sau đó thêm vào chuỗi băm.
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString(); // Trả về chuỗi băm MD5 dưới dạng chuỗi hexa.
        }
    }
}
