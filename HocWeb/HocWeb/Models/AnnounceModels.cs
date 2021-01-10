using System.ComponentModel.DataAnnotations;

namespace HocWeb.Models
{
    public class AnnounceModels
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập tiêu đề ")]
        public string Title { get; set; }
        public string Type { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập thông tin chi tiết ")]
        public string Details { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string Status { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập đường dẫn ")]
        public string Url { get; set; }
    }
}