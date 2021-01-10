using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{
    public class OrderDetailModels
    {
        public string OrderDetailID { get; set; }
        public string ProductID { get; set; }
        public string Picture { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string BrandID { get; set; }
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
        
            

    }
}

