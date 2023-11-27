using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuyenDungCNTT.Models.ViewModels.Common
{
    public class GetListPaging
    {
        public string keyWord { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}