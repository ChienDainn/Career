using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Employer;
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Areas.NhaTuyenDung.Controllers
{
    public class HomeController : BaseController
    {
        private readonly NhaTuyenDungDao nhatuyendungDao;
        private readonly TinTuyenDungDao tintuyendungDao;

        public HomeController()
        {
            nhatuyendungDao = new NhaTuyenDungDao();
            tintuyendungDao = new TinTuyenDungDao();
        }

        // GET: NhaTuyenDung/Home
        public ActionResult Index()
        {
            var userLogin = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];
            if (userLogin == null) return RedirectToAction("Index", "Login");
            ViewBag.SlTinTuyenDung = tintuyendungDao.SlTinTuyenDung(userLogin.Id, true);
            ViewBag.SlTinChoDuyet = tintuyendungDao.SlTinTuyenDung(userLogin.Id, false);
            ViewBag.SlBaiViet = new BaiVietDao().SlBaiViet(userLogin.Id, true);
            ViewBag.SlUngVien = new UngTuyenDao().SlUngTuyen(userLogin.Id);
            ViewBag.TopView = new TinTuyenDungDao().GetListTopView(5, userLogin.Id);
            return View();
        }

        public ActionResult Logout()
        {
            SetAlert("Đăng xuất thành công", "success");
            Session[CommonConstants.EMPLOYER_SESSION] = null;
            return RedirectToAction("Index", "Login");
        }

        public async Task<ActionResult> Info()
        {
            var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];
            if (session == null) return RedirectToAction("Index");
            var member = await nhatuyendungDao.GetByIdClient(session.Id);
            ViewBag.AnhDaiDien = member.AnhDaiDien;
            ViewBag.AnhBia = member.AnhBia;
            return View(member);
        }

        [HttpPost]
        public async Task<ActionResult> Info(EmployerEditClient member)
        {
            var item = await nhatuyendungDao.GetByIdClient(member.MaNTD);
            ViewBag.AnhDaiDien = item.AnhDaiDien;
            ViewBag.AnhBia = item.AnhBia;
            if (ModelState.IsValid)
            {
                var result = await nhatuyendungDao.UpdateClient(member, Server);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        [ChildActionOnly]
        public ActionResult _SidebarMenu()
        {
            var userLogin = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];
            ViewBag.SlTinChoDuyet = tintuyendungDao.SlTinTuyenDung(userLogin.Id, false);
            ViewBag.BaiVietChuaDuyet = new BaiVietDao().SlBaiViet(userLogin.Id, false);
            return PartialView();
        }
    }
}