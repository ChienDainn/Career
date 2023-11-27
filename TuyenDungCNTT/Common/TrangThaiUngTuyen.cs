using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuyenDungCNTT.Common
{
    public class TrangThaiUngTuyen
    {
        // Các trạng thái ứng tuyển
        public static string DALUU = "Đã lưu";                   // Ứng viên đã lưu hồ sơ ứng tuyển
        public static string CHUAXEM = "Chưa xem";               // Hồ sơ ứng tuyển chưa được xem
        public static string DAXEM = "Đã xem";                   // Hồ sơ ứng tuyển đã được xem
        public static string TUCHOI = "Từ chối";                // Hồ sơ ứng tuyển bị từ chối
        public static string CHAPNHAN = "Được nhận";             // Hồ sơ ứng tuyển được chấp nhận và nhận việc
        public static string TIEMNANG = "Ứng viên tiềm năng";    // Ứng viên được xem là tiềm năng cho việc làm
        public static string HENPHONGVAN = "Hẹn phỏng vấn";      // Hồ sơ được hẹn phỏng vấn
    }
}
