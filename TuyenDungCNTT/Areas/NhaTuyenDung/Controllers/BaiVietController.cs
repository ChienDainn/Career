using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.BaiViet;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Areas.NhaTuyenDung.Controllers
{
    public class BaiVietController : BaseController
    {// Khai báo trường (field) để lưu trữ thể hiện của lớp BaiVietDao.
        private readonly BaiVietDao dao;

        // Hàm khởi tạo của lớp BaiVietController.
        public BaiVietController()
        {
            // Tạo thể hiện của lớp BaiVietDao và gán cho trường dao.
            dao = new BaiVietDao();
        }

        // Phương thức Index sẽ được gọi khi người dùng truy cập đường dẫn tương ứng.
        public ActionResult Index()
        {
            // Trả về một ActionResult kiểu View, không có nội dung cụ thể ở đây.
            return View();
        }

        // Phương thức GetPaging làm việc với dữ liệu và trả về JSON.
        public ActionResult GetPaging(bool trangThai, string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Lấy thể hiện của lớp UserLogin từ session.
            var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

            // Tạo một yêu cầu (request) dựa trên tham số truyền vào.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức GetList của thể hiện dao để lấy danh sách dữ liệu.
            var data = dao.GetList(request, trangThai, session.Id);

            // Tính tổng số mục dữ liệu.
            int totalRecord = data.TotalRecord;

            // Tính toán số trang dựa trên tổng số mục và số lượng mục trên mỗi trang.
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng JSON, bao gồm danh sách dữ liệu, trang hiện tại, tổng số trang, và tổng số mục.
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }, JsonRequestBehavior.AllowGet);
        }

        // Phương thức Create sẽ được gọi khi người dùng truy cập đường dẫn tạo bài viết (HTTP GET).
        public ActionResult Create()
        {
            // Trả về một ActionResult kiểu View để hiển thị form tạo bài viết.
            return View();
        }

        // Phương thức Create sẽ được gọi khi người dùng gửi yêu cầu tạo bài viết (HTTP POST).
        [HttpPost]
        public async Task<ActionResult> Create(BaiVietCreate member)
        {
            // Kiểm tra xem dữ liệu mà người dùng nhập vào form có hợp lệ hay không.
            if (ModelState.IsValid)
            {
                // Lấy thể hiện của lớp UserLogin từ session.
                var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

                // Gọi phương thức Create của thể hiện dao để tạo bài viết.
                var result = await dao.Create(member, Server, session.Id);

                // Kiểm tra kết quả từ việc tạo bài viết.
                if (result > 0)
                {
                    // Nếu tạo bài viết thành công, đặt thông báo thành công.
                    SetAlert("Tạo bài viết thành công", "success");

                    // Chuyển hướng người dùng đến trang danh sách bài viết.
                    return RedirectToAction("Index");
                }
                else if (result == 0)
                {
                    // Nếu bài viết có tên đã tồn tại, thêm thông báo lỗi vào ModelState.
                    ModelState.AddModelError("", "Tên bài viết đã tồn tại");
                }
                else
                {
                    // Nếu có lỗi xảy ra trong quá trình tạo bài viết, đặt thông báo lỗi.
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }

            // Trả về ActionResult kiểu View để hiển thị lại form với thông báo lỗi nếu có.
            return View();
        }

        [HttpGet]
        // Phương thức Edit sẽ được gọi khi người dùng truy cập đường dẫn chỉnh sửa bài viết với một id cụ thể (HTTP GET).
        public async Task<ActionResult> Edit(int id)
        {
            // Lấy thông tin bài viết dựa trên id từ thể hiện dao.
            var member = await dao.GetById(id);

            // Đặt các thông tin từ bài viết vào ViewBag để sử dụng trong View.
            ViewBag.MaBaiViet = member.MaBaiViet;
            ViewBag.AnhChinh = member.AnhChinh;

            // Trả về ActionResult kiểu View với dữ liệu bài viết cho người dùng chỉnh sửa.
            return View(member);
        }

        // Phương thức Edit sẽ được gọi khi người dùng gửi yêu cầu cập nhật bài viết (HTTP POST).
        [HttpPost]
        public async Task<ActionResult> Edit(BaiVietEdit member)
        {
            // Lấy thông tin bài viết từ thể hiện dao dựa trên MaBaiViet của đối tượng member.
            var item = await dao.GetById(member.MaBaiViet);

            // Đặt các thông tin từ bài viết vào ViewBag để sử dụng trong View.
            ViewBag.MaBaiViet = item.MaBaiViet;
            ViewBag.AnhChinh = item.AnhChinh;

            // Kiểm tra xem dữ liệu mà người dùng nhập vào form có hợp lệ hay không.
            if (ModelState.IsValid)
            {
                // Lấy thông tin đăng nhập từ session.
                var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

                // Gọi phương thức UpdateBaiViet của thể hiện dao để cập nhật bài viết.
                var result = await dao.UpdateBaiViet(member, Server, session.Id);

                // Kiểm tra kết quả từ việc cập nhật bài viết.
                if (result)
                {
                    // Nếu cập nhật thành công, đặt thông báo thành công.
                    SetAlert("Cập nhật thành công", "success");

                    // Chuyển hướng người dùng đến trang danh sách bài viết.
                    return RedirectToAction("Index");
                }
            }

            // Trả về ActionResult kiểu View để hiển thị lại form chỉnh sửa với thông báo lỗi nếu có.
            return View();
        }


        // Phương thức Delete sẽ được gọi khi người dùng gửi yêu cầu xóa bài viết (HTTP POST).
        [HttpPost]
        public async Task<ActionResult> Delete(int maBaiViet)
        {
            // Gọi phương thức Delete của thể hiện dao để xóa bài viết dựa trên mã bài viết.
            var result = await dao.Delete(maBaiViet);

            // Kiểm tra kết quả từ việc xóa bài viết.
            if (result)
            {
                // Nếu xóa thành công, đặt thông báo thành công.
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu có lỗi xảy ra trong quá trình xóa bài viết, đặt thông báo lỗi.
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Trả về kết quả xóa bài viết dưới dạng JSON.
            return Json(result);
        }

        // Phương thức ChoDuyet s

        public ActionResult ChoDuyet()
        {
            return View();
        }
    }
}

//quản lý quá trình tạo, chỉnh sửa
//, xóa và hiển thị bài viết, cũng như hỗ trợ phân trang dữ liệu.