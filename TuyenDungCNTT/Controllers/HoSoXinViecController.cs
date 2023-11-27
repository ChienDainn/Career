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
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (UserLogin() == null)
                return RedirectToAction("Index", "Home"); // Nếu chưa đăng nhập, chuyển hướng đến trang chủ

            // Lấy danh sách hồ sơ xin việc của người dùng
            var list = dao.GetListByIdNguoiDung(UserLogin().Id);
            return View(list); // Trả về view với danh sách hồ sơ xin việc
        }

        public ActionResult HoSo(int ungvien, int id)
        {
            var userLogin = UserLogin();
            var ntd = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

            // Kiểm tra xem người dùng đã đăng nhập hoặc là nhà tuyển dụng
            if (userLogin == null && ntd == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning"); // Hiển thị thông báo nếu chưa đăng nhập
                return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ
            }
            else if (userLogin != null && userLogin.Id != ungvien)
            {
                return RedirectToAction("Index", "HoSoXinViec"); // Chuyển hướng nếu không có quyền xem hồ sơ này
            }

            // Lấy thông tin hồ sơ xin việc theo id
            var model = dao.GetById(id, ungvien);

            // Nếu không tìm thấy hồ sơ, chuyển hướng đến trang danh sách hồ sơ
            if (model == null)
                return RedirectToAction("Index", "HoSoXinViec");

            return View(model); // Trả về view với thông tin hồ sơ xin việc
        }

        public ActionResult Create()
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning"); // Hiển thị thông báo nếu chưa đăng nhập
                return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ
            }
            return View(); // Trả về view để tạo hồ sơ xin việc
        }

        [HttpPost]
        public async Task<ActionResult> Create(HoSoXinViecCreate item)
        {
            if (ModelState.IsValid)
            {
                // Tạo hồ sơ xin việc
                var result = await dao.Create(item, UserLogin().Id);

                if (result > 0)
                {
                    SetAlert("Tạo hồ sơ xin việc thành công", "success"); // Hiển thị thông báo nếu tạo hồ sơ thành công
                    return RedirectToAction("Index"); // Chuyển hướng đến trang danh sách hồ sơ
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tên hồ sơ đã tồn tại"); // Thêm lỗi nếu tên hồ sơ đã tồn tại
                }
                else
                {
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error"); // Hiển thị thông báo nếu có lỗi
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            // Kiểm tra xem người dùng đã đăng nhập chưa
            if (UserLogin() == null)
            {
                SetAlert("Bạn chưa đăng nhập", "warning"); // Hiển thị thông báo nếu chưa đăng nhập
                return RedirectToAction("Index", "Home"); // Chuyển hướng đến trang chủ
            }

            // Lấy thông tin hồ sơ xin việc để chỉnh sửa
            var model = dao.GetByIdEdit(id, UserLogin().Id);

            // Nếu không tìm thấy hồ sơ, hiển thị thông báo và chuyển hướng đến trang danh sách hồ sơ
            if (model == null)
            {
                SetAlert("Đã có lỗi xảy ra", "warning");
                return RedirectToAction("Index", "Home");
            }

            return View(model); // Trả về view để chỉnh sửa hồ sơ xin việc
        }

        [HttpPost]
        public async Task<ActionResult> Edit(HoSoXinViecEdit item)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật hồ sơ xin việc
                var result = await dao.Update(item, UserLogin().Id);

                if (result > 0)
                {
                    SetAlert("Cập nhật hồ sơ xin việc thành công", "success"); // Hiển thị thông báo nếu cập nhật thành công
                    return RedirectToAction("Index", "HoSoXinViec"); // Chuyển hướng đến trang danh sách hồ sơ
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tên hồ sơ đã tồn tại"); // Thêm lỗi nếu tên hồ sơ đã tồn tại
                }
                else
                {
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error"); // Hiển thị thông báo nếu có lỗi
                }
            }
            return View();
        }
    }
}
