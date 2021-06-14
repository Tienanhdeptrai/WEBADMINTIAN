using System;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using HocWeb.Models;
using HocWeb.DAO;


namespace HocWeb.Areas.Admin.Controllers
{
    public class CateProductController : BaseController
    {
        // GET: Admin/CateProduct
        public ActionResult Index()
        {
            var dao = new CateProductDao();
            var model = dao.ListAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(string ID)
        {
            var category = new CateProductDao().ViewDetail(ID);
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
            var result = new CateProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        public ActionResult Create(CateProductModels model)
        {
            var dao = new CateProductDao();
            var result = dao.CheckProduct(model.Name);
          
            if (ModelState.IsValid)
            {
                if(result==false)
                {
                    var session = (UserSession)Session[CommomConstants.USER_SESSION];
                    model.CreatedDate = DateTime.Now.ToString();
                    model.ModifiedDate = DateTime.Now.ToString();
                    model.ModifiedBy = session.TenTK;
                    model.CreatedBy = session.TenTK;
                    var id = new CateProductDao().Insert(model);
                    if (id ==true)
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
                    SetAlert("Tên danh mục đã tồn tại", "error");
                }
               
                
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CateProductModels category)
        {
            if (ModelState.IsValid)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];

                var dao = new CateProductDao();
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
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            new CateProductDao().Delete(id);
            SetAlert("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
    }
}