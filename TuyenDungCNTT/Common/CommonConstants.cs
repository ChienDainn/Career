using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuyenDungCNTT.Common
{
    // Định nghĩa một lớp tĩnh (static class) có tên CommonConstants
    public class CommonConstants
    {
        // Các hằng số cho loại người dùng
        public static int QUANTRIVIEN = 1;    // Loại người dùng là Quản trị viên
        public static int NHATUYENDUNG = 2;   // Loại người dùng là Nhà tuyển dụng
        public static int UNGVIEN = 3;        // Loại người dùng là Ứng viên

        // Các tên cho phiên làm việc của người dùng
        public static string USER_SESSION = "USER_SESSION";         // Phiên làm việc của người dùng
        public static string EMPLOYER_SESSION = "EMPLOYER_SESSION"; // Phiên làm việc của Nhà tuyển dụng
        public static string ADMIN_SESSION = "ADMIN_SESSION";       // Phiên làm việc của Quản trị viên
    }
}
