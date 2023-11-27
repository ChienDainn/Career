using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.UngVien;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class UngVienController : BaseController
    {
        UngVienDao dao;

        public UngVienController()
        {
            dao = new UngVienDao();
        }

        // GET: Admin/UngVien
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetPaging(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo một yêu cầu truy vấn 'GetListPaging' với thông tin từ khóa tìm kiếm, trang hiện tại, và kích thước trang.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức 'GetList' từ đối tượng 'dao' để lấy dữ liệu ứng viên dựa trên yêu cầu truy vấn.
            var data = await dao.GetList(request);

            // Tính toán tổng số bản ghi và số trang cần thiết để hiển thị dữ liệu phân trang.
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng JSON để hiển thị trên trang web. Dữ liệu bao gồm danh sách các bản ghi, trang hiện tại, tổng số trang và tổng số bản ghi.
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UngVienVm ungvien)
        {
            // Kiểm tra xem ModelState có hợp lệ không (được thiết lập bởi validation rules).
            if (ModelState.IsValid)
            {
                // Gọi phương thức 'Create' từ đối tượng 'dao' để tạo ứng viên.
                var result = await dao.Create(ungvien);

                // Kiểm tra kết quả của việc tạo ứng viên.
                if (result > 0)
                {
                    // Nếu tạo ứng viên thành công, đặt thông báo thành công và chuyển hướng người dùng đến trang "Index".
                    SetAlert("Tạo ứng viên thành công", "success");
                    return RedirectToAction("Index");
                }
                else if (result == 0)
                {
                    // Nếu mã ứng viên đã tồn tại, thêm lỗi vào ModelState để hiển thị trên trang tạo ứng viên.
                    ModelState.AddModelError("", "Mã ứng viên đã tồn tại");
                }
                else if (result == -1)
                {
                    // Nếu có lỗi xảy ra trong quá trình tạo ứng viên, đặt thông báo lỗi.
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }
            return View();
        }


        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {
            // Gọi phương thức 'GetById' từ đối tượng 'dao' để lấy thông tin chi tiết ứng viên dựa trên 'id'.
            var ungvien = await dao.GetById(id);

            // Đặt dữ liệu 'MaUngVien' vào ViewBag để có thể sử dụng trong view.
            ViewBag.MaUngVien = ungvien.MaUngVien;

            // Trả về view 'Detail' và truyền thông tin chi tiết ứng viên để hiển thị.
            return View(ungvien);
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int maUngVien)
        {
            // Gọi phương thức 'Delete' từ đối tượng 'dao' để xóa ứng viên dựa trên 'maUngVien'.
            var result = await dao.Delete(maUngVien);

            // Kiểm tra kết quả của việc xóa ứng viên.
            if (result)
            {
                // Nếu xóa thành công, đặt thông báo thành công.
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu có lỗi xảy ra trong quá trình xóa, đặt thông báo lỗi.
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Trả về kết quả dưới dạng JSON. Kết quả này thường sẽ thông báo xóa thành công hoặc không thành công.
            return Json(result);
        }

    }
}