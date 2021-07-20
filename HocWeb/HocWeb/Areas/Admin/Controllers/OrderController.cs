using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using HocWeb.DAO;
using HocWeb.Models;

namespace HocWeb.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        // GET: Admin/Order
        [HttpGet]
        public ActionResult Detail(string id)
        {
            var result = new OrderDao().ViewDetail(id);
            var dao = new USERDAO().ViewDetail(result.CustomerID);
            IList<OrderDetailModels> product = new OrderDao().GetAll(id);
            IList<UserModels> user = new List<UserModels>();
            user.Add(dao);
            SetViewBag();
            ViewData["KHACHHANG"] = user;
            ViewData["SANPHAM"] = product;
            return View(result);
        }
        public ActionResult Index()
        {
            var dao = new OrderDao();
            var model = dao.ListAllPaging();
            return View(model);
        }
        public ActionResult IndexSeller()
        {
            var dao = new OrderDao();
            var session = (UserSession)Session[CommomConstants.USER_SESSION];
            var model = dao.GetListOrderbySeller(session.UserID);
            return View(model);
        }
        public ActionResult IndexRefund()
        {
            var dao = new OrderDao();
            var model = dao.GetListRefund();
            IList<OrderModels> seller = new OrderDao().GetListRejectBySeller();
            ViewData["REJECTSELLER"] = seller;
            return View(model);
        }
        [HttpGet]
        public ActionResult DetailRefund(string id)
        {
            var result = new OrderDao().ViewDetailRefund(id);
            var dao = new USERDAO().ViewDetail(result.CustomerID);
            IList<OrderDetailModels> product = new OrderDao().GetAllRefund(id);
            IList<UserModels> user = new List<UserModels>();
            user.Add(dao);
            ViewData["KHACHHANG"] = user;
            ViewData["SANPHAM"] = product;
            return View(result);
        }
        [HttpPost]
        public ActionResult DetailRefund(OrderModels orders)
        {
            if (ModelState.IsValid)
            {
                var result = new OrderDao().ViewDetailRefund(orders.OrderID);
                var dao = new USERDAO().ViewDetail(result.CustomerID);
                IList<OrderDetailModels> product = new OrderDao().GetAllRefund(orders.OrderID);
                IList<UserModels> user = new List<UserModels>();
                user.Add(dao);
                ViewData["KHACHHANG"] = user;
                ViewData["SANPHAM"] = product;
                var order = new OrderDao();      
                order.updateRefund(orders);
                SetAlert("Chỉnh sửa trạng thái thành công", "success");
                RedirectToAction("IndexRefund");
            }
            return View(orders);
        }
        [HttpGet]
        public ActionResult DetailRejectSeller(string id)
        {
            var result = new OrderDao().ViewDetailSellerReject(id);
            var dao = new USERDAO().ViewDetail(result.CustomerID);
            IList<OrderDetailModels> product = new OrderDao().GetAllRejectSeller(id);
            IList<UserModels> user = new List<UserModels>();
            user.Add(dao);
            ViewData["KHACHHANG"] = user;
            ViewData["SANPHAM"] = product;
            return View(result);
        }
        [HttpPost]
        public ActionResult DetailRejectSeller(OrderModels orders)
        {
            if (ModelState.IsValid)
            {
                var result = new OrderDao().ViewDetailSellerReject(orders.OrderID);
                var dao = new USERDAO().ViewDetail(result.CustomerID);
                IList<OrderDetailModels> product = new OrderDao().GetAllRejectSeller(orders.OrderID);
                IList<UserModels> user = new List<UserModels>();
                user.Add(dao);
                ViewData["KHACHHANG"] = user;
                ViewData["SANPHAM"] = product;
                var order = new OrderDao();
                order.updateRefund(orders);
                SetAlert("Chỉnh sửa trạng thái thành công", "success");
                RedirectToAction("IndexRefund");
            }
            return View(orders);
        }
        [HttpPost]
        public ActionResult Detail(OrderModels orders)
        {
            if (ModelState.IsValid)
            {
                SetViewBag();
                var result = new OrderDao().ViewDetail(orders.OrderID);
                var dao = new USERDAO().ViewDetail(result.CustomerID);
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                IList<OrderDetailModels> product = new OrderDao().GetAll(orders.OrderID);
                IList<UserModels> user = new List<UserModels>();
                user.Add(dao);
                ViewData["KHACHHANG"] = user;
                ViewData["SANPHAM"] = product;
                var order = new OrderDao();
                order.update(orders, session.UserID);
                SetAlert("Chỉnh sửa trạng thái thành công", "success");
                if(session.ChucVu == "Seller")
                {
                    RedirectToAction("IndexSeller");
                }
                else
                {
                    RedirectToAction("Index");
                }                          
            }           
            return View(orders);
        }
        public void SetViewBag(long? selectedID = null)
        {
            var dao = new FeedBackDAO();
            ViewBag.WarehouseId = new SelectList(dao.ListAll(), "WarehouseId", "name", selectedID);
        }
    }
}