using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;
using HocWeb.Models;
using HocWeb.DAO;
using Firebase.Auth;
using System.Web;
using System.IO;
using System.Threading;
using Firebase.Storage;
using System.Threading.Tasks;

namespace HocWeb.Areas.Admin.Controllers
{
    public class ShipperController : BaseController
    {
        private static string APIKEY = "AIzaSyDSWIekvpvwQbRiGh4WF88H91tqFzL6OWI";
        private static string Bucket = "doan-d2374.appspot.com";
        private static string AuthEmail = "nguyentienanh1999@gmail.com";
        private static string AuthPass = "123456";
        // GET: Admin/Shipper
        public ActionResult Index()
        {
            var dao = new ShipperDAO();
            var model = dao.GetAll();
            return View(model);
        }    
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = new ShipperDAO().ViewDetail(id);
            return View(user);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ShipperModels user, HttpPostedFileBase file)
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
                var dao = new ShipperDAO();
                if (dao.CheckUserName(user.UserName))
                {
                    SetAlert("Tên đăng nhập đã đăng kí", "error");
                }
                else if (dao.CheckUserEmail(user.Email))
                {
                    SetAlert("Tài khoản Email đã tồn tại", "error");
                }
                else
                {
                    var auth = new FirebaseAuthProvider(new FirebaseConfig(APIKEY));
                    if (user.Passwords == "")
                    {
                        user.Passwords = "123456";
                    }
                    var ab = await auth.CreateUserWithEmailAndPasswordAsync(user.Email, user.Passwords);
                    string token = ab.FirebaseToken;
                    var infouser = ab.User;
                    user.ShipperID = infouser.LocalId;
                    user.CreatedDate = DateTime.Now.ToString();
                    user.CreatedBy = "Admin";
                    user.Avatar = link;
                    user.ModifiedBy = session.TenTK;
                    bool id = dao.Insert(user);
                    if (id == true)
                    {
                        SetAlert("Thêm thành công", "success");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        SetAlert("Thêm không thành công", "error");
                    }
                }
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ShipperModels user, HttpPostedFileBase file)
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
                var dao = new ShipperDAO();
                user.Avatar = link;
                user.ModifiedBy = session.TenTK;
                user.ModifiedDate = DateTime.Now.ToString(); ;
                var result = dao.Update(user);
                if (result)
                {
                    SetAlert("Chỉnh sửa thành công", "success");
                    return RedirectToAction("Index", "Shipper");
                }
                else
                {
                    SetAlert("Cập nhật tài khoản không thành công", "error");
                    return View();
                }
            }
            return View();
        }
        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new ShipperDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}