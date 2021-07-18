using System;
using System.Collections.Generic;
using System.Linq;
using HocWeb.DAO;
using HocWeb.Models;
using HocWeb.Areas.Admin.Code;
using System.Web.Mvc;
using System.Web;
using System.IO;
using HocWeb.Areas.Admin.Code;
using Firebase.Auth;
using System.Threading;
using Firebase.Storage;
using System.Threading.Tasks;

namespace HocWeb.Areas.Admin.Controllers
{
    public class ProductUserController : BaseController
    {
        // GET: Admin/ProductUser
        private static string APIKEY = "AIzaSyDSWIekvpvwQbRiGh4WF88H91tqFzL6OWI";
        private static string Bucket = "doan-d2374.appspot.com";
        private static string AuthEmail = "nguyentienanh1999@gmail.com";
        private static string AuthPass = "123456";
        public ActionResult Index()
        {
            var dao = new ProductUserDao();
            var model = dao.ListAll();
            return View(model);
        }
        public ActionResult IndexProduct()
        {
            var dao = new ProductUserDao();
            var session = (UserSession)Session[CommomConstants.USER_SESSION];
            var model = dao.GetProductByUser(session.UserID);
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(string ID)
        {
            var product = new ProductUserDao().ViewDetail(ID);
            SetViewBag();
            return View(product);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(ProductUserModels product, HttpPostedFileBase file)
        {
            FileStream stream;
            if (file != null)
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
                    product.Image = await task;
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Cannot upload file");
                }

            }
            var session = (UserSession)Session[CommomConstants.USER_SESSION];
            var data = new ProductUserDao();
            var result = data.update(product);
            if (result == true)
            {
                if(session.ChucVu == "Seller")
                {
                    SetAlert("Sửa sản phẩm thành công", "success");
                    return RedirectToAction("IndexProduct", "ProductUser");
                }
                else
                {
                    SetAlert("Sửa sản phẩm thành công", "success");
                    return RedirectToAction("Index", "ProductUser");
                }
       
            }
            else
            {
                SetAlert("Sửa không thành công", "error");
            }
            return View(product);
        }
        public void SetViewBag(long? selectedID = null)
        {
            var dao = new CateProductDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "CateProductID", "Name", selectedID);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Create(ProductUserModels product, HttpPostedFileBase file)
        {

            FileStream stream;
            if (file != null)
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
                    product.Image = await task;
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Cannot upload file");
                }

            }
            var dao = new ProductDao();
            var result = dao.CheckProduct(product.Name);
            if (result == false)
            {
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var data = new ProductUserDao();
                product.CreatedDate = DateTime.Now.ToString();
                product.Status = true;
                product.UserID = session.UserID;
                var id = data.Insert(product);
                if (id == true)
                {
                    SetAlert("Thêm sản phẩm thành công", "success");
                    return RedirectToAction("IndexProduct");
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
            return View(product);
        }
    }
}