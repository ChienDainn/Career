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
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            Session[CommonConstants.ADMIN_SESSION] = null;
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new TaiKhoanDao();
                var result = await dao.Login(model.UserName, model.PassWord, CommonConstants.QUANTRIVIEN);
                if(result > 0)
                {
                    var userLogin = await dao.GetAdminByEmail(model.UserName);
                    Session[CommonConstants.ADMIN_SESSION] = userLogin;
                    TempData["AlertMessage"] = "Đăng nhập thành công !";
                    TempData["AlertType"] = "alert-success";
                    return RedirectToAction("Index", "Home");
                }
                else if(result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản đã bị khóa");
                }else if(result == -1)
                {
                    ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                }
            }
            return View();
        }
    }
}