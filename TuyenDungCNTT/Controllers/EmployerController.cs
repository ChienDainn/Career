using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TuyenDungCNTT.Models.Dao;
using TuyenDungCNTT.Models.ViewModels.Common;

namespace TuyenDungCNTT.Controllers
{
    public class EmployerController : BaseController
    {
        private readonly NhaTuyenDungDao nhatuyendungDao;
        public EmployerController()
        {
            nhatuyendungDao = new NhaTuyenDungDao();
        }

        // GET: Employer
        public async Task<ActionResult> Index(int id)
        {
            var employer = await nhatuyendungDao.GetByIdClient(id);
            return View(employer);
        }

        public ActionResult GetPaging(int idNTD, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                keyWord = null,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = new TinTuyenDungDao().GetListByIdNTD(request, idNTD);
            int totalRecord = data.TotalRecord;
            int toalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, toalPage = toalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet);
        }
    }
}