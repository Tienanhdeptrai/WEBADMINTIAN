using System;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using HocWeb.Models;
using HocWeb.DAO;
using System.Web;
using System.IO;
using Firebase.Auth;
using System.Threading;
using Firebase.Storage;
using System.Threading.Tasks;

namespace HocWeb.Areas.Admin.Controllers
{
    public class BrandController : BaseController
    {
        // GET: Admin/Brand
        private static string APIKEY = "AIzaSyDSWIekvpvwQbRiGh4WF88H91tqFzL6OWI";
        private static string Bucket = "doan-d2374.appspot.com";
        private static string AuthEmail = "nguyentienanh1999@gmail.com";
        private static string AuthPass = "123456";
        public ActionResult Index()
        {
            var dao = new BrandDAO();
            var model = dao.ListAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(string ID)
        {
            var category = new BrandDAO().ViewDetail(ID);
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
            var result = new BrandDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BrandModels model, HttpPostedFileBase file)
        {
            var dao = new BrandDAO();
            var result = dao.CheckProduct(model.Name);
            if (result == false)
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
                model.Image = link;
                model.CreatedDate = DateTime.Now.ToString();
                model.ModifiedDate = DateTime.Now.ToString();
                model.ModifiedBy = session.TenTK;
                model.CreatedBy = session.TenTK;
                var id = new BrandDAO().Insert(model);
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
                SetAlert("Tên danh mục đã tồn tại", "error");
            }


            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BrandModels category, HttpPostedFileBase file)
        {
            
            if (ModelState.IsValid)
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
                var dao = new BrandDAO();
                category.ModifiedDate = DateTime.Now.ToString();
                category.ModifiedBy = session.TenTK;
                category.Image = link;
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
            new BrandDAO().Delete(id);
            SetAlert("Xóa thành công", "success");
            return RedirectToAction("Index");
        }
    }
}