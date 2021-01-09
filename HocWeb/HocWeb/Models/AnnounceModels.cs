using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HocWeb.Models
{
    public class AnnounceModels
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public string Details { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public string Status { get; set; }

        public string Url { get; set; }
    }
}