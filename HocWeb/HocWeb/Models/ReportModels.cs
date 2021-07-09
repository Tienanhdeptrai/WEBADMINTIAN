using System.ComponentModel.DataAnnotations;

namespace HocWeb.Models
{
    public class ReportModels
    {
        public string CreatedDate { get; set; }

        public string FeedBackID { get; set; }

        public string Status { get; set; }

        public string ReportName { get; set; }

        public string StoreID { get; set; }

        public string StoreName { get; set; }

        public string UserID { get; set; }
    }
}