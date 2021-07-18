using System.Web.Mvc;
using HocWeb.Areas.Admin.Models;
using HocWeb.DAO;
using HocWeb.Areas.Admin.Code;

namespace HocWeb.Areas.Admin.Controllers
{
    public class LoginSellerController : Controller
    {
        // GET: Admin/Login       
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new USERDAO();
                var result = dao.Login(model.TenTK, model.MatKhau);
                if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng");
                }
                else if (result == -3)
                {
                    ModelState.AddModelError("", "Bạn chưa phải là người bán hàng");
                }
                else if (result == 1)
                {
                    var user = dao.GetByID(model.TenTK);
                    var userSession = new UserSession();
                    userSession.TenTK = user.UserName;
                    userSession.Pass = user.Passwords;
                    userSession.UserID = user.UserID.ToString();
                    userSession.ChucVu = "Seller";
                    userSession.Avartar = user.Avatar;
                    userSession.Name = user.FullName;
                    Session.Add(CommomConstants.USER_SESSION, userSession);
                    return RedirectToAction("Index", "Default");
                }
            }
            return View("Index");
        }
    }
}