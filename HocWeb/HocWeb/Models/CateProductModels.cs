using System.ComponentModel.DataAnnotations;

namespace HocWeb.Models
{

    public class CateProductModels
    {
        public string CateProductID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tiêu đề ")]
        public string MetaTitle { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập vị trí ")]
        public string Displayed { get; set; }
        public string CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung ")]
        public string MetaDescription { get; set; }
        public string Status { get; set; }
        public string Icon { get; set; }
    }
}