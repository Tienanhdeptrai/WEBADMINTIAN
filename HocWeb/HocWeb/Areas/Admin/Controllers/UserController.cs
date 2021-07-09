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
    public class UserController : BaseController
    {
        private static string APIKEY = "AIzaSyDSWIekvpvwQbRiGh4WF88H91tqFzL6OWI";
        private static string Bucket = "doan-d2374.appspot.com";
        private static string AuthEmail = "nguyentienanh1999@gmail.com";
        private static string AuthPass = "123456";
        // GET: Admin/User
        public ActionResult Index()
        {
            var dao = new USERDAO();
            var model = dao.GetAll();
            return View(model);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = new USERDAO().ViewDetail(id);
            if(user.Merchant == true || new SellserDAO().CheckSellerStatus1(user.UserID) == true)
            {   
                IList<AddressModels> address = new SellserDAO().GetListAddress(id);
                IList<ProductUserModels> product = new ProductUserDao().GetProductByUser(user.UserID);
                ViewData["SANPHAM"] = product;
                ViewData["Seller"] = new SellserDAO().ViewDetail(id);
                ViewData["DIACHI"] = address;
                ViewData["ORDERS"] = new OrderDao().GetByCustomer(id);
            }
            return View(user);
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Create(UserModels user, HttpPostedFileBase file)
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
                    catch{}

                }
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var dao = new USERDAO();
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
                    if(user.Passwords=="")
                    {
                        user.Passwords = "123456";
                    }      
                    var ab = await auth.CreateUserWithEmailAndPasswordAsync(user.Email, user.Passwords);
                    string token = ab.FirebaseToken;
                    var infouser = ab.User;
                    user.UserID = infouser.LocalId;
                    user.CreatedDate = DateTime.Now.ToString();
                    user.Avatar = link;
                    user.CreatedBy = "Admin";
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
            
            return View(user);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(UserModels user, HttpPostedFileBase file)
        {
            
          
                var link = "https://i.ibb.co/S6QZ2N4/web-hi-res-512.png";
                FileStream stream;
                if (file  !=null)
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
                catch { }
                }
                var session = (UserSession)Session[CommomConstants.USER_SESSION];
                var dao = new USERDAO();
                user.ModifiedBy = session.TenTK;
                if(user.Avatar == "")
                {
                    user.Avatar = link;
                }
                user.ModifiedDate = DateTime.Now.ToString(); 
                
                var result = dao.Update(user);
                if (result)
                {   
                    if(user.Merchant== false && new SellserDAO().CheckSeller(user.UserID))
                    {
                        new ProductUserDao().ChangeStatusAllProductSeller(user.UserID);
                        new SellserDAO().ChangeStatusById(user.UserID);
                    }
                    if(user.Merchant == true && new SellserDAO().CheckSellerStatus1(user.UserID) == true)
                    {
                        new ProductUserDao().ChangeStatusAllProductSeller(user.UserID);
                          new SellserDAO().ChangeStatusById(user.UserID);
                     }
                    SetAlert("Chỉnh sửa thành công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    SetAlert("Cập nhật tài khoản không thành công", "error");
                    return View();
                }
           
        }
        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new USERDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}