using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{
    public class FeedBackModels
    {
        public string WarehouseId { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ ")]
        public string address { get; set; }
 
        [Required(ErrorMessage = "Bạn chưa nhập latitude ")]

        public float lat { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập longitude ")]
        public float lng { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên kho ")]
        public string name { get; set; }


        public bool status { get; set; }     
    }
}

