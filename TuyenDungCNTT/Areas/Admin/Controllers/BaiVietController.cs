using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Areas.Admin.Models;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.BaiViet;
using TuyenDungCNTT.Models.ViewModels.Common;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class BaiVietController : BaseController
    {
        private readonly BaiVietDao dao;

        public BaiVietController()
        {
            dao = new BaiVietDao();
        }

        // GET: Admin/BaiViet
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaging(bool trangThai, string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Phương thức GetPaging được sử dụng để trả về dữ liệu phân trang dựa trên các tham số truyền vào.

            // Lấy thông tin người dùng đăng nhập từ Session và lưu vào biến session.
            var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];

            // Tạo một đối tượng request của kiểu GetListPaging và đặt các thông tin cần thiết.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức GetList của đối tượng dao để lấy dữ liệu phân trang dựa trên request và các thông tin khác.
            var data = dao.GetList(request, trangThai, session.Id);

            // Tính tổng số bản ghi (totalRecord) và tổng số trang (totalPage) dựa trên dữ liệu trả về và kích thước trang (pageSize).
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng phản hồi JSON, bao gồm dữ liệu trang hiện tại, số trang tổng cộng và tổng số bản ghi.
            return Json(new
            {
                data = data.Items,
                pageCurrent = pageIndex,
                toalPage = toalPage,
                totalRecord = totalRecord
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(BaiVietCreate member)
        {
            // Phương thức Create(BaiVietCreate member) dùng để tạo một bài viết dựa trên thông tin từ đối tượng BaiVietCreate.

            // Kiểm tra xem ModelState có hợp lệ không. ModelState chứa thông tin về lỗi kiểm tra dữ liệu.
            if (ModelState.IsValid)
            {
                // Nếu ModelState hợp lệ, tiếp tục kiểm tra quyền truy cập của người dùng.

                // Lấy thông tin người dùng đăng nhập từ Session và lưu vào biến session.
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];

                // Gọi phương thức Create để tạo bài viết, truyền vào các tham số cần thiết.
                var result = await dao.Create(member, Server, session.Id);

                if (result > 0)
                {
                    // Nếu tạo bài viết thành công (kết quả > 0), đặt thông báo thành công và chuyển hướng đến trang "Index".
                    SetAlert("Tạo bài viết thành công", "success");
                    return RedirectToAction("Index");
                }
                else if (result == 0)
                {
                    // Nếu kết quả là 0, tức là tên bài viết đã tồn tại, thêm lỗi vào ModelState.
                    ModelState.AddModelError("", "Tên bài viết đã tồn tại");
                }
                else
                {
                    // Nếu có lỗi trong quá trình tạo bài viết, đặt thông báo lỗi.
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }

            // Nếu có lỗi hoặc ModelState không hợp lệ, trả về view "Create" để người dùng có thể thử lại hoặc xem giao diện tạo bài viết.
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Phương thức Edit(int id) dùng để hiển thị giao diện sửa thông tin bài viết dựa trên mã bài viết (id).

            // Lấy thông tin bài viết dựa trên mã bài viết (id) từ đối tượng dao và lưu vào biến member.
            var member = await dao.GetById(id);

            // Gán mã bài viết và đường dẫn ảnh chính vào ViewBag để sử dụng trong view.
            ViewBag.MaBaiViet = member.MaBaiViet;
            ViewBag.AnhChinh = member.AnhChinh;

            // Trả về view "Edit" và truyền thông tin bài viết (member) vào view để người dùng có thể xem và sửa.
            return View(member);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(BaiVietEdit member)
        {
            // Phương thức Edit nhận một đối tượng BaiVietEdit làm tham số, đại diện cho thông tin bài viết cần chỉnh sửa.

            // Lấy thông tin bài viết hiện tại dựa trên mã bài viết từ đối tượng dao và lưu vào biến item.
            var item = await dao.GetById(member.MaBaiViet);

            // Gán mã bài viết và đường dẫn ảnh chính vào ViewBag để sử dụng trong view.
            ViewBag.MaBaiViet = item.MaBaiViet;
            ViewBag.AnhChinh = item.AnhChinh;

            // Kiểm tra xem ModelState có hợp lệ không. ModelState chứa thông tin về lỗi kiểm tra dữ liệu.
            if (ModelState.IsValid)
            {
                // Nếu ModelState hợp lệ, tiếp tục kiểm tra quyền truy cập của người dùng.

                // Lấy thông tin người dùng đăng nhập từ Session và lưu vào biến session.
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];

                // Gọi phương thức UpdateBaiViet để cập nhật thông tin bài viết, truyền vào các tham số cần thiết.
                var result = await dao.UpdateBaiViet(member, Server, session.Id);

                if (result)
                {
                    // Nếu cập nhật thành công, đặt thông báo thành công và chuyển hướng đến trang "Index".
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index");
                }
            }

            // Nếu có lỗi hoặc ModelState không hợp lệ, trả về view "Edit" để người dùng có thể xem và sửa thông tin lại.
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int maBaiViet)
        {
            // Phương thức Delete(int maBaiViet) dùng để xóa một bài viết với mã bài viết là maBaiViet.
            var result = await dao.Delete(maBaiViet);

            if (result)
            {
                // Nếu kết quả là true, tức là xóa thành công,
                // thì gọi phương thức SetAlert để đặt thông báo thành công với loại "success".
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu kết quả là false, tức là có lỗi xảy ra trong quá trình xóa,
                // thì gọi phương thức SetAlert để đặt thông báo lỗi với loại "error".
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Trả về một phản hồi JSON chứa kết quả xóa bài viết (result).
            return Json(result);
        }

        public ActionResult ChoDuyet()
        {
            // Phương thức ChoDuyet() trả về một view hiển thị danh sách bài viết chờ duyệt.
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Duyet(int maBaiViet)
        {
            // Phương thức Duyet(int maBaiViet) dùng để duyệt một bài viết với mã bài viết là maBaiViet.

            // Gọi phương thức Duyet của đối tượng dao và lưu kết quả vào biến result.
            var result = await dao.Duyet(maBaiViet);

            if (result)
            {
                // Nếu kết quả là true, tức là việc duyệt thành công,
                // thì gọi phương thức SetAlert để đặt thông báo thành công với loại "success".
                SetAlert("Duyệt thành công", "success");
            }
            else
            {
                // Nếu kết quả là false, tức là có lỗi xảy ra trong quá trình duyệt,
                // thì gọi phương thức SetAlert để đặt thông báo lỗi với loại "error".
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Trả về một phản hồi JSON với kết quả duyệt bài viết (result).
            return Json(result);
        }

    }
}

//Controller này quản lý các tác vụ liên quan đến quản lý và quản trị bài viết,
//bao gồm việc hiển thị danh sách bài viết, tạo, sửa, xóa bài viết, duyệt bài viết,
//và các tác vụ quản lý khác liên quan đến nội dung bài viết. 