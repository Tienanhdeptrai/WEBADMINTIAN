using System;
using System.Web.Mvc;
using HocWeb.Models;
using HocWeb.DAO;
using System.Collections.Generic;

namespace HocWeb.Areas.Admin.Controllers
{
    public class SellsersController : BaseController
    {
        // GET: Admin/Sellsers
        public ActionResult Index()
        {
            var dao = new SellserDAO();
            var model = dao.GetAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var seller = new SellserDAO().ViewDetail(id);
            var dao = new USERDAO().ViewDetail(id);
            IList<AddressModels> address = new SellserDAO().GetListAddress(id);
            IList <UserModels> user = new List<UserModels>();
            user.Add(dao);
            ViewData["KHACHHANG"] = user;
            ViewData["DIACHI"] = address;
            return View(seller);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SellserModels sellers)
        {
            if (ModelState.IsValid)
            {
                var dao = new SellserDAO();
        
                sellers.ModifyAt = DateTime.Now.ToString();
                var result = dao.Update(sellers);
                if (result)
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index");
                }
                else
                {
                    SetAlert("Cập nhật thất bại", "error");
                }
            }
            return View(sellers);
        }
    }
}