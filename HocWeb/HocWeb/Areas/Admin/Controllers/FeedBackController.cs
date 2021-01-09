using System;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using HocWeb.Models;
using HocWeb.DAO;

namespace HocWeb.Areas.Admin.Controllers
{
    public class FeedBackController : BaseController
    {
        // GET: Admin/FeedBack
        public ActionResult Index()
        {
            var dao = new FeedBackDAO();
            var model = dao.ListAll();
            return View(model);
        }
        public ActionResult Detail(string id)
        {
            var dao = new FeedBackDAO();
            var result = dao.ViewDetail(id);
            dao.ChangeStatus(id);
            return View(result);
        }
    }
}