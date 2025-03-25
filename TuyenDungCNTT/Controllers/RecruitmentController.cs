using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;

namespace TuyenDungCNTT.Controllers
{

    public class RecruitmentController : BaseController
    {
        private readonly TinTuyenDungDao tinTuyenDungDao;
        private readonly UngTuyenDao ungTuyenDao;

        public RecruitmentController()
        {
            tinTuyenDungDao = new TinTuyenDungDao();
            ungTuyenDao = new UngTuyenDao();
        }

        // GET: Recruitment
        public async Task<ActionResult> Index(int id)
        {
            var model = await tinTuyenDungDao.GetViewById(id);
            tinTuyenDungDao.UpdateCount(id);
            if(UserLogin() != null)
            {
                ViewBag.Status = ungTuyenDao.GetStatus(UserLogin().Id, id);
                ViewBag.ListCV = new HoSoXinViecDao().GetListByIdNguoiDung(UserLogin().Id);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UngTuyen(string HoSo, HttpPostedFileBase FileCV, int MaTTD)
        {
            var result = -1;

            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }

            if (HoSo != null)
            {
                result = ungTuyenDao.UngTuyenOnline(UserLogin().Id, MaTTD, HoSo);
            }
            if(FileCV != null)
            {
                result = ungTuyenDao.UngTuyenFile(UserLogin().Id, MaTTD, FileCV, Server);
            }

            if(result > 0)
            {
                SetAlert("Ứng tuyển thành công", "success");
            }else if(result == 0)
            {
                SetAlert("Bạn đã ứng tuyển tin tuyển dụng này", "warning");
            }
            else
            {
                SetAlert("Đã có lỗi xảy ra", "warning");
            }
            return RedirectToAction("Index", "Recruitment", new { id = MaTTD});
        }

        [HttpPost]
        public ActionResult Save(int MaTTD)
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }
            var result = ungTuyenDao.Save(UserLogin().Id, MaTTD);
            if(result > 0)
            {
                SetAlert("Lưu thành công", "success");
            }else if(result == 0)
            {
                SetAlert("Bỏ lưu thành công", "success");
            }
            else
            {
                SetAlert("Bạn đã ứng tuyển tin tuyển dụng này", "warning");
            }
            return RedirectToAction("Index", "Recruitment", new { id = MaTTD });
        }

        public ActionResult DaLuu()
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }
            var model = tinTuyenDungDao.GetListSaveByIdUser(UserLogin().Id, TrangThaiUngTuyen.DALUU);
            return View(model);
        }

        public ActionResult PhuHop()
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult GetPaging(int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                keyWord = null,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = new TinTuyenDungDao().GetListByIdUser(request, UserLogin().Id);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet);
        }
    }
}