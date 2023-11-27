using System.Web.Mvc;

namespace TuyenDungCNTT.Areas.NhaTuyenDung
{
    // Định nghĩa lớp NhaTuyenDungAreaRegistration kế thừa từ AreaRegistration
    public class NhaTuyenDungAreaRegistration : AreaRegistration
    {
        // Thuộc tính AreaName định nghĩa tên của khu vực này
        public override string AreaName
        {
            get
            {
                return "NhaTuyenDung";
            }
        }

        // Phương thức RegisterArea đăng ký tuyến đường cho khu vực này
        public override void RegisterArea(AreaRegistrationContext context)
        {
            // Đăng ký một tuyến đường cho khu vực "NhaTuyenDung"
            context.MapRoute(
                "NhaTuyenDung_default", // Tên của tuyến đường
                "nha-tuyen-dung/{controller}/{action}/{id}", // Mẫu tuyến đường
                new { controller = "Login", action = "Index", id = UrlParameter.Optional } // Giá trị mặc định
            );
        }
    }
}
