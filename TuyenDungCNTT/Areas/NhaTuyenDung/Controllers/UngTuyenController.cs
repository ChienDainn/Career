using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.HoSoXinViec;
using TuyenDungCNTT.Models.ViewModels.UngTuyen;

namespace TuyenDungCNTT.Areas.NhaTuyenDung.Controllers
{
    public class UngTuyenController : BaseController
    {
        private readonly UngTuyenDao ungTuyenDao;
        public UngTuyenController()
        {
            ungTuyenDao = new UngTuyenDao();
        }

        // GET: NhaTuyenDung/UngTuyen
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetPaging(bool status, int month, string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo một yêu cầu (request) dựa trên tham số truyền vào.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức GetListByNTD để lấy danh sách ứng viên dựa trên các thông tin đã cung cấp.
            var data = new UngTuyenDao().GetListByNTD(status, month, request, UserLogin().Id);

            // Tính tổng số mục dữ liệu.
            int totalRecord = data.TotalRecord;

            // Tính toán số trang dựa trên tổng số mục và số lượng mục trên mỗi trang.
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng JSON.
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ChuaXem()
        {
            return View();
        }

        public async Task<RedirectResult> XemCV(int maUV, int maTTD)
        {
            // Gọi phương thức UpdateStatus để cập nhật trạng thái của ứng viên.
            var link = await ungTuyenDao.UpdateStatus(maUV, maTTD);

            // Trả về RedirectPermanent để chuyển hướng người dùng đến đường dẫn (link) để xem CV.
            return RedirectPermanent(link);
        }


        public ActionResult TimUngVien()
        {
            return View();
        }

        public ActionResult DeXuat()
        {
            // Tạo một thể hiện của lớp HoSoXinViecDao và gọi phương thức DeXuatUngVien để lấy danh sách ứng viên được đề xuất.
            var model = new HoSoXinViecDao().DeXuatUngVien(UserLogin().Id);

            // Trả về View với dữ liệu danh sách ứng viên được đề xuất.
            return View(model);
        }

        public ActionResult CapNhat(int maUV, int maTTD)
        {
            // Gọi phương thức GetItem của thể hiện ungTuyenDao để lấy thông tin ứng viên dựa trên mã ứng viên (maUV) và mã tin tuyển dụng (maTTD).
            var model = ungTuyenDao.GetItem(maUV, maTTD);

            // Trả về View với dữ liệu thông tin ứng viên để cập nhật.
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CapNhat(int MaUngVien, int MaTTD, string trangThai)
        {
            // Gọi phương thức UpdateStatus của thể hiện ungTuyenDao để cập nhật trạng thái ứng viên.
            var result = await ungTuyenDao.UpdateStatus(MaUngVien, MaTTD, trangThai);

            // Kiểm tra kết quả từ việc cập nhật trạng thái.
            if (result)
            {
                // Nếu cập nhật thành công, đặt thông báo thành công.
                SetAlert("Cập nhật thành công", "success");
            }
            else
            {
                // Nếu có lỗi xảy ra trong quá trình cập nhật, đặt thông báo lỗi.
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }

            // Chuyển hướng người dùng về trang Index sau khi cập nhật trạng thái.
            return RedirectToAction("Index");
        }


        public JsonResult GetSearchPaging(string KeyWord, int pageIndex, string CapBac, string ChuyenNganh, string LoaiCV)
        {
            // Tạo một yêu cầu (request) dựa trên thông tin được truyền vào từ các tham số.
            var request = new HoSoXinViecSearch()
            {
                CapBac = CapBac,
                ChuyenNganh = ChuyenNganh,
                LoaiCV = LoaiCV
            };

            // Tạo yêu cầu phân trang (paging) dựa trên từ khóa tìm kiếm, trang hiện tại, và kích thước trang.
            var paging = new GetListPaging()
            {
                keyWord = KeyWord.ToLower(),
                PageIndex = pageIndex,
                PageSize = 5
            };

            // Gọi phương thức GetListSearch của thể hiện dao để lấy danh sách hồ sơ xin việc dựa trên yêu cầu tìm kiếm và yêu cầu phân trang.
            var data = new HoSoXinViecDao().GetListSearch(paging, request);

            // Tính tổng số mục dữ liệu.
            int totalRecord = data.TotalRecord;

            // Tính toán số trang dựa trên tổng số mục và số lượng mục trên mỗi trang.
            int toalPage = (int)Math.Ceiling((double)totalRecord / paging.PageSize);

            // Trả về dữ liệu dưới dạng JSON, bao gồm danh sách dữ liệu, trang hiện tại, tổng số trang, và tổng số mục.
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }, JsonRequestBehavior.AllowGet);
        }

    }
}