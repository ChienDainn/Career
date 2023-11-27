using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
//quản lý việc người dùng ứng tuyển công việc trên trang web và hiển thị thông tin liên quan.
namespace TuyenDungCNTT.Controllers
{
    public class UngTuyenController : BaseController
    {
        private readonly UngTuyenDao ungTuyenDao;

        public UngTuyenController()
        {
            // Khởi tạo đối tượng dao để tương tác với cơ sở dữ liệu
            ungTuyenDao = new UngTuyenDao();
        }

        // GET: UngTuyen
        public ActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (UserLogin() == null)
            {
                // Nếu chưa đăng nhập, hiển thị thông báo và chuyển hướng đến trang chủ
                SetAlert("Bạn chưa đăng nhập", "warning");
                return RedirectToAction("Index", "Home");
            }

            // Lấy danh sách các công việc mà người dùng đã ứng tuyển
            var model = ungTuyenDao.GetListByUser(UserLogin().Id);

            // Trả về view hiển thị danh sách các công việc đã ứng tuyển
            return View(model);
        }
    }
}
