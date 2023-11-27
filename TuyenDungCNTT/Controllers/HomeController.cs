using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.TinTuyenDung;
using TuyenDungCNTT.Models.ViewModels.UngVien;
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Controllers
{
    public class HomeController : BaseController
    {
        private readonly TaiKhoanDao taiKhoanDao; // Khai báo một biến readonly để lưu trữ một đối tượng của lớp TaiKhoanDao.
        private readonly UngVienDao ungVienDao;   // Khai báo một biến readonly để lưu trữ một đối tượng của lớp UngVienDao.
        private readonly TinTuyenDungDao tinTuyenDungDao; // Khai báo một biến readonly để lưu trữ một đối tượng của lớp TinTuyenDungDao.

        public HomeController()
        {
            taiKhoanDao = new TaiKhoanDao();         // Khởi tạo đối tượng taiKhoanDao bằng cách tạo một thể hiện mới của lớp TaiKhoanDao.
            ungVienDao = new UngVienDao();           // Khởi tạo đối tượng ungVienDao bằng cách tạo một thể hiện mới của lớp UngVienDao.
            tinTuyenDungDao = new TinTuyenDungDao(); // Khởi tạo đối tượng tinTuyenDungDao bằng cách tạo một thể hiện mới của lớp TinTuyenDungDao.
        }

        public ActionResult Index()
        {
            var model = new TinTuyenDungDao().GetListItemHot(9); // Tạo một đối tượng TinTuyenDungDao mới và gọi phương thức GetListItemHot với đối số 9 để lấy danh sách các mục "hot".
            return View(model); // Trả về một ActionResult có tên Index và truyền model (danh sách các mục "hot") đến View để hiển thị trên trang web.
        }


        [HttpGet] //  GET request.
        public ActionResult Search()
        {
            return View(); 
        }

        public JsonResult GetPaging(string KeyWord, int pageIndex, string CapBac, string DiaChi, string ChuyenNganh, string LoaiCV, string MucLuong)
        {
            // Tạo một đối tượng TinTuyenDungSearch để lưu các thông tin tìm kiếm.
            var request = new TinTuyenDungSearch()
            {
                CapBac = CapBac,
                DiaChi = DiaChi,
                ChuyenNganh = ChuyenNganh,
                LoaiCV = LoaiCV,
                MucLuong = MucLuong
            };

            // Tạo đối tượng GetListPaging để lưu các thông tin phân trang.
            var paging = new GetListPaging()
            {
                keyWord = KeyWord.ToLower(),
                PageIndex = pageIndex,
                PageSize = 5
            };

            // Gọi phương thức GetListSearch từ đối tượng tinTuyenDungDao để lấy dữ liệu theo yêu cầu tìm kiếm và phân trang.
            var data = tinTuyenDungDao.GetListSearch(paging, request);

            int totalRecord = data.TotalRecord; // Lấy tổng số bản ghi tìm thấy.
            int toalPage = (int)Math.Ceiling((double)totalRecord / paging.PageSize); // Tính số trang dựa trên tổng số bản ghi và kích thước trang.

            // Trả về một đối tượng JsonResult chứa dữ liệu phân trang và kết quả tìm kiếm dưới dạng JSON.
            return Json(new
            {
                data = data.Items, // Danh sách các bản ghi kết quả tìm kiếm.
                pageCurrent = pageIndex, // Trang hiện tại.
                toalPage = toalPage, // Tổng số trang.
                totalRecord = totalRecord // Tổng số bản ghi.
            }, JsonRequestBehavior.AllowGet); // JsonRequestBehavior.AllowGet cho phép HTTP GET request truy cập vào dữ liệu JSON này.
        }


        [ChildActionOnly]
        public ActionResult _ViewSearch()
        {
            // Tạo một đối tượng TinTuyenDungSearch để lưu trữ thông tin tìm kiếm.
            var search = new TinTuyenDungSearch()
            {
                CapBac = Request["CapBac"], // Lấy giá trị từ tham số "CapBac" của HTTP Request và gán cho thuộc tính "CapBac" của đối tượng TinTuyenDungSearch.
                DiaChi = Request["DiaChi"] // Lấy giá trị từ tham số "DiaChi" của HTTP Request và gán cho thuộc tính "DiaChi" của đối tượng TinTuyenDungSearch.
            };

            return PartialView(search); // Trả về một PartialView và truyền đối tượng TinTuyenDungSearch cho nó để hiển thị trên trang web. PartialView thường là một phần của trang chứ không phải là trang hoàn chỉnh.
        }


        [HttpPost]
        public async Task<ActionResult> Login(LoginModel model)
        {
            string data = ""; // Khai báo một biến string để lưu trữ dữ liệu.

            if (ModelState.IsValid) // Kiểm tra xem ModelState có hợp lệ không, tức là dữ liệu mà người dùng đã nhập qua form có lỗi không.
            {
                // Gọi phương thức Login từ đối tượng taiKhoanDao để kiểm tra đăng nhập của người dùng.
                var result = await taiKhoanDao.Login(model.login_email, model.login_password, CommonConstants.UNGVIEN);

                if (result > 0) // Nếu đăng nhập thành công (result > 0), thực hiện các thao tác sau đây.
                {
                    // Gọi phương thức GetByEmail_UngVien từ đối tượng taiKhoanDao để lấy thông tin người dùng dựa trên email đăng nhập.
                    var userLogin = await taiKhoanDao.GetByEmail_UngVien(model.login_email);

                    // Lưu thông tin người dùng vào Session với key là CommonConstants.USER_SESSION.
                    Session[CommonConstants.USER_SESSION] = userLogin;

                    // Đặt thông báo "Đăng nhập thành công !" với loại là "success".
                    SetAlert("Đăng nhập thành công !", "success");

                    // Trả về một đối tượng JSON với thuộc tính "success" bằng true.
                    return Json(new
                    {
                        success = true
                    });
                }
                else if (result == 0) // Nếu tài khoản bị khóa (result == 0), gán thông báo tương ứng vào biến data.
                {
                    data = "Tài khoản đã bị khóa";
                }
                else if (result == -1) // Nếu tài khoản hoặc mật khẩu không đúng (result == -1), gán thông báo tương ứng vào biến data.
                {
                    data = "Sai tài khoản hoặc mật khẩu";
                }
            }

            // Trả về một đối tượng JSON chứa thông tin trong biến data và thuộc tính "success" bằng false.
            return Json(new
            {
                data = data,
                success = false
            });
        }


        [HttpPost] // Thuộc tính [HttpPost] xác định rằng phương thức này chỉ được gọi bằng HTTP POST request.
        public async Task<ActionResult> Register(RegisterModel model)
        {
            string data = ""; // Khai báo một biến string để lưu trữ dữ liệu.

            if (ModelState.IsValid) // Kiểm tra xem ModelState có hợp lệ không, tức là dữ liệu mà người dùng đã nhập qua form có lỗi không.
            {
                if (model.register_password == model.password_confirm) // Kiểm tra xem mật khẩu và mật khẩu xác nhận có giống nhau không.
                {
                    // Gọi phương thức Register_UngVien từ đối tượng taiKhoanDao để thực hiện đăng ký tài khoản ứng viên.
                    var result = await taiKhoanDao.Register_UngVien(model);

                    if (result > 0) // Nếu đăng ký thành công (result > 0), thực hiện các thao tác sau đây.
                    {
                        // Đặt thông báo "Đăng ký thành công !" với loại là "success".
                        SetAlert("Đăng ký thành công !", "success");

                        // Trả về một đối tượng JSON với thuộc tính "success" bằng true.
                        return Json(new
                        {
                            success = true
                        });
                    }
                    else if (result == 0) // Nếu email đã tồn tại (result == 0), gán thông báo tương ứng vào biến data.
                    {
                        data = "Email này đã tồn tại";
                    }
                    else if (result == -1) // Nếu có lỗi xảy ra trong quá trình đăng ký (result == -1), gán thông báo tương ứng vào biến data.
                    {
                        data = "Đã có lỗi xảy ra. Vui lòng thử lại";
                    }
                }
                else
                {
                    data = "Mật khẩu xác nhận chưa đúng"; // Nếu mật khẩu xác nhận không đúng, gán thông báo tương ứng vào biến data.
                }
            }

            // Trả về một đối tượng JSON chứa thông tin trong biến data và thuộc tính "success" bằng false.
            return Json(new
            {
                data = data,
                success = false
            });
        }

        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null; // Xóa thông tin người dùng khỏi Session.
            SetAlert("Đăng xuất thành công !", "success"); // Đặt thông báo "Đăng xuất thành công !" với loại là "success".
            return Redirect("/"); // Chuyển hướng về trang chủ.
        }

        public async Task<ActionResult> Info()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION]; // Lấy thông tin người dùng từ Session.
            if (session == null) return RedirectToAction("Index"); // Nếu không có thông tin người dùng trong Session, chuyển hướng về trang "Index".
            var member = await ungVienDao.GetByIdClient(session.Id); // Lấy thông tin ứng viên dựa trên ID người dùng trong Session.

            // Đặt dữ liệu hình ảnh vào ViewBag để hiển thị trên trang.
            ViewBag.AnhDaiDien = member.AnhDaiDien;
            ViewBag.AnhBia = member.AnhBia;

            return View(member); // Trả về một trang View "Info" với dữ liệu thông tin ứng viên.
        }


        [HttpPost] // Thuộc tính [HttpPost] xác định rằng phương thức này chỉ được gọi bằng HTTP POST request.
        public async Task<ActionResult> Info(UngVienEditClient member)
        {
            var item = await ungVienDao.GetByIdClient(member.MaUngVien); // Lấy thông tin ứng viên dựa trên ID người dùng.

            // Đặt dữ liệu hình ảnh vào ViewBag để hiển thị trên trang.
            ViewBag.AnhDaiDien = item.AnhDaiDien;
            ViewBag.AnhBia = item.AnhBia;

            if (ModelState.IsValid) // Kiểm tra xem ModelState có hợp lệ không, tức là dữ liệu mà người dùng đã nhập qua form có lỗi không.
            {
                if ((DateTime.Now - DateTime.Parse(member.NgaySinh)).TotalDays / 365 < 18) // Kiểm tra tuổi của ứng viên.
                {
                    ModelState.AddModelError("", "Bạn chưa đủ 18 tuổi"); // Thêm lỗi vào ModelState nếu tuổi không đủ 18.
                    return View();
                }

                // Gọi phương thức UpdateClient từ đối tượng ungVienDao để cập nhật thông tin ứng viên.
                var result = await ungVienDao.UpdateClient(member, Server);

                if (result) // Nếu cập nhật thành công, thực hiện các thao tác sau đây.
                {
                    SetAlert("Cập nhật thành công", "success"); // Đặt thông báo "Cập nhật thành công" với loại là "success".
                    return RedirectToAction("Index"); // Chuyển hướng về trang "Index".
                }
            }

            return View(); // Trả về trang View "Info" nếu có lỗi hoặc không thành công.
        }


        public ActionResult DoiMatKhau()
        {
            return View(); // Trả về một trang View "DoiMatKhau" để cho phép người dùng thay đổi mật khẩu.
        }

        [HttpPost] // Thuộc tính [HttpPost] xác định rằng phương thức này chỉ được gọi bằng HTTP POST request.
        public ActionResult DoiMatKhau(UserPassword model)
        {
            if (ModelState.IsValid) // Kiểm tra xem ModelState có hợp lệ không, tức là dữ liệu mà người dùng đã nhập qua form có lỗi không.
            {
                // Gọi phương thức UpdatePass từ đối tượng taiKhoanDao để cập nhật mật khẩu của người dùng.
                var result = taiKhoanDao.UpdatePass(model, UserLogin().Id);

                if (result > 0) // Nếu cập nhật mật khẩu thành công (result > 0), thực hiện các thao tác sau đây.
                {
                    SetAlert("Cập nhật thành công!", "success"); // Đặt thông báo "Cập nhật thành công" với loại là "success".
                    return RedirectToAction("Index", "Home"); // Chuyển hướng về trang "Index" của HomeController.
                }
                else if (result == 0) // Nếu mật khẩu cũ không chính xác (result == 0), thêm lỗi vào ModelState.
                {
                    ModelState.AddModelError("", "Mật khẩu cũ chưa chính xác");
                }
                else // Nếu có lỗi xảy ra trong quá trình cập nhật (result < 0), đặt thông báo lỗi.
                {
                    SetAlert("Đã có lỗi xảy ra!", "error");
                }
            }

            return View(); // Trả về trang View "DoiMatKhau" nếu có lỗi hoặc không thành công.
        }


        public async Task<ActionResult> BaiViet(int id)
        {
            var model = await new BaiVietDao().GetByIdView(id); // Lấy thông tin bài viết dựa trên ID và gán vào biến model.

            new BaiVietDao().UpdateCount(id); // Gọi phương thức UpdateCount để tăng số lượt xem của bài viết.

            return View(model); // Trả về một trang View "BaiViet" và truyền model (thông tin bài viết) cho nó để hiển thị trên trang web.
        }

    }
}
// xử lý các yêu cầu từ phía người dùng và quản lý việc hiển thị các trang web và dữ liệu liên quan