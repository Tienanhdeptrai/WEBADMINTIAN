using System;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using HocWeb.Models;
using HocWeb.DAO;
using System.Collections.Generic;

namespace HocWeb.Areas.Admin.Controllers
{
    public class ReportController : BaseController
    {
        // GET: Admin/Report
        public ActionResult Index()
        {
            var dao = new ReportDAO();
            var model = dao.ListAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult Detail(string id)
        {
            var dao = new ReportDAO();
            var result = dao.ViewDetail(id);
            ViewData["REPORTS"] = dao.GetListReport(id);
            dao.ChangeStatus(id);
            return View(result);
        }
    }
}