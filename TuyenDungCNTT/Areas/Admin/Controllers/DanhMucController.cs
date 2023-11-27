using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;
using TuyenDungCNTT.Models.ViewModels.DanhMuc;

namespace TuyenDungCNTT.Areas.Admin.Controllers
{
    public class DanhMucController : BaseController
    {
        private DanhMucDao dao;

        public DanhMucController()
        {
            dao = new DanhMucDao();
        }

        // GET: Admin/DanhMuc
        public ActionResult ChuyenNganh()
        {
            return View();
        }

        public ActionResult GetPagingChuyenNganh(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = dao.GetListChuyenNganh(request);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateChuyenNganh(ChuyenNganh item)
        {
            var result = await dao.CreateChuyenNganh(item);
            if (result > 0)
            {
                SetAlert("Tạo thành công", "success");
            }
            else if (result == 0)
            {
                SetAlert("Chuyên ngành đã tồn tại", "error");
            }
            else
            {
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> EditChuyenNganh(ChuyenNganh item)
        {
            var result = await dao.EditChuyenNganh(item);
            if (result)
            {
                SetAlert("Cập nhật thành công", "success");
            }
            else
            {
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteChuyenNganh(string MaCN)
        {
            var result = await dao.DeleteChuyenNganh(MaCN);
            if (result)
            {
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }
            return Json(result);
        }

        public ActionResult CapBac()
        {
            return View();
        }

        public ActionResult GetPagingCapBac(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo một đối tượng request của kiểu GetListPaging và đặt các thông tin cần thiết.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức GetListCapBac của đối tượng dao để lấy dữ liệu phân trang dựa trên request.
            var data = dao.GetListCapBac(request);

            // Tính tổng số bản ghi (totalRecord) và tổng số trang (totalPage) dựa trên dữ liệu trả về và kích thước trang (pageSize).
            int totalRecord = data.TotalRecord;
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng phản hồi JSON, bao gồm dữ liệu trang hiện tại, số trang tổng cộng và tổng số bản ghi.
            return Json(new
            {
                data = data.Items,
                pageCurrent = pageIndex,
                totalPage = totalPage,
                totalRecord = totalRecord
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<ActionResult> CreateCapBac(CapBac item)
        {
            // Gọi phương thức CreateCapBac của đối tượng dao để tạo một cấp bậc mới dựa trên thông tin từ đối tượng CapBac (item).
            var result = await dao.CreateCapBac(item);

            if (result > 0)
            {
                // Nếu tạo thành công (kết quả > 0), đặt thông báo thành công và loại "success."
                SetAlert("Tạo thành công", "success");
            }
            else if (result == 0)
            {
                // Nếu kết quả là 0, tức là cấp bậc đã tồn tại, đặt thông báo lỗi và loại "error."
                SetAlert("Cấp bậc đã tồn tại", "error");
            }
            else
            {
                // Nếu có lỗi trong quá trình tạo cấp bậc, đặt thông báo lỗi và loại "error."
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả tạo cấp bậc (result).
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> EditCapBac(CapBac item)
        {
            // Gọi phương thức EditCapBac của đối tượng dao để cập nhật thông tin cấp bậc dựa trên thông tin từ đối tượng CapBac (item).
            var result = await dao.EditCapBac(item);

            if (result)
            {
                // Nếu cập nhật thành công (kết quả là true), đặt thông báo thành công và loại "success."
                SetAlert("Cập nhật thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình cập nhật, đặt thông báo lỗi và loại "error."
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả cập nhật (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteCapBac(string MaCB)
        {
            // Gọi phương thức DeleteCapBac của đối tượng dao để xóa cấp bậc dựa trên mã cấp bậc (MaCB).
            var result = await dao.DeleteCapBac(MaCB);

            if (result)
            {
                // Nếu xóa thành công (kết quả là true), đặt thông báo thành công và loại "success."
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình xóa, đặt thông báo lỗi và loại "error."
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả xóa (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }

        public ActionResult GetPagingDiaChi(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo một đối tượng request của kiểu GetListPaging và đặt các thông tin cần thiết.
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            // Gọi phương thức GetListDiaChi của đối tượng dao để lấy dữ liệu địa chỉ phân trang dựa trên request.
            var data = dao.GetListDiaChi(request);

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


        [HttpPost]
        public async Task<ActionResult> CreateDiaChi(DiaChi item)
        {
            // Gọi phương thức CreateDiaChi của đối tượng dao để tạo địa chỉ dựa trên thông tin từ đối tượng DiaChi (item).
            var result = await dao.CreateDiaChi(item);

            if (result > 0)
            {
                // Nếu tạo thành công (kết quả > 0), đặt thông báo thành công và loại "success."
                SetAlert("Tạo thành công", "success");
            }
            else if (result == 0)
            {
                // Nếu kết quả là 0, tức là địa chỉ đã tồn tại, đặt thông báo lỗi và loại "error."
                SetAlert("Địa chỉ đã tồn tại", "error");
            }
            else
            {
                // Nếu có lỗi trong quá trình tạo địa chỉ, đặt thông báo lỗi và loại "error."
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả tạo mới (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> EditDiaChi(DiaChi item)
        {
            // Gọi phương thức EditDiaChi của đối tượng dao để cập nhật thông tin địa chỉ dựa trên thông tin từ đối tượng DiaChi (item).
            var result = await dao.EditDiaChi(item);

            if (result)
            {
                // Nếu cập nhật thành công (kết quả là true), đặt thông báo thành công và loại "success".
                SetAlert("Cập nhật thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình cập nhật, đặt thông báo lỗi và loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả cập nhật (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> DeleteDiaChi(string MaDC)
        {
            // Gọi phương thức DeleteDiaChi của đối tượng dao để xóa địa chỉ dựa trên mã địa chỉ (MaDC).
            var result = await dao.DeleteDiaChi(MaDC);

            if (result)
            {
                // Nếu kết quả là true, tức là xóa thành công,
                // đặt thông báo thành công và loại "success".
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình xóa, đặt thông báo lỗi và loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả xóa (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }


        public ActionResult MucLuong()
        {
            return View();
        }

        public ActionResult GetPagingMucLuong(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = dao.GetListMucLuong(request);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateMucLuong(MucLuong item)
        {
            // Gọi phương thức CreateMucLuong của đối tượng dao để tạo mới mức lương dựa trên đối tượng MucLuong (item).
            var result = await dao.CreateMucLuong(item);

            if (result > 0)
            {
                // Nếu kết quả là số nguyên dương, tức là tạo thành công,
                // đặt thông báo thành công và loại "success".
                SetAlert("Tạo thành công", "success");
            }
            else if (result == 0)
            {
                // Nếu kết quả là 0, tức là mức lương đã tồn tại, đặt thông báo lỗi và loại "error".
                SetAlert("Mức lương đã tồn tại", "error");
            }
            else
            {
                // Nếu có lỗi trong quá trình tạo mức lương, đặt thông báo lỗi và loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả tạo mới (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }

        [HttpPost]
        public async Task<ActionResult> EditMucLuong(MucLuong item)
        {
            // Gọi phương thức EditMucLuong của đối tượng dao để cập nhật thông tin mức lương dựa trên đối tượng MucLuong (item).
            var result = await dao.EditMucLuong(item);

            if (result)
            {
                // Nếu kết quả là true, tức là cập nhật thành công, đặt thông báo thành công và loại "success".
                SetAlert("Cập nhật thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình cập nhật mức lương, đặt thông báo lỗi và loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả cập nhật (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }


        [HttpPost]
        public async Task<ActionResult> DeleteMucLuong(string MaML)
        {
            // Gọi phương thức DeleteMucLuong của đối tượng dao để xóa mức lương dựa trên mã mức lương (MaML).
            var result = await dao.DeleteMucLuong(MaML);

            if (result)
            {
                // Nếu kết quả là true, tức là xóa thành công, đặt thông báo thành công và loại "success".
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình xóa mức lương, đặt thông báo lỗi và loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON với kết quả xóa (result) để thông báo cho client về kết quả thực hiện.
            return Json(result);
        }


        // Phương thức GetPagingLoaiCV được sử dụng để lấy dữ liệu phân trang của danh sách loại công việc.
        public ActionResult GetPagingLoaiCV(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            // Tạo một đối tượng request của kiểu GetListPaging và đặt các thông tin cần thiết.
            var request = new GetListPaging()
            {
                keyWord = keyWord,    // Từ khóa tìm kiếm (nếu có).
                PageIndex = pageIndex, // Trang hiện tại.
                PageSize = pageSize    // Kích thước trang.
            };

            // Gọi phương thức GetListLoaiCV của đối tượng dao để lấy dữ liệu dựa trên request.
            var data = dao.GetListLoaiCV(request);

            // Tính tổng số bản ghi (totalRecord) và tổng số trang (totalPage) dựa trên dữ liệu trả về và kích thước trang (pageSize).
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Trả về dữ liệu dưới dạng phản hồi JSON, bao gồm dữ liệu trang hiện tại, số trang tổng cộng và tổng số bản ghi.
            return Json(new
            {
                data = data.Items,     // Danh sách các loại công việc trang hiện tại.
                pageCurrent = pageIndex, // Trang hiện tại.
                toalPage = toalPage,    // Tổng số trang.
                totalRecord = totalRecord // Tổng số bản ghi.
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        // Phương thức CreateLoaiCV dùng để tạo một loại công việc mới dựa trên thông tin từ đối tượng LoaiCongViec (item).
        public async Task<ActionResult> CreateLoaiCV(LoaiCongViec item)
        {
            // Gọi phương thức CreateLoaiCV của đối tượng dao để thực hiện tạo loại công việc.
            var result = await dao.CreateLoaiCV(item);

            if (result > 0)
            {
                // Nếu tạo thành công (kết quả > 0), đặt thông báo thành công với loại "success".
                SetAlert("Tạo thành công", "success");
            }
            else if (result == 0)
            {
                // Nếu kết quả là 0, tức là loại công việc đã tồn tại, đặt thông báo lỗi với loại "error".
                SetAlert("Loại công việc đã tồn tại", "error");
            }
            else
            {
                // Nếu có lỗi trong quá trình tạo, đặt thông báo lỗi với loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON chứa kết quả của quá trình tạo loại công việc (result).
            return Json(result);
        }


        [HttpPost]
        // Phương thức EditLoaiCV dùng để cập nhật thông tin một loại công việc dựa trên thông tin từ đối tượng LoaiCongViec (item).
        public async Task<ActionResult> EditLoaiCV(LoaiCongViec item)
        {
            // Gọi phương thức EditLoaiCV của đối tượng dao để thực hiện cập nhật loại công việc.
            var result = await dao.EditLoaiCV(item);

            if (result)
            {
                // Nếu cập nhật thành công (result là true), đặt thông báo thành công với loại "success".
                SetAlert("Cập nhật thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình cập nhật, đặt thông báo lỗi với loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON chứa kết quả của quá trình cập nhật loại công việc (result).
            return Json(result);
        }

        [HttpPost]
        // Phương thức DeleteLoaiCV dùng để xóa một loại công việc dựa trên mã loại công việc (MaLoaiCV).
        public async Task<ActionResult> DeleteLoaiCV(string MaLoaiCV)
        {
            // Gọi phương thức DeleteLoaiCV của đối tượng dao để thực hiện xóa loại công việc.
            var result = await dao.DeleteLoaiCV(MaLoaiCV);

            if (result)
            {
                // Nếu xóa thành công (result là true), đặt thông báo thành công với loại "success".
                SetAlert("Xóa thành công", "success");
            }
            else
            {
                // Nếu có lỗi trong quá trình xóa, đặt thông báo lỗi với loại "error".
                SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
            }

            // Trả về một phản hồi JSON chứa kết quả của quá trình xóa loại công việc (result).
            return Json(result);
        }

    }
}