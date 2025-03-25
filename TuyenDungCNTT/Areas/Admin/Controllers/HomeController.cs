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
        public ActionResult Index()
        {
            ViewBag.SlUngVien = new TaiKhoanDao().SlUngVien();
            ViewBag.SlNhaTuyenDung = new TaiKhoanDao().SlNhaTuyenDung();
            ViewBag.SlTinTuyenDung = new TinTuyenDungDao().SlTinTuyenDung(null, true);
            ViewBag.SlBaiViet = new BaiVietDao().SlBaiViet(null, true);
            ViewBag.TopView = new TinTuyenDungDao().GetListTopView(5, null);
            return View();
        }

        public ActionResult Logout()
        {
            SetAlert("Đăng xuất thành công", "success");
            Session[CommonConstants.ADMIN_SESSION] = null;
            return RedirectToAction("Index","Login");
        }

        [ChildActionOnly]
        public ActionResult _SidebarMenu()
        {
            return PartialView();
        }
    }
}