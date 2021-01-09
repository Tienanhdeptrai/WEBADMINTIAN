
using System.Web;
namespace HocWeb.Areas.Admin.Code
{
    public class SessionHeper
    {
        public static void SetSession(UserSession session)
        {
            HttpContext.Current.Session["LoginSession"] = session;
        }

        public static UserSession GetSession()
        {
            var session = HttpContext.Current.Session["LoginSession"];
            if (session == null)
                return null;
            else
            {
                return session as UserSession;
            }
        }
    }
}