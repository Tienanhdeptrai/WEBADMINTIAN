using System.ComponentModel.DataAnnotations;
namespace HocWeb.Models
{

    public class ContactModels
    {
        public string ContactID { get; set; }
        public string Content { get; set; }
        public bool Status { get; set; }
    }
}