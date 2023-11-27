using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        TaiKhoanDao dao;

        public UserController()
        {
            dao = new TaiKhoanDao();
        }

        // GET: Admin/User
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> GetPaging(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo đối tượng 'request' chứa thông tin phân trang và từ khóa tìm kiếm.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức 'GetList' từ đối tượng 'dao' để lấy dữ liệu phân trang dựa trên thông tin từ 'request'.
            var data = await dao.GetList(request);

            // Tính tổng số bản ghi và số trang dựa trên dữ liệu lấy được.
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng JSON để sử dụng bởi các phần tử giao diện người dùng (cụ thể là JavaScript).
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> Create()
        {
            // Lấy danh sách vai trò và đặt vào ViewBag để sử dụng trong view.
            ViewBag.ListRole = await dao.GetAllRole();

            // Trả về view 'Create' để người dùng có thể tạo tài khoản mới hoặc xem danh sách vai trò.
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Create(UserCreate userCreate)
        {
            // Lấy danh sách vai trò và đặt vào ViewBag để sử dụng trong view.
            ViewBag.ListRole = await dao.GetAllRole();

            // Kiểm tra xem ModelState có hợp lệ không (được thiết lập bởi validation rules).
            if (ModelState.IsValid)
            {
                // Gọi phương thức 'Create' từ đối tượng 'dao' để tạo tài khoản dựa trên thông tin từ biến 'userCreate'.
                var result = await dao.Create(userCreate);

                // Kiểm tra kết quả của việc tạo tài khoản.
                if (result > 0)
                {
                    // Nếu tạo tài khoản thành công, đặt thông báo thành công và chuyển hướng người dùng đến trang "Index".
                    SetAlert("Tạo tài khoản thành công", "success");
                    return RedirectToAction("Index");
                }
                else if (result == 0)
                {
                    // Nếu email đã tồn tại, thêm lỗi vào ModelState để hiển thị trên trang tạo tài khoản.
                    ModelState.AddModelError("", "Email này đã tồn tại");
                }
                else if (result == -1)
                {
                    // Nếu có lỗi khác xảy ra trong quá trình tạo tài khoản, đặt thông báo lỗi.
                    SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                }
            }

            // Trả về view 'Create' để người dùng có thể tạo tài khoản hoặc xem thông báo lỗi (nếu có).
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            // Lấy thông tin tài khoản dựa trên 'id' và đặt vào biến 'user'.
            var user = await dao.GetById(id);

            // Lấy danh sách vai trò và đặt vào ViewBag để sử dụng trong view.
            ViewBag.ListRole = await dao.GetAllRole();

            // Trả về view 'Edit' và truyền thông tin tài khoản và danh sách vai trò để hiển thị trang chỉnh sửa tài khoản.
            return View(user);
        }


        [HttpPost]
        public async Task<ActionResult> Edit(UserEdit user)
        {
            // Kiểm tra xem ModelState có hợp lệ không (được thiết lập bởi validation rules).
            if (ModelState.IsValid)
            {
                // Gọi phương thức 'UpdateUser' từ đối tượng 'dao' để cập nhật thông tin tài khoản dựa trên thông tin từ biến 'user'.
                var result = await dao.UpdateUser(user);

                // Kiểm tra kết quả của việc cập nhật tài khoản.
                if (result)
                {
                    // Nếu cập nhật thành công, đặt thông báo thành công và chuyển hướng người dùng đến trang "Index".
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index");
                }
            }

            // Trả về view 'Edit' để người dùng có thể tiếp tục chỉnh sửa tài khoản hoặc xem thông báo lỗi (nếu có).
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> Delete(int userId)
        {
            // Gọi phương thức 'Delete' từ đối tượng 'dao' để xóa tài khoản dựa trên 'userId'.
            var result = await dao.Delete(userId);

            // Kiểm tra kết quả của việc xóa tài khoản.
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

        public async Task<ActionResult> RoleAssign(int id)
        {
            // Lấy thông tin tài khoản dựa trên 'id' và đặt vào biến 'user'.
            var user = await dao.GetRoleById(id);

            // Lấy danh sách vai trò và đặt vào ViewBag để sử dụng trong view.
            ViewBag.ListRole = await dao.GetAllRole();

            // Trả về view 'RoleAssign' và truyền thông tin tài khoản và danh sách vai trò để hiển thị trang phân quyền.
            return View(user);
        }

        [HttpPost]
        public async Task<ActionResult> RoleAssign(UserRole userRole)
        {
            // Kiểm tra xem ModelState có hợp lệ không (được thiết lập bởi validation rules).
            if (ModelState.IsValid)
            {
                // Gọi phương thức 'UpdateRole' từ đối tượng 'dao' để cập nhật vai trò cho tài khoản dựa trên thông tin từ biến 'userRole'.
                var result = await dao.UpdateRole(userRole);

                // Kiểm tra kết quả của việc cập nhật vai trò.
                if (result)
                {
                    // Nếu cập nhật thành công, đặt thông báo thành công và chuyển hướng người dùng đến trang "Index".
                    SetAlert("Phân quyền thành công", "success");
                    return RedirectToAction("Index");
                }
            }

            // Trả về view 'RoleAssign' để người dùng có thể tiếp tục phân quyền hoặc xem thông báo lỗi (nếu có).
            return View();
        }

        public ActionResult ChangePassword()
        {
            // Trả về trang 'ChangePassword' để người dùng có thể thay đổi mật khẩu.
            return View();
        }

    }
}