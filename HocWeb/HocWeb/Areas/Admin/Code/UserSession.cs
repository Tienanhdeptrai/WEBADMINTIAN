using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HocWeb.Areas.Admin.Code
{
    [Serializable]
    public class UserSession
    {

        public string TenTK { get; set; }
        public string UserID { get; set; }

        public string Pass { get; set; }
        public string Name { get; set; }
        public string Avartar { get; set; }

        public string ChucVu { get; set; }
    }
}