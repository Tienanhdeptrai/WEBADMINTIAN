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
            var data = new ProductUserDao();
            product.Image = link;
            var result = data.update(product);
            if (result == true)
            {
                SetAlert("Sửa sản phẩm thành công", "success");
                return RedirectToAction("Index", "Product");
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
    }
}