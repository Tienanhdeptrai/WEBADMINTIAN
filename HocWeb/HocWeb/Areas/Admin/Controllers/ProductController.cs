using System;
using System.Collections.Generic;
using System.Linq;
using HocWeb.DAO;
using HocWeb.Models;
using HocWeb.Areas.Admin.Code;
using System.Web.Mvc;
using System.Web;
using System.IO;
using Firebase.Auth;
using System.Threading;
using Firebase.Storage;
using System.Threading.Tasks;


namespace HocWeb.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        private static string APIKEY = "AIzaSyDSWIekvpvwQbRiGh4WF88H91tqFzL6OWI";
        private static string Bucket = "doan-d2374.appspot.com";
        private static string AuthEmail = "nguyentienanh1999@gmail.com";
        private static string AuthPass = "123456";
        public ActionResult Index()
        {
            var dao = new ProductDao();
            var model = dao.ListAllPaging();
            return View(model);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            SetViewBag1();
            return View();
        }
        [HttpGet]
        public ActionResult Edit(string ID)
        {
            var product = new ProductDao().ViewDetail(ID);
            SetViewBag1();
            SetViewBag();
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Create(ProductModels product, HttpPostedFileBase file)
        {
            
           
                var link = "https://i.ibb.co/S6QZ2N4/web-hi-res-512.png";
                FileStream stream;
                if (file.ContentLength > 0)
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKEY));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPass);
                    var cancellation = new CancellationTokenSource();
                    string path = Path.Combine(Server.MapPath("~/Content/images/"), file.FileName);
                    file.SaveAs(path);
                    stream = new FileStream(Path.Combine(path), FileMode.Open);

                    var task = new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true
                        })
                        .Child("images")
                        .Child(file.FileName)
                        .PutAsync(stream, cancellation.Token);
                    try
                    {
                        link = await task;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Cannot upload file");
                    }

                }
                var dao = new ProductDao();
                var result = dao.CheckProduct(product.Name);
                if (result ==false)
                {                 
                    var session = (UserSession)Session[CommomConstants.USER_SESSION];
                    var data = new ProductDao();
                    product.Image = link;
                    product.CreatedDate = DateTime.Now.ToString();
                    product.CreatedBy = session.TenTK;
                    product.ModifiedDate = DateTime.Now.ToString();
                   product.ModifiedBy = session.TenTK;
                    var id=data.Insert(product);
                    if (id ==true)
                    {
                        SetAlert("Thêm sản phẩm thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Thêm sản phẩm thất bại", "error");
                    }                   
                }
                else
                {
                    SetAlert("Sản phẩm đã tồn tại", "error");
                }
            
            SetViewBag();
            SetViewBag1();
            return View(product);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ProductModels product, HttpPostedFileBase file)
        {
            
           
                var link = "https://i.ibb.co/S6QZ2N4/web-hi-res-512.png";
                FileStream stream;
                if (file.ContentLength > 0)
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKEY));
                    var a = await auth.SignInWithEmailAndPasswordAsync(AuthEmail, AuthPass);
                    var cancellation = new CancellationTokenSource();
                    string path = Path.Combine(Server.MapPath("~/Content/images/"), file.FileName);
                    file.SaveAs(path);
                    stream = new FileStream(Path.Combine(path), FileMode.Open);

                    var task = new FirebaseStorage(
                        Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                            ThrowOnCancel = true
                        })
                        .Child("images")
                        .Child(file.FileName)
                        .PutAsync(stream, cancellation.Token);
                    try
                    {
                        link = await task;
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine("Cannot upload file");
                    }

                }
                var session = (UserSession)Session[CommomConstants.USER_SESSION];

                var data = new ProductDao();
                product.Image = link;
                product.ModifiedDate = DateTime.Now.ToString();
                product.ModifiedBy = session.TenTK;
                var result = data.update(product);
                if (result==true)
                {
                   SetAlert("Sửa sản phẩm thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    SetAlert("Sửa không thành công", "error");
                }
            
            SetViewBag();
            SetViewBag1();
            return View(product);
        }
        public void SetViewBag(long? selectedID=null)
        {
            var dao = new CateProductDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "CateProductID", "Name",selectedID);
        }
        public void SetViewBag1(long? selectedID = null)
        {
            var dao = new ProductDao();
            ViewBag.BrandID = new SelectList(dao.ListAll(), "BrandID", "Name", selectedID);
        }
        public ActionResult Delete(string ID)
        {
            var oders = new OrderDao();
            bool result = oders.hasOrderDetail(ID);
            if (result == false)
            {
                SetAlert("Xoá thất bại !!!", "error");
            }
            else
            {
                new ProductDao().Delete(ID);
                SetAlert("Xóa thành công", "success");
            }                      
            return RedirectToAction("Index", "Product");
        }
        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new ProductDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        public ActionResult DashBoard()
        {
            var dao = new ProductDao();
            var list = dao.ListAllProduct();
            List<int> reparttitons = new List<int>();
            var posion = list.Select(x => x.Price).Distinct();
            foreach (var item in posion)
            {
                reparttitons.Add(list.Count(x => x.Price == item));
            }

            var rep = reparttitons;
            ViewBag.POSI = posion;
            ViewBag.REP = reparttitons.ToList();
            return View();
        }
        public ActionResult Detail(string id)
        {
            var dao = new ProductDao();
            var result = dao.ViewDetail(id);
            ViewBag.BrandName = dao.GetBrand_Name(result);
            ViewBag.CateProductName = dao.GetCate_Name(result);
            return View(result);
        }

    }
}