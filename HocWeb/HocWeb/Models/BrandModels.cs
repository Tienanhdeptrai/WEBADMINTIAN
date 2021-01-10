using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{

    public class BrandModels
    {
        public string BrandID { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tên ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung ")]
        public string MetaDescription { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tiêu đề ")]
        public string MetaTitle { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string Status { get; set; }

        [Required(ErrorMessage = "Chưa có hình ảnh ")]
        public string Image { get; set; }
    }
}