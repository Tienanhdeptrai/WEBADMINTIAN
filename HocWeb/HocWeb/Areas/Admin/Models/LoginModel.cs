using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Web;

namespace HocWeb.Areas.Admin.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Bạn chưa nhập Tài Khoản ") ]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Độ dài mật khẩu ít nhất 4 ký tự ")]
        public string TenTK { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập Mật Khẩu ")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự ")]
        public string MatKhau { get; set; }
        public bool RememberMe { get; set; }
    }
}