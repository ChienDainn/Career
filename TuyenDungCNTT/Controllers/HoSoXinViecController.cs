using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.HoSoXinViec;
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Controllers
{
    public class HoSoXinViecController : BaseController
    {
        private readonly HoSoXinViecDao dao;
        public HoSoXinViecController()
        {
            dao = new HoSoXinViecDao();
        }

        // GET: HoSoXinViec
        public ActionResult Index()
        {
            if (UserLogin() == null) return RedirectToAction("Index", "Home");
            var list = dao.GetListByIdNguoiDung(UserLogin().Id);
            return View(list);
        }

        public ActionResult HoSo(int ungvien, int id)
        {
            var userLogin = UserLogin();
            var ntd = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];
            if (userLogin == null && ntd == null) 
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }else if(userLogin != null && userLogin.Id != ungvien)
            {
                return RedirectToAction("Index", "HoSoXinViec");
            }
            var model = dao.GetById(id, ungvien);
            if(model == null) return RedirectToAction("Index", "HoSoXinViec");
            return View(model);
        }

        public ActionResult Create()
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(HoSoXinViecCreate item)
        {
            if (ModelState.IsValid)
            {
                var result = await dao.Create(item, UserLogin().Id);
                if (result > 0)
                {
                    SetAlert("Tạo hồ sơ xin việc thành công", "success");
                    return RedirectToAction("Index");
                }else if(result == 0)
                {
                    ModelState.AddModelError("", "Tên hồ sơ đã tồn tại");
                }
                else
                {
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }
            var model = dao.GetByIdEdit(id, UserLogin().Id);
            if(model == null)
            {
                SetAlert("Đã có lỗi xảy ra", "warning");
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(HoSoXinViecEdit item)
        {
            if (ModelState.IsValid)
            {
                var result = await dao.Update(item, UserLogin().Id);
                if (result > 0)
                {
                    SetAlert("Cập nhật hồ sơ xin việc thành công", "success");
                    return RedirectToAction("Index", "HoSoXinViec");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tên hồ sơ đã tồn tại");
                }
                else
                {
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }
            return View();
        }
    }
}