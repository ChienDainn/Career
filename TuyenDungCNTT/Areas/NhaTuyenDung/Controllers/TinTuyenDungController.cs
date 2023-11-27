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
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Areas.NhaTuyenDung.Controllers
{
    public class TinTuyenDungController : BaseController
    {
        private readonly TinTuyenDungDao dao;
        public TinTuyenDungController()
        {
            dao = new TinTuyenDungDao();
        }

        // GET: NhaTuyenDung/TinTuyenDung
        // Phương thức Index được gọi khi người dùng truy cập đường dẫn trang danh sách tin tuyển dụng.

        public ActionResult Index()
        {
            // Trả về một ActionResult kiểu View, không có nội dung cụ thể ở đây.
            return View();
        }


        // Phương thức GetPaging làm việc với dữ liệu và trả về JSON dựa trên các tham số truyền vào.

        public ActionResult GetPaging(bool? hetHan, bool trangThai, string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Lấy thể hiện của lớp UserLogin từ session.
            var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

            // Tạo một yêu cầu (request) dựa trên các tham số truyền vào.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức GetList của thể hiện dao để lấy danh sách dữ liệu tuyển dụng dựa trên các tham số truyền vào.
            var data = dao.GetList(hetHan, request, trangThai, session.Id);

            // Tính tổng số mục dữ liệu.
            int totalRecord = data.TotalRecord;

            // Tính toán số trang dựa trên tổng số mục và số lượng mục trên mỗi trang.
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng JSON, bao gồm danh sách dữ liệu, trang hiện tại, tổng số trang, và tổng số mục.
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChoDuyet()
        {
            return View();
        }

        public ActionResult HetHan()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        // Phương thức Create được gọi khi người dùng truy cập đường dẫn tạo tin tuyển dụng (HTTP GET).

        public async Task<ActionResult> Create(TinTuyenDungCreate item)
        {
            // Kiểm tra xem dữ liệu mà người dùng nhập vào form có hợp lệ hay không.
            if (ModelState.IsValid)
            {
                // Lấy thể hiện của lớp UserLogin từ session để xác định người dùng đang đăng nhập.
                var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

                // Gọi phương thức Create của thể hiện dao để tạo tin tuyển dụng dựa trên đối tượng TinTuyenDungCreate và ID của người dùng đăng nhập.
                var result = await dao.Create(item, session.Id);

                // Kiểm tra kết quả từ việc tạo tin tuyển dụng.
                if (result > 0)
                {
                    // Nếu tạo tin tuyển dụng thành công, đặt thông báo thành công.
                    SetAlert("Tạo tin tuyển dụng thành công", "success");

                    // Chuyển hướng người dùng đến trang danh sách tin tuyển dụng.
                    return RedirectToAction("Index");
                }
                else
                {
                    // Nếu có lỗi xảy ra trong quá trình tạo tin tuyển dụng, đặt thông báo lỗi.
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }

            // Trả về ActionResult kiểu View để hiển thị lại form tạo tin tuyển dụng với thông báo lỗi nếu có.
            return View();
        }

        // Phương thức Edit được gọi khi người dùng truy cập đường dẫn chỉnh sửa tin tuyển dụng với một ID cụ thể (HTTP GET).

        public async Task<ActionResult> Edit(int id)
        {
            // Lấy thông tin tin tuyển dụng dựa trên ID từ thể hiện dao.
            var member = await dao.GetById(id);

            // Trả về ActionResult kiểu View với dữ liệu tin tuyển dụng cho người dùng chỉnh sửa.
            return View(member);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TinTuyenDungEdit member)
        {
            // Kiểm tra xem dữ liệu mà người dùng nhập vào form có hợp lệ hay không.
            if (ModelState.IsValid)
            {
                // Lấy thể hiện của lớp UserLogin từ session để xác định người dùng đang đăng nhập.
                var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

                // Gọi phương thức EditTTD của thể hiện dao để cập nhật thông tin tin tuyển dụng dựa trên thông tin trong đối tượng TinTuyenDungEdit và ID của người dùng đăng nhập.
                var result = await dao.EditTTD(member, session.Id);

                // Kiểm tra kết quả từ việc cập nhật tin tuyển dụng.
                if (result)
                {
                    // Nếu cập nhật thành công, đặt thông báo thành công bằng phương thức SetAlert.
                    SetAlert("Cập nhật thành công", "success");

                    // Chuyển hướng người dùng đến trang danh sách tin tuyển dụng bằng phương thức RedirectToAction.
                    return RedirectToAction("Index");
                }
            }
            // Trả về ActionResult kiểu View để hiển thị lại form chỉnh sửa với thông báo lỗi nếu có.
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int maTTD)
        {
            // Gọi phương thức Delete của thể hiện dao để xóa tin tuyển dụng dựa trên mã tin tuyển dụng.
            var result = await dao.Delete(maTTD);

            // Kiểm tra kết quả từ việc xóa tin tuyển dụng.
            if (result)
            {
                // Nếu xóa thành công, đặt thông báo thành công bằng phương thức SetAlert.
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu có lỗi xảy ra trong quá trình xóa tin tuyển dụng, đặt thông báo lỗi bằng phương thức SetAlert.
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Trả về kết quả xóa tin tuyển dụng dưới dạng JSON.
            return Json(result);
        }

    }
}