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
        // Phương thức Index của trang đăng nhập admin.
        public ActionResult Index()
        {
            // Xóa thông tin người dùng đăng nhập từ Session (nếu có) để đảm bảo đăng nhập mới.
            Session[CommonConstants.ADMIN_SESSION] = null;

            // Trả về view "Index" cho trang đăng nhập.
            return View();
        }

        // Phương thức Index (post method) xử lý việc đăng nhập người dùng admin.
        [HttpPost]
        public async Task<ActionResult> Index(LoginModel model)
        {
            // Kiểm tra xem dữ liệu được gửi từ form đăng nhập có hợp lệ (valid) không.
            if (ModelState.IsValid)
            {
                // Tạo đối tượng dao để thực hiện các thao tác liên quan đến tài khoản.
                var dao = new TaiKhoanDao();

                // Gọi phương thức Login của đối tượng dao để kiểm tra đăng nhập.
                var result = await dao.Login(model.UserName, model.PassWord, CommonConstants.QUANTRIVIEN);

                // Nếu kết quả đăng nhập > 0, tức là đăng nhập thành công.
                if (result > 0)
                {
                    // Lấy thông tin người dùng đăng nhập và lưu vào Session.
                    var userLogin = await dao.GetAdminByEmail(model.UserName);
                    Session[CommonConstants.ADMIN_SESSION] = userLogin;

                    // Đặt thông báo thành công và loại "alert-success" để hiển thị cho người dùng.
                    TempData["AlertMessage"] = "Đăng nhập thành công !";
                    TempData["AlertType"] = "alert-success";

                    // Chuyển hướng đến trang "Index" của controller "Home".
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    // Nếu kết quả đăng nhập là 0, tức là tài khoản đã bị khóa.
                    // Thêm lỗi vào ModelState để hiển thị cho người dùng.
                    ModelState.AddModelError("", "Tài khoản đã bị khóa");
                }
                else if (result == -1)
                {
                    // Nếu kết quả đăng nhập là -1, tức là sai tài khoản hoặc mật khẩu.
                    // Thêm lỗi vào ModelState để hiển thị cho người dùng.
                    ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                }
            }

            // Nếu có lỗi hoặc ModelState không hợp lệ, trả về view "Index" để người dùng có thể thử lại đăng nhập.
            return View();
        }

    }
}