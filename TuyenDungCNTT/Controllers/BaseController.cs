using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Controllers
{
    public class BaseController : Controller
    {
        // lấy thông tin người dùng đăng nhập từ session
        protected UserLogin UserLogin()
        {
            return (UserLogin)Session[CommonConstants.USER_SESSION];
        }

        // Phương thức được gọi trước khi một hành động trong Controller được thực hiện
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Lấy thông tin đăng nhập từ session
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];

            // Kiểm tra nếu session có giá trị
            if (session != null)
            {
                // Sử dụng đối tượng UngVienDao để lấy thông tin người dùng dựa trên session.Id
                var userLogin = new UngVienDao().GetUngVien(session.Id);

                // Lưu hình ảnh đại diện của người dùng vào TempData để sử dụng trong Controller
                TempData["AnhDaiDien"] = userLogin.AnhDaiDien;
            }

            // Gọi phương thức cha (là phương thức OnActionExecuting của Controller cơ sở)
            base.OnActionExecuting(filterContext);
        }

        //ể thiết lập thông báo (alert)
        protected void SetAlert(string message, string type)
        {
            // Lưu thông báo vào TempData để có thể hiển thị ở các view sau khi thực hiện hành động
            TempData["Notify"] = message;

            // Dựa vào tham số 'type', thiết lập loại thông báo hiển thị giao diện
            if (type == "success")
            {
                TempData["AlertType"] = "alert-success";
            }
            else if (type == "warning")
            {
                TempData["AlertType"] = "alert-warning";
            }
            else if (type == "error")
            {
                TempData["AlertType"] = "alert-danger";
            }
        }

    }
}