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
        [HttpGet]
        public ActionResult Detail(string id)
        {
            var dao = new FeedBackDAO();
            var result = dao.ViewDetail(id);
            return View(result);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Detail(FeedBackModels category)
        {
            if (ModelState.IsValid)
            {
                var dao = new FeedBackDAO();
                var result = dao.update(category);
                if (result)
                {
                    SetAlert("Sửa kho thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Sửa kho thành công", "error");
                }
            }
            return View(category);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FeedBackModels model)
        {
            if (ModelState.IsValid)
            {

                    var id = new FeedBackDAO().Insert(model);
                    if (id == true)
                    {
                        SetAlert("Thêm danh mục thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Thêm danh mục thất bại", "error");
                    }



            }
            return View(model);
        }
    }
}