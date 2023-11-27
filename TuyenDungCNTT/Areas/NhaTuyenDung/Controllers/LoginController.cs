using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;

namespace TuyenDungCNTT.Areas.NhaTuyenDung.Controllers
{
    public class LoginController : Controller
    {
        private readonly TaiKhoanDao taiKhoanDao;

        public LoginController()
        {
            taiKhoanDao = new TaiKhoanDao();
        }

        // GET: Login
        // Phương thức Index được gọi khi người dùng truy cập đường dẫn trang đăng nhập (Login).

        public ActionResult Index()
        {
            // Đặt session EMPLOYER_SESSION thành null để đảm bảo người dùng không đăng nhập.
            Session[CommonConstants.EMPLOYER_SESSION] = null;

            // Trả về một ActionResult kiểu View để hiển thị trang đăng nhập.
            return View();
        }


        // Phương thức Index (sử dụng phiên bản với tham số LoginModel) được gọi khi người dùng gửi yêu cầu đăng nhập thông qua form.

        [HttpPost]
        public async Task<ActionResult> Index(Models.ViewModels.Employer.LoginModel model)
        {
            // Kiểm tra xem dữ liệu mà người dùng nhập vào form có hợp lệ hay không bằng cách sử dụng ModelState.IsValid.
            if (ModelState.IsValid)
            {
                // Gọi phương thức Login của thể hiện taiKhoanDao để kiểm tra đăng nhập.
                var result = await taiKhoanDao.Login(model.Email, model.Password, CommonConstants.NHATUYENDUNG);

                // Kiểm tra kết quả từ quá trình đăng nhập.
                if (result > 0)
                {
                    // Nếu đăng nhập thành công:
                    // - Lấy thông tin người dùng dựa trên địa chỉ email.
                    var userLogin = await taiKhoanDao.GetByEmail_NhaTuyenDung(model.Email);

                    // - Đặt thông tin người dùng vào session EMPLOYER_SESSION để đánh dấu người dùng đã đăng nhập.
                    Session[CommonConstants.EMPLOYER_SESSION] = userLogin;

                    // - Đặt thông báo "Đăng nhập thành công" vào TempData với kiểu thông báo "alert-success".
                    TempData["Notify"] = "Đăng nhập thành công!";
                    TempData["AlertType"] = "alert-success";

                    // - Chuyển hướng người dùng đến trang chính (trang "Index" của Home Controller).
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    // Nếu tài khoản bị khóa, thêm thông báo lỗi vào ModelState.
                    ModelState.AddModelError("", "Tài khoản đã bị khóa");
                }
                else if (result == -1)
                {
                    // Nếu tài khoản hoặc mật khẩu không đúng, thêm thông báo lỗi vào ModelState.
                    ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                }
            }

            // Trả về ActionResult kiểu View để hiển thị lại trang đăng nhập với thông báo lỗi nếu có.
            return View();
        }


        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // Phương thức Register được gọi khi người dùng gửi yêu cầu đăng ký tài khoản thông qua form.

        [HttpPost]
        public async Task<ActionResult> Register(Models.ViewModels.Employer.RegisterModel model)
        {
            // Kiểm tra xem dữ liệu mà người dùng nhập vào form có hợp lệ hay không bằng cách sử dụng ModelState.IsValid.
            if (ModelState.IsValid)
            {
                // Gọi phương thức Register_NhaTuyenDung của thể hiện taiKhoanDao để đăng ký tài khoản nhà tuyển dụng.
                var result = await taiKhoanDao.Register_NhaTuyenDung(model);

                // Kiểm tra kết quả từ quá trình đăng ký.
                if (result > 0)
                {
                    // Nếu đăng ký thành công (kết quả > 0), đặt thông báo "Đăng ký thành công!" vào TempData.
                    TempData["notify"] = "Đăng ký thành công !";

                    // Chuyển hướng người dùng đến trang đăng nhập (trang "Index" của Login Controller).
                    return RedirectToAction("Index");
                }
                else if (result == 0)
                {
                    // Nếu địa chỉ email đã tồn tại (kết quả == 0), thêm thông báo lỗi vào ModelState.
                    ModelState.AddModelError("", "Email này đã tồn tại");
                }
                else if (result == -1)
                {
                    // Nếu có lỗi xảy ra trong quá trình đăng ký (kết quả == -1), thêm thông báo lỗi vào ModelState.
                    ModelState.AddModelError("", "Đã có lỗi xảy ra. Vui lòng thử lại");
                }
            }

            // Trả về ActionResult kiểu View để hiển thị lại trang đăng ký với thông báo lỗi nếu có.
            return View();
        }

    }
}