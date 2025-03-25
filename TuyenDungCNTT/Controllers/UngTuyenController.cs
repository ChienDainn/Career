using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;

namespace TuyenDungCNTT.Controllers
{
    public class UngTuyenController : BaseController
    {
        private readonly UngTuyenDao ungTuyenDao;

        public UngTuyenController()
        {
            ungTuyenDao = new UngTuyenDao();
        }

        // GET: UngTuyen
        public ActionResult Index()
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }
            var model = ungTuyenDao.GetListByUser(UserLogin().Id);
            return View(model);
        }

    }
}