using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Areas.Admin.Models;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        // Phương thức Index của trang admin.
        public ActionResult Index()
        {
            // Lấy và gán các thông tin cơ bản vào ViewBag để sử dụng trong view.
            ViewBag.SlUngVien = new TaiKhoanDao().SlUngVien();
            ViewBag.SlNhaTuyenDung = new TaiKhoanDao().SlNhaTuyenDung();
            ViewBag.SlTinTuyenDung = new TinTuyenDungDao().SlTinTuyenDung(null, true);
            ViewBag.SlBaiViet = new BaiVietDao().SlBaiViet(null, true);
            ViewBag.TopView = new TinTuyenDungDao().GetListTopView(5, null);

            // Trả về view "Index".
            return View();
        }

        // Phương thức Logout dùng để đăng xuất người dùng admin.
        public ActionResult Logout()
        {
            // Đặt thông báo thành công với loại "success".
            SetAlert("Đăng xuất thành công", "success");

            // Xóa thông tin người dùng đăng nhập từ Session.
            Session[CommonConstants.ADMIN_SESSION] = null;

            // Chuyển hướng đến trang "Index" của controller "Login".
            return RedirectToAction("Index", "Login");
        }

        // Phương thức _SidebarMenu là một phương thức con (Child Action) chỉ được sử dụng để render một phần của trang.
        [ChildActionOnly]
        public ActionResult _SidebarMenu()
        {
            // Trả về một PartialView cho phần sidebar menu.
            return PartialView();
        }

    }
}