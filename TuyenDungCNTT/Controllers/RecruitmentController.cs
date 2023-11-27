using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
//quản lý hiển thị thông tin về tuyển dụng, ứng tuyển, lưu tin tuyển dụng quản lý danh sách đã lưu.
namespace TuyenDungCNTT.Controllers
{
    public class RecruitmentController : BaseController
    {
        private readonly TinTuyenDungDao tinTuyenDungDao; // Đối tượng Dao để tương tác với bảng TinTuyenDung
        private readonly UngTuyenDao ungTuyenDao; // Đối tượng Dao để tương tác với bảng UngTuyen

        public RecruitmentController()
        {
            // Khởi tạo đối tượng Dao để tương tác với cơ sở dữ liệu
            tinTuyenDungDao = new TinTuyenDungDao(); // Tạo đối tượng cho bảng TinTuyenDung
            ungTuyenDao = new UngTuyenDao(); // Tạo đối tượng cho bảng UngTuyen
        }

        // GET: Recruitment
        public async Task<ActionResult> Index(int id)
        {
            // Lấy thông tin tin tuyển dụng theo id
            var model = await tinTuyenDungDao.GetViewById(id); // Gọi phương thức để lấy thông tin tuyển dụng dựa trên id

            // Cập nhật số lần xem tin tuyển dụng
            tinTuyenDungDao.UpdateCount(id); // Gọi phương thức để cập nhật số lần xem tin tuyển dụng

            if (UserLogin() != null)
            {
                // Nếu người dùng đã đăng nhập, lấy thông tin trạng thái ứng tuyển và danh sách hồ sơ xin việc
                ViewBag.Status = ungTuyenDao.GetStatus(UserLogin().Id, id); // Gọi phương thức để lấy thông tin trạng thái ứng tuyển
                ViewBag.ListCV = new HoSoXinViecDao().GetListByIdNguoiDung(UserLogin().Id); // Gọi phương thức để lấy danh sách hồ sơ xin việc của người dùng
            }
            return View(model); // Trả về trang giao diện với dữ liệu tuyển dụng
        }

        [HttpPost]
        public ActionResult UngTuyen(string HoSo, HttpPostedFileBase FileCV, int MaTTD)
        {
            var result = -1; // Kết quả mặc định là -1

            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning"); // Hiển thị thông báo "Bạn chưa đăng nhập" với loại "warning"
                return RedirectToAction("Index", "Home"); // Chuyển hướng người dùng đến trang chủ
            }

            if (HoSo != null)
            {
                result = ungTuyenDao.UngTuyenOnline(UserLogin().Id, MaTTD, HoSo); // Gọi phương thức để ứng tuyển trực tuyến với hồ sơ
            }
            if (FileCV != null)
            {
                result = ungTuyenDao.UngTuyenFile(UserLogin().Id, MaTTD, FileCV, Server); // Gọi phương thức để ứng tuyển với tệp CV
            }

            if (result > 0)
            {
                SetAlert("Ứng tuyển thành công", "success"); // Hiển thị thông báo "Ứng tuyển thành công" với loại "success"
            }
            else if (result == 0)
            {
                SetAlert("Bạn đã ứng tuyển tin tuyển dụng này", "warning"); // Hiển thị thông báo "Bạn đã ứng tuyển tin tuyển dụng này" với loại "warning"
            }
            else
            {
                SetAlert("Đã có lỗi xảy ra", "warning"); // Hiển thị thông báo "Đã có lỗi xảy ra" với loại "warning"
            }
            return RedirectToAction("Index", "Recruitment", new { id = MaTTD }); // Chuyển hướng người dùng trở lại trang tin tuyển dụng
        }

        [HttpPost]
        public ActionResult Save(int MaTTD)
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning"); // Hiển thị thông báo "Bạn chưa đăng nhập" với loại "warning"
                return RedirectToAction("Index", "Home"); // Chuyển hướng người dùng đến trang chủ
            }
            var result = ungTuyenDao.Save(UserLogin().Id, MaTTD); // Gọi phương thức để lưu hoặc bỏ lưu tin tuyển dụng
            if (result > 0)
            {
                SetAlert("Lưu thành công", "success"); // Hiển thị thông báo "Lưu thành công" với loại "success"
            }
            else if (result == 0)
            {
                SetAlert("Bỏ lưu thành công", "success"); // Hiển thị thông báo "Bỏ lưu thành công" với loại "success"
            }
            else
            {
                SetAlert("Bạn đã ứng tuyển tin tuyển dụng này", "warning"); // Hiển thị thông báo "Bạn đã ứng tuyển tin tuyển dụng này" với loại "warning"
            }
            return RedirectToAction("Index", "Recruitment", new { id = MaTTD }); // Chuyển hướng người dùng trở lại trang tin tuyển dụng
        }

        public ActionResult DaLuu()
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning"); // Hiển thị thông báo "Bạn chưa đăng nhập" với loại "warning"
                return RedirectToAction("Index", "Home"); // Chuyển hướng người dùng đến trang chủ
            }
            var model = tinTuyenDungDao.GetListSaveByIdUser(UserLogin().Id, TrangThaiUngTuyen.DALUU); // Gọi phương thức để lấy danh sách các tin tuyển dụng đã lưu của người dùng
            return View(model); // Trả về view với dữ liệu đã lấy được
        }

        public ActionResult PhuHop()
        {
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning"); // Hiển thị thông báo "Bạn chưa đăng nhập" với loại "warning"
                return RedirectToAction("Index", "Home"); // Chuyển hướng người dùng đến trang chủ
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
            var data = new TinTuyenDungDao().GetListByIdUser(request, UserLogin().Id); // Gọi phương thức để lấy danh sách tin tuyển dụng dựa trên yêu cầu phân trang
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet); // Trả về dữ liệu phân trang dưới dạng JSON
        }
    }
}
