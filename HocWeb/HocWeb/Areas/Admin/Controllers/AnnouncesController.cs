using System;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using HocWeb.Models;
using HocWeb.DAO;


namespace HocWeb.Areas.Admin.Controllers
{
    public class AnnouncesController : BaseController
    {
        // GET: Admin/Announces
        public ActionResult Index()
        {
            var dao = new AnnounceDAO();
            var model = dao.ListAll();
            return View(model);
        }
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            new AnnounceDAO().Delete(id);
            SetAlert("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(string ID)
        {
            var category = new AnnounceDAO().ViewDetail(ID);
            return View(category);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new AnnounceDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public ActionResult Create(AnnounceModels model)
        {
            var dao = new AnnounceDAO();
            var result = dao.CheckProduct(model.Title);

            if (ModelState.IsValid)
            {
                if (result == false)
                {
                    var session = (UserSession)Session[CommomConstants.USER_SESSION];
                    model.CreatedDate = DateTime.Now.ToString();
                    model.ModifiedDate = DateTime.Now.ToString();
                    model.ModifiedBy = session.TenTK;
                    model.CreatedBy = session.TenTK;
                    var id = new AnnounceDAO().Insert(model);
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
                else
                {
                    SetAlert("Tên danh mục đã tồn tại", "warning");
                }
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(AnnounceModels category)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];

                var dao = new AnnounceDAO();
                category.ModifiedDate = DateTime.Now.ToString();
                category.ModifiedBy = session.TenTK;
                var result = dao.update(category);
                if (result)
                {
                    SetAlert("Sửa danh mục sản phẩm thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Sửa danh mục không thành công", "error");
                }
            }
            return View(category);
        }
        public ActionResult Detail(string id)
        {
            var result = new AnnounceDAO().ViewDetail(id);
            return View(result);
        }
    }
    
}