using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.User;

namespace TuyenDungCNTT.Areas.NhaTuyenDung.Controllers
{
    public class BaseController : Controller
    {
        // Phương thức UserLogin dùng để trả về thông tin người dùng đăng nhập từ session.
        protected UserLogin UserLogin()
        {
            return (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];
        }

        // Phương thức OnActionExecuting được gọi trước khi một hành động (action) của Controller được thực hiện.
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Lấy thông tin người dùng đăng nhập từ session.
            var session = (UserLogin)Session[CommonConstants.EMPLOYER_SESSION];

            // Kiểm tra xem người dùng có đăng nhập hay không.
            if (session == null)
            {
                // Nếu không có người dùng đăng nhập, chuyển hướng họ đến trang đăng nhập.
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index" }));
            }
            else
            {
                // Nếu có người dùng đăng nhập, lấy thông tin người dùng và đặt vào TempData.
                var userLogin = new NhaTuyenDungDao().GetNhaTuyenDung(session.Id);
                TempData["AnhDaiDien"] = userLogin.AnhDaiDien;
            }

            // Gọi phương thức cơ sở của lớp cơ sở để tiếp tục xử lý.
            base.OnActionExecuting(filterContext);
        }


        // Phương thức SetAlert để đặt thông báo và kiểu thông báo trong TempData.
        protected void SetAlert(string message, string type)
        {
            // Đặt thông báo vào TempData với khóa "Notify".
            TempData["Notify"] = message;

            // Dựa trên kiểu thông báo (type) được truyền vào, đặt kiểu thông báo tương ứng.
            // Kiểu thông báo sẽ được sử dụng để hiển thị thông báo với màu nền khác nhau trong giao diện người dùng.
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

// xác thực người dùng, quản lý thông tin đăng nhập, và quản lý thông báo
// cho các Controller con trong ứng dụng web. Controller con có thể kế thừa từ
// BaseController để tái sử dụng các chức năng này và thực hiện logic cụ thể cho từng trang hoặc tính năng.