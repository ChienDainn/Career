using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TuyenDungCNTT.Areas.Admin.Models;
using TuyenDungCNTT.Common;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        // Phương thức này được gọi trước khi một action trong controller con được thực thi.
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Kiểm tra xem người dùng đã đăng nhập vào hệ thống hay chưa.
            var session = (AdminLogin)Session[CommonConstants.ADMIN_SESSION];

            if (session == null)
            {
                // Nếu người dùng chưa đăng nhập, chuyển hướng người dùng đến trang đăng nhập trong khu vực "Admin."
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Login", action = "Index", Area = "Admin" }));
            }

            base.OnActionExecuting(filterContext);
        }

        // Phương thức dùng để đặt thông báo để hiển thị cho người dùng.
        protected void SetAlert(string message, string type)
        {
            // Lưu thông báo và loại thông báo vào TempData để truyền chúng đến view sau khi một action đã hoàn thành.
            TempData["AlertMessage"] = message;

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
