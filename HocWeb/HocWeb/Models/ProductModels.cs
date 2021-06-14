using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{
       public class ProductModels
       {
        public string ProductID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên sản phẩm ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập mã sản phẩm ")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tiêu đề ")]
        public string MetaTitle { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Chưa có hình ảnh ")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập giá ")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập giá khuyến mãi")]
        public string PromotionPrice { get; set; }
        public string BrandID { get; set; }
        public string CategoryID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập bảo hành ")]
       public string Warranty { get; set; }
       public string CreatedDate { get; set; }
       public string CreatedBy { get; set; }
       public string ModifiedDate { get; set; }
       public string ModifiedBy { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung ")]
        public string MetaDescription { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập số lượng ")]
        public string Counts { get; set; }
       public object MoreImages { get; set; }
       public object Rating { get; set; }
        
    }
 
}