using System.ComponentModel.DataAnnotations;

namespace HocWeb.Models
{
    public class OrderModels
    {
        public string OrderID { get; set; }

        public string CreatedDate { get; set; }

        public string CustomerID { get; set; }

        public string ShipName { get; set; }

        public string ShipMoblie { get; set; }

        public string ShipAddress { get; set; }

        public string ShipEmail { get; set; }

        public double ShipPayment { get; set; }
        public string Payment { get; set; }

        public string Status { get; set; }

        public string ShipperID { get; set; }
        public string Total { get; set; }

        public string ShipLocation { get; set; }

        public string WarehouseId { get; set; }
        public string ReasonCanelOrder { get; set; }

        //refund
        public double RefundMonney { get; set; }

        public bool isRefund { get; set; }

        public string apptransid { get; set; }
    }
}