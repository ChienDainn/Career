using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Areas.Admin.Models;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.TinTuyenDung;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class TinTuyenDungController : BaseController
    {
        private readonly TinTuyenDungDao dao;

        public TinTuyenDungController()
        {
            dao = new TinTuyenDungDao();
        }

        // GET: Admin/TinTuyenDung
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaging(bool trangThai, string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Lấy thông tin đăng nhập của người dùng quản trị từ session.
            var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];

            // Tạo một đối tượng 'GetListPaging' để đóng gói các tham số truy vấn.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức 'GetList' từ đối tượng 'dao' để lấy dữ liệu phân trang dựa trên các tham số.
            var data = dao.GetList(false, request, trangThai, session.Id);

            // Lấy tổng số bản ghi từ dữ liệu phân trang.
            int totalRecord = data.TotalRecord;

            // Tính toán số trang dựa trên tổng số bản ghi và kích thước trang (pageSize).
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu JSON bao gồm dữ liệu danh sách (data.Items), trang hiện tại (pageIndex), tổng số trang (toalPage),
            // và tổng số bản ghi (totalRecord).
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TinChoDuyet()
        {
            // Trả về view 'TinChoDuyet' để hiển thị danh sách tin tuyển dụng cho duyệt.
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            // Sử dụng đối tượng 'dao' để lấy thông tin tin tuyển dụng dựa trên 'id'.
            var member = await dao.GetById(id);

            // Trả về view 'Edit' và truyền đối tượng tin tuyển dụng để hiển thị và chỉnh sửa thông tin.
            return View(member);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TinTuyenDungEdit member)
        {
            // Kiểm tra xem ModelState có hợp lệ không (được thiết lập bởi validation rules).
            if (ModelState.IsValid)
            {
                // Lấy thông tin đăng nhập của người quản trị từ session.
                var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];

                // Gọi phương thức 'EditTTD' từ đối tượng 'dao' để cập nhật thông tin tin tuyển dụng.
                var result = await dao.EditTTD(member, session.Id);

                // Kiểm tra kết quả của việc cập nhật.
                if (result)
                {
                    // Nếu cập nhật thành công, đặt thông báo thành công và chuyển hướng người dùng đến trang "Index".
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index");
                }
            }

            // Trả về view 'Edit' nếu ModelState không hợp lệ hoặc nếu có lỗi xảy ra.
            return View();
        }

        public ActionResult ChoDuyet()
        {
            // Trả về view 'ChoDuyet' để hiển thị danh sách tin tuyển dụng chờ duyệt.
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Duyet(int maTTD)
        {
            // Gọi phương thức 'Duyet' từ đối tượng 'dao' để duyệt một tin tuyển dụng dựa trên 'maTTD'.
            var result = await dao.Duyet(maTTD);

            // Kiểm tra kết quả của việc duyệt.
            if (result)
            {
                // Nếu duyệt thành công, đặt thông báo thành công.
                SetAlert("Duyệt thành công", "success");
            }
            else
            {
                // Nếu có lỗi xảy ra trong quá trình duyệt, đặt thông báo lỗi.
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Trả về kết quả dưới dạng JSON. Kết quả này thường sẽ thông báo duyệt thành công hoặc không thành công.
            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int maTTD)
        {
            // Gọi phương thức 'Delete' từ đối tượng 'dao' để xóa một tin tuyển dụng dựa trên 'maTTD'.
            var result = await dao.Delete(maTTD);

            // Kiểm tra kết quả của việc xóa.
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