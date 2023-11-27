using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TuyenDungCNTT.Models.ViewModels.User
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int idQuyen { get; set; }
    }
}