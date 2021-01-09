
using Newtonsoft.Json;

namespace HocWeb.Models
{
    public class EmployeeModels
    {
        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("EmployeeID")]
        public string EmployeeID { get; set; }

        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [JsonProperty("Passwords")]
        public string Passwords { get; set; }

        [JsonProperty("Avatar")]
        public string Avatar { get; set; }

        [JsonProperty("FirstName")]
        public string FirstName { get; set; }

        [JsonProperty("LastName")]
        public string LastName { get; set; }

        [JsonProperty("Position")]
        public string Position { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Phone")]
        public string Phone { get; set; }

        [JsonProperty("CMND")]
        public string CMND { get; set; }       

        [JsonProperty("CreatedDate")]
        public string CreatedDate { get; set; }

        [JsonProperty("CreatedBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("ModifiedDate")]
        public string ModifiedDate { get; set; }

        [JsonProperty("ModifiedBy")]
        public string ModifiedBy { get; set; }

        [JsonProperty("Status")]
        public string Status { get; set; }
    }
}