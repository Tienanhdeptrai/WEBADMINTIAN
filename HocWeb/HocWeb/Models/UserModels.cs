﻿using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{
    public class MechantModel
    {
        public bool Merchant { get; set; }
    }
  
    public class UserModels
    {
        public string UserID { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tài khoản ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập email ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu ")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Độ dài mật khẩu ít nhất 6 ký tự ")]
        public string Passwords { get; set; }

        public string Avatar { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập họ tên ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại ")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập CMND ")]
        public string CMND { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ ")]

        public string CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string Status { get; set; }

        public bool Merchant { get; set; }

    }
}