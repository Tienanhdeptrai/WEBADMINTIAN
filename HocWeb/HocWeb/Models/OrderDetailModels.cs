using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{
    public class OrderDetailModels
    {
        public string OrderDetailID { get; set; }
        public string ProductID { get; set; }
        public string Picture { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        public bool UserProduct { get; set; }
        public string detailStatus { get; set; }
        public string UserID { get; set; }
    }
}

