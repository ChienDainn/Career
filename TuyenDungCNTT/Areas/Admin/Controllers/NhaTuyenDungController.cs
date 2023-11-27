using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.Employer;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class NhaTuyenDungController : BaseController
    {
        NhaTuyenDungDao dao;

        public NhaTuyenDungController()
        {
            dao = new NhaTuyenDungDao();
        }

        // GET: Admin/TuyenDung
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetPaging(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo một đối tượng 'GetListPaging' để đóng gói các tham số truy vấn.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức 'GetList' từ đối tượng 'dao' để lấy dữ liệu phân trang dựa trên 'request'.
            var data = await dao.GetList(request);

            // Lấy tổng số bản ghi từ dữ liệu phân trang.
            int totalRecord = data.TotalRecord;

            // Tính toán số trang dựa trên tổng số bản ghi và kích thước trang (pageSize).
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu JSON bao gồm dữ liệu danh sách (data.Items), trang hiện tại (pageIndex), tổng số trang (toalPage),
            // và tổng số bản ghi (totalRecord).
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            // Trả về view 'Create' để hiển thị một form tạo mới.
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(EmployerEdit member)
        {
            // Kiểm tra xem ModelState có hợp lệ không (được thiết lập bởi validation rules).
            if (ModelState.IsValid)
            {
                // Gọi phương thức 'Create' của đối tượng 'dao' để tạo nhà tuyển dụng dựa trên dữ liệu từ biến 'member'.
                var result = await dao.Create(member);

                // Kiểm tra kết quả của việc tạo nhà tuyển dụng.
                if (result > 0)
                {
                    // Nếu thành công, đặt thông báo thành công và chuyển hướng người dùng đến trang "Index".
                    SetAlert("Tạo nhà tuyển dụng thành công", "success");
                    return RedirectToAction("Index");
                }
                else if (result == 0)
                {
                    // Nếu mã nhà tuyển dụng đã tồn tại, thêm lỗi vào ModelState.
                    ModelState.AddModelError("", "Mã nhà tuyển dụng đã tồn tại");
                }
                else if (result == -2)
                {
                    // Nếu mã nhà tuyển chưa trùng với mã tài khoản đăng ký nhà tuyển dụng, thêm lỗi vào ModelState.
                    ModelState.AddModelError("", "Mã nhà tuyển chưa trùng với mã tài khoản đăng ký nhà tuyển dụng");
                }
                else
                {
                    // Nếu có lỗi khác xảy ra, đặt thông báo lỗi.
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }

            // Trả về view 'Create' nếu ModelState không hợp lệ hoặc nếu có lỗi xảy ra.
            return View();
        }


        // Xác định rằng phương thức này xử lý HTTP GET request.
        [HttpGet]
        public async Task<ActionResult> Detail(int id)
        {
            // Sử dụng đối tượng 'dao' để lấy thông tin thành viên dựa trên giá trị 'id'.
            var member = await dao.GetById(id);

            // Đặt giá trị 'MaNTD' trong ViewBag bằng giá trị từ thuộc tính 'MaNTD' của đối tượng thành viên.
            ViewBag.MaNTD = member.MaNTD;

            // Trả về trang web (view) 'Detail' và truyền đối tượng thành viên để hiển thị thông tin trên trang web.
            return View(member);
        }


        // Đây là một phương thức controller sẽ xử lý HTTP POST request.
        [HttpPost]
        public async Task<ActionResult> Delete(int maNTD)
        {
            // Sử dụng đối tượng 'dao' để thực hiện xóa dựa trên giá trị 'maNTD'.
            var result = await dao.Delete(maNTD);

            // Kiểm tra kết quả của việc xóa. Nếu xóa thành công, đặt thông báo thành công.
            if (result)
            {
                SetAlert("Xóa thành công", "success");
            }
            // Nếu xóa không thành công, đặt thông báo lỗi.
            else
            {
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Trả về kết quả dưới dạng JSON. Kết quả này thường sẽ thông báo xóa thành công hoặc không thành công.
            return Json(result);
        }

    }
}