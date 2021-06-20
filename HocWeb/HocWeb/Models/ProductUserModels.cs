using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{
    public class ProductUserModels
    {
        public string ProductID { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên sản phẩm ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung chi tiết ")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Chưa có hình ảnh ")]
        public string Image { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập nội dung ")]
        public string MetaDescription { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập giá ")]
        public string Price { get; set; }
        
        [Required(ErrorMessage = "Bạn chưa nhập giá khuyến mãi ")]
        public string PromotionPrice { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập số lượng")]
        public int Count { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập bảo hành")]
        public string Warranty { get; set; }
        public string CategoryID { get; set; }
        public bool Status { get; set; }
        public string UserID { get; set; }
        public object Rating { get; set; }

        public string CreatedDate { get; set; }

        public string MoreImage { get; set; }
    }

}