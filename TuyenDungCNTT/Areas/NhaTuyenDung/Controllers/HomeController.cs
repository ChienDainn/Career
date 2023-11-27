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
        // Khai báo trường (field) để lưu trữ thể hiện của lớp NhaTuyenDungDao.
        private readonly NhaTuyenDungDao nhatuyendungDao;

        // Khai báo trường (field) để lưu trữ thể hiện của lớp TinTuyenDungDao.
        private readonly TinTuyenDungDao tintuyendungDao;

        // Hàm khởi tạo của lớp HomeController.
        public HomeController()
        {
            // Tạo thể hiện của lớp NhaTuyenDungDao và gán cho trường nhatuyendungDao.
            nhatuyendungDao = new NhaTuyenDungDao();

            // Tạo thể hiện của lớp TinTuyenDungDao và gán cho trường tintuyendungDao.
            tintuyendungDao = new TinTuyenDungDao();
        }


        // GET: NhaTuyenDung/Home
        // Phương thức Index của Controller, được gọi khi người dùng truy cập trang chính của ứng dụng.

        public ActionResult Index()
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa bằng cách kiểm tra session.
            var userLogin = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

            // Nếu người dùng chưa đăng nhập (userLogin là null), chuyển hướng họ đến trang đăng nhập.
            if (userLogin == null)
                return RedirectToAction("Index", "Login");

            // Nếu người dùng đã đăng nhập:
            // - Lấy số lượng tin tuyển dụng đã được duyệt và chưa được duyệt và đặt vào ViewBag.
            ViewBag.SlTinTuyenDung = tintuyendungDao.SlTinTuyenDung(userLogin.Id, true);
            ViewBag.SlTinChoDuyet = tintuyendungDao.SlTinTuyenDung(userLogin.Id, false);

            // - Lấy số lượng bài viết đã được duyệt và đặt vào ViewBag.
            ViewBag.SlBaiViet = new BaiVietDao().SlBaiViet(userLogin.Id, true);

            // - Lấy số lượng ứng viên và đặt vào ViewBag.
            ViewBag.SlUngVien = new UngTuyenDao().SlUngTuyen(userLogin.Id);

            // - Lấy danh sách các tin tuyển dụng có lượt xem cao nhất (top views) và đặt vào ViewBag.
            ViewBag.TopView = new TinTuyenDungDao().GetListTopView(5, userLogin.Id);

            // Trả về ActionResult kiểu View để hiển thị trang chính của người dùng đã đăng nhập.
            return View();
        }

        // Phương thức Logout được gọi khi người dùng muốn đăng xuất khỏi ứng dụng.

        public ActionResult Logout()
        {
            // Đặt thông báo "Đăng xuất thành công" với kiểu thông báo "success" để hiển thị cho người dùng.
            SetAlert("Đăng xuất thành công", "success");

            // Xóa thông tin đăng nhập của người dùng bằng cách đặt session EMPLOYER_SESSION thành null.
            Session[CommonConstants.EMPLOYER_SESSION] = null;

            // Chuyển hướng người dùng đến trang đăng nhập (trang "Login").
            return RedirectToAction("Index", "Login");
        }


        // Phương thức Info được gọi khi người dùng muốn xem thông tin cá nhân của họ.

        public async Task<ActionResult> Info()
        {
            // Lấy thông tin đăng nhập của người dùng từ session.
            var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

            // Nếu người dùng chưa đăng nhập (session là null), chuyển hướng họ đến trang đăng nhập.
            if (session == null) return RedirectToAction("Index");

            // Lấy thông tin người dùng từ cơ sở dữ liệu dựa trên session.Id và đặt vào biến member.
            var member = await nhatuyendungDao.GetByIdClient(session.Id);

            // Đặt các thông tin ảnh đại diện và ảnh bìa vào ViewBag để sử dụng trong View.
            ViewBag.AnhDaiDien = member.AnhDaiDien;
            ViewBag.AnhBia = member.AnhBia;

            // Trả về ActionResult kiểu View với thông tin cá nhân của người dùng để hiển thị trong giao diện.
            return View(member);
        }


        [HttpPost]
        // Phương thức Info (sử dụng phiên bản với tham số EmployerEditClient) được gọi khi người dùng muốn cập nhật thông tin cá nhân của họ.

        public async Task<ActionResult> Info(EmployerEditClient member)
        {
            // Lấy thông tin người dùng từ cơ sở dữ liệu dựa trên MaNTD (mã người tuyển dụng) của đối tượng member.
            var item = await nhatuyendungDao.GetByIdClient(member.MaNTD);

            // Đặt các thông tin ảnh đại diện và ảnh bìa từ thông tin người dùng (item) vào ViewBag để sử dụng trong View.
            ViewBag.AnhDaiDien = item.AnhDaiDien;
            ViewBag.AnhBia = item.AnhBia;

            // Kiểm tra xem dữ liệu mà người dùng nhập vào form có hợp lệ hay không bằng cách sử dụng ModelState.IsValid.
            if (ModelState.IsValid)
            {
                // Nếu dữ liệu là hợp lệ:
                // - Gọi phương thức UpdateClient của thể hiện nhatuyendungDao để cập nhật thông tin người dùng.
                var result = await nhatuyendungDao.UpdateClient(member, Server);

                // Kiểm tra kết quả từ việc cập nhật thông tin người dùng.
                if (result)
                {
                    // Nếu cập nhật thành công, đặt thông báo "Cập nhật thành công" với kiểu thông báo "success".
                    SetAlert("Cập nhật thành công", "success");

                    // Chuyển hướng người dùng đến trang chính (trang "Index").
                    return RedirectToAction("Index");
                }
            }

            // Trả về ActionResult kiểu View để hiển thị lại form cập nhật với thông báo lỗi nếu có.
            return View();
        }


        // Phương thức _SidebarMenu là một phương thức con (child action) chỉ được gọi bởi các view hoặc layout. 
        // Nó được sử dụng để tạo các dữ liệu và thông tin hiển thị trong thanh sidebar của giao diện.

        [ChildActionOnly]
        public ActionResult _SidebarMenu()
        {
            // Lấy thông tin đăng nhập của người dùng từ session.
            var userLogin = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

            // Đặt số lượng tin tuyển dụng chưa được duyệt vào ViewBag để hiển thị trong thanh sidebar.
            ViewBag.SlTinChoDuyet = tintuyendungDao.SlTinTuyenDung(userLogin.Id, false);

            // Đặt số lượng bài viết chưa được duyệt vào ViewBag để hiển thị trong thanh sidebar.
            ViewBag.BaiVietChuaDuyet = new BaiVietDao().SlBaiViet(userLogin.Id, false);

            // Trả về một PartialView, mà thông thường là một phần của trang web sẽ được nạp bằng AJAX hoặc sử dụng Razor Partial.
            return PartialView();
        }

    }
}