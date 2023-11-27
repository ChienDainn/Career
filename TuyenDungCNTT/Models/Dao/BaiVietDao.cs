using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TuyenDungCNTT.Areas.Admin.Models;
using TuyenDungCNTT.Common;
using TuyenDungCNTT.Models.EF;
using TuyenDungCNTT.Models.ViewModels.BaiViet;
using TuyenDungCNTT.Models.ViewModels.Common;

namespace TuyenDungCNTT.Models.Dao
{
    public class BaiVietDao
    {
        private readonly TuyenDungDbContext dbContext;

        public BaiVietDao()
        {
            dbContext = new TuyenDungDbContext();
        }

        public async Task<BaiVietEdit> GetById(int id)
        {
            try
            {
                // Thử tìm một bài viết trong cơ sở dữ liệu dựa trên ID được cung cấp
                var item = await dbContext.tbl_BaiViet
                    .Where(x => x.PK_iMaBaiViet.Equals(id))
                    .SingleOrDefaultAsync();

                // Kiểm tra nếu không tìm thấy bài viết, trả về giá trị null
                if (item == null)
                    return null;

                // Tạo một đối tượng BaiVietEdit từ thông tin bài viết tìm thấy
                return new BaiVietEdit()
                {
                    MaBaiViet = item.PK_iMaBaiViet,     // Gán ID bài viết
                    AnhChinh = item.sAnhChinh,         // Gán ảnh chính
                    TenBaiViet = item.sTenBaiViet,     // Gán tên bài viết
                    NoiDung = item.sNoiDung,           // Gán nội dung bài viết
                    TenTacGia = GetAuthorByIdBaiViet(id), // Gọi hàm GetAuthorByIdBaiViet để lấy tên tác giả và gán
                    TrangThai = (bool)item.bTrangThai   // Gán trạng thái bài viết
                };
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về giá trị null
                return null;
            }
        }


        public async Task<BaiVietVm> GetByIdView(int id)
        {
            try
            {
                // Thử tìm một bài viết trong cơ sở dữ liệu dựa trên ID được cung cấp
                var item = await dbContext.tbl_BaiViet
                    .Where(x => x.PK_iMaBaiViet.Equals(id))
                    .SingleOrDefaultAsync();

                // Kiểm tra nếu không tìm thấy bài viết, trả về giá trị null
                if (item == null)
                    return null;

                // Tạo một đối tượng BaiVietVm từ thông tin bài viết tìm thấy
                return new BaiVietVm()
                {
                    MaBaiViet = item.PK_iMaBaiViet,             // Gán ID bài viết
                    AnhChinh = item.sAnhChinh,                 // Gán ảnh chính
                    TenBaiViet = item.sTenBaiViet,             // Gán tên bài viết
                    NoiDung = item.sNoiDung,                   // Gán nội dung bài viết
                    TenTacGia = GetAuthorByIdBaiViet((int)item.PK_iMaBaiViet), // Gọi hàm GetAuthorByIdBaiViet để lấy tên tác giả và gán
                    ThoiGian = ((DateTime)(item.dThoiGian)).ToString("dd/MM/yyyy HH:mm:ss"), // Chuyển định dạng thời gian và gán
                    LuotXem = item.iLuotXem                    // Gán số lượt xem
                };
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về giá trị null
                return null;
            }
        }


        public string GetAuthorByIdBaiViet(int id)
        {
            try
            {
                // Tìm bài viết dựa trên ID
                var item = dbContext.tbl_BaiViet.Find(id);

                // Kiểm tra nếu không tìm thấy bài viết, trả về giá trị null
                if (item == null)
                    return null;

                // Lấy ID tác giả từ bài viết
                var authorId = item.FK_iMaTaiKhoan;

                // Tìm tài khoản dựa trên ID tác giả
                var user = dbContext.tbl_TaiKhoan.Find(authorId);

                // Kiểm tra loại quyền của tài khoản để xác định tên tác giả
                if (user.FK_iMaQuyen == CommonConstants.NHATUYENDUNG)
                    return (dbContext.tbl_NhaTuyenDung.Find(authorId)).sTenNTD;
                else if (user.FK_iMaQuyen == CommonConstants.QUANTRIVIEN)
                    return "Admin";

                // Trả về null nếu không tìm thấy quyền tác giả phù hợp
                return null;
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về giá trị null
                return null;
            }
        }


        public ResultPaging<BaiVietVm> GetList(GetListPaging paging, bool trangThai, int idNguoiDung)
        {
            // Lấy danh sách bài viết dựa trên trạng thái (trangThai)
            var model = dbContext.tbl_BaiViet.Where(x => x.bTrangThai == trangThai);

            // Lấy vai trò của người dùng dựa trên ID người dùng (idNguoiDung)
            var role = dbContext.tbl_TaiKhoan.Find(idNguoiDung).FK_iMaQuyen;

            // Nếu người dùng có vai trò "NHATUYENDUNG", lọc danh sách bài viết theo người dùng
            if (role == CommonConstants.NHATUYENDUNG)
            {
                model = model.Where(x => x.FK_iMaTaiKhoan == idNguoiDung);
            }

            // Chuyển đổi danh sách kết quả thành danh sách BaiVietVm và tạo đối tượng BaiVietVm từ thông tin bài viết
            var listItem = model.ToList().Select(x => new BaiVietVm()
            {
                MaBaiViet = x.PK_iMaBaiViet,              // Lấy mã bài viết
                AnhChinh = x.sAnhChinh,                  // Lấy ảnh chính của bài viết
                TenBaiViet = x.sTenBaiViet,              // Lấy tên bài viết
                TenTacGia = GetAuthorByIdBaiViet((int)x.PK_iMaBaiViet),  // Lấy tên tác giả bằng cách gọi hàm GetAuthorByIdBaiViet
                ThoiGian = ((DateTime)(x.dThoiGian)).ToString("dd/MM/yyyy HH:mm:ss"),  // Định dạng thời gian thành chuỗi
                LuotXem = x.iLuotXem,                   // Lấy số lượt xem
                TrangThai = (bool)x.bTrangThai ? "Đã duyệt" : "Chưa được duyệt"  // Kiểm tra trạng thái bài viết và đặt chuỗi tương ứng
            }).ToList();

            // Nếu có từ khóa tìm kiếm, lọc danh sách theo từ khóa
            if (!string.IsNullOrEmpty(paging.keyWord))
            {
                listItem = listItem.Where(x => x.TenBaiViet.Contains(paging.keyWord.Trim())).ToList();
            }

            // Tính tổng số bài viết
            int total = model.Count();

            // Sắp xếp danh sách kết quả theo MaBaiViet, phân trang và trả về kết quả phân trang
            var items = listItem.OrderBy(x => x.MaBaiViet)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .ToList();

            return new ResultPaging<BaiVietVm>()
            {
                Items = items,
                TotalRecord = total
            };
        }


        public List<BaiVietVm> GetListItemHot(int top)
        {
            // Lấy danh sách bài viết có trạng thái true, sắp xếp theo thời gian giảm dần và lấy số lượng top
            var list = dbContext.tbl_BaiViet.Where(x => x.bTrangThai == true) // Lọc bài viết có trạng thái true
                .OrderByDescending(x => x.dThoiGian) // Sắp xếp theo thời gian giảm dần
                .Take(top) // Lấy số lượng top bài viết
                .ToList() // Chuyển kết quả thành danh sách
                .Select(x => new BaiVietVm()
                {
                    MaBaiViet = x.PK_iMaBaiViet, // Lấy mã bài viết
                    AnhChinh = x.sAnhChinh, // Lấy ảnh chính của bài viết
                    TenBaiViet = x.sTenBaiViet, // Lấy tên bài viết
                    TenTacGia = GetAuthorByIdBaiViet((int)x.PK_iMaBaiViet), // Lấy tên tác giả bằng cách gọi hàm GetAuthorByIdBaiViet
                    ThoiGian = ((DateTime)(x.dThoiGian)).ToString("dd/MM/yyyy HH:mm:ss"), // Định dạng thời gian thành chuỗi
                    LuotXem = x.iLuotXem // Lấy số lượt xem
                }).ToList(); // Chuyển kết quả thành danh sách của lớp BaiVietVm

            // Trả về danh sách các bài viết nổi bật
            return list;
        }



        public async Task<int> Create(BaiVietCreate item, HttpServerUtilityBase httpServer, int idNguoiDung)
        {
            try
            {
                // Chuyển đổi tiêu đề bài viết thành dạng không dấu
                var tieude = StringHelper.ToUnsignString(item.TenBaiViet);

                // Kiểm tra xem tiêu đề đã tồn tại chưa
                var check = await dbContext.tbl_BaiViet.Where(x => x.sTieuDe.Equals(tieude)).FirstOrDefaultAsync();
                if (check != null) return 0; // Tiêu đề đã tồn tại, trả về 0

                // Kiểm tra xem có hình ảnh được đính kèm không và lưu hình ảnh vào thư mục
                if (item.Image != null && item.Image.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(item.Image.FileName);
                    string extension = Path.GetExtension(item.Image.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    item.AnhChinh = "/Images/Blogs/" + fileName;
                    fileName = Path.Combine(httpServer.MapPath("~/Images/Blogs/"), fileName);
                    item.Image.SaveAs(fileName);
                }

                // Lấy vai trò của người dùng dựa trên ID người dùng
                var role = (await dbContext.tbl_TaiKhoan.FindAsync(idNguoiDung)).FK_iMaQuyen;

                // Tạo một bài viết mới từ thông tin được cung cấp
                var member = new tbl_BaiViet()
                {
                    sTenBaiViet = item.TenBaiViet,
                    sAnhChinh = item.AnhChinh,
                    FK_iMaTaiKhoan = idNguoiDung,
                    sNoiDung = item.NoiDung,
                    sTieuDe = StringHelper.ToUnsignString(item.TenBaiViet),
                    iLuotXem = 0,
                    dThoiGian = DateTime.Now,
                    bTrangThai = role == CommonConstants.QUANTRIVIEN ? item.TrangThai : false
                };

                // Thêm bài viết vào cơ sở dữ liệu
                dbContext.tbl_BaiViet.Add(member);

                // Lưu thay đổi vào cơ sở dữ liệu và trả về số bản ghi được tạo
                var result = await dbContext.SaveChangesAsync();
                return result;
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về -1 để chỉ ra lỗi
                return -1;
            }
        }


        public async Task<bool> Delete(int maBaiViet)
        {
            try
            {
                // Tìm bài viết dựa trên ID
                var member = await dbContext.tbl_BaiViet.FindAsync(maBaiViet);

                // Kiểm tra nếu không tìm thấy bài viết, trả về false
                if (member == null)
                    return false;

                // Loại bỏ bài viết khỏi cơ sở dữ liệu
                dbContext.tbl_BaiViet.Remove(member);

                // Lưu thay đổi vào cơ sở dữ liệu và kiểm tra xem có thay đổi nào được lưu thành công hay không
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về false
                return false;
            }
        }



        public async Task<bool> Duyet(int maBaiViet)
        {
            try
            {
                // Tìm bài viết dựa trên ID
                var member = await dbContext.tbl_BaiViet.FindAsync(maBaiViet);

                // Kiểm tra nếu không tìm thấy bài viết, trả về false
                if (member == null)
                    return false;

                // Đánh dấu bài viết là "đã duyệt" bằng cách đặt giá trị trangThai thành true
                member.bTrangThai = true;

                // Lưu thay đổi vào cơ sở dữ liệu và kiểm tra xem có thay đổi nào được lưu thành công hay không
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về false
                return false;
            }
        }


        public async Task<bool> UpdateBaiViet(BaiVietEdit item, HttpServerUtilityBase httpServer, int idNguoiDung)
        {
            try
            {
                // Tìm bài viết dựa trên ID
                var member = await dbContext.tbl_BaiViet.FindAsync(item.MaBaiViet);

                // Kiểm tra nếu không tìm thấy bài viết, trả về false
                if (member == null)
                    return false;

                // Kiểm tra nếu có hình ảnh được đính kèm, lưu hình ảnh và cập nhật đường dẫn hình ảnh
                if (item.Image != null && item.Image.ContentLength > 0)
                {
                    // Tạo tên tệp mới cho hình ảnh
                    string fileName = Path.GetFileNameWithoutExtension(item.Image.FileName);
                    string extension = Path.GetExtension(item.Image.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;

                    // Cập nhật đường dẫn hình ảnh trong cơ sở dữ liệu
                    item.AnhChinh = "/Images/Blogs/" + fileName;
                    fileName = Path.Combine(httpServer.MapPath("~/Images/Blogs/"), fileName);
                    item.Image.SaveAs(fileName);
                    member.sAnhChinh = item.AnhChinh;
                }

                // Cập nhật thông tin bài viết
                member.sTenBaiViet = item.TenBaiViet;
                member.sTieuDe = StringHelper.ToUnsignString(item.TenBaiViet);
                member.sNoiDung = item.NoiDung;

                // Lấy vai trò của người dùng dựa trên ID người dùng
                var role = (await dbContext.tbl_TaiKhoan.FindAsync(idNguoiDung)).FK_iMaQuyen;

                // Nếu người dùng có vai trò là QUANTRIVIEN, cập nhật trạng thái bài viết
                if (role == CommonConstants.QUANTRIVIEN)
                {
                    member.bTrangThai = item.TrangThai;
                }

                // Lưu thay đổi vào cơ sở dữ liệu và kiểm tra xem có thay đổi nào được lưu thành công hay không
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về false
                return false;
            }
        }


        public void UpdateCount(int id)
        {
            // Tìm bài viết dựa trên ID
            var item = dbContext.tbl_BaiViet.Find(id);

            // Kiểm tra nếu không tìm thấy bài viết, thoát khỏi phương thức
            if (item == null)
                return;

            // Tăng số lượt xem của bài viết lên 1
            item.iLuotXem = item.iLuotXem + 1;

            // Lưu thay đổi vào cơ sở dữ liệu
            dbContext.SaveChanges();
        }



        public int SlBaiViet(int? idNguoiDung, bool trangThai)
        {
            try
            {
                // Lấy danh sách bài viết có trạng thái truyền vào (true hoặc false)
                var number = dbContext.tbl_BaiViet.Where(x => x.bTrangThai == trangThai);

                // Nếu có ID người dùng được cung cấp, thêm điều kiện lọc theo ID người dùng và đếm số bài viết của người dùng đó
                if (idNguoiDung != null)
                {
                    return number.Where(x => x.FK_iMaTaiKhoan == idNguoiDung).Count();
                }

                // Nếu không có ID người dùng, đếm tổng số bài viết có trạng thái truyền vào
                return number.Count();
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có lỗi xảy ra và trả về 0
                return 0;
            }
        }

    }
}