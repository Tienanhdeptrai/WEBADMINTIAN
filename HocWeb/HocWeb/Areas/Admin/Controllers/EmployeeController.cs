using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EmployeeController : BaseController
    {

        // GET: Admin/Employee
        private static string APIKEY = "AIzaSyDSWIekvpvwQbRiGh4WF88H91tqFzL6OWI";
        private static string Bucket = "doan-d2374.appspot.com";
        private static string AuthEmail = "nguyentienanh1999@gmail.com";
        private static string AuthPass = "123456";
        public ActionResult Index()
        {
            var dao = new EmployeeDAO();
            var model = dao.GetAll();
            return View(model);
        }  
       public ActionResult Logout()
        {
            Session[CommomConstants.USER_SESSION] = null;
            return Redirect("/Admin/Login/Index");
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        
         [HttpGet]
        public ActionResult Edit(string id)
        {
            var user = new EmployeeDAO().ViewDetail(id);     
            return View(user);
        }
        [HttpPost]       
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EmployeeModels user, HttpPostedFileBase file)
        {
            var link = "https://i.ibb.co/HDzz1rC/avartarnone.png";
        
                FileStream stream;
                if(file.ContentLength > 0)
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
                var dao = new EmployeeDAO();
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
                    user.CreatedDate = DateTime.Now.ToString(); ;
                    user.CreatedBy = "Admin";
                    user.ModifiedBy = session.TenTK;
                    user.Avatar = link;
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
        public async Task<ActionResult> Edit(EmployeeModels user, HttpPostedFileBase file)
        {
            var link = "https://i.ibb.co/HDzz1rC/avartarnone.png";
          
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
                var dao = new EmployeeDAO();
                user.ModifiedBy = session.TenTK;
                user.ModifiedDate = DateTime.Now.ToString();
                user.Avatar = link;
                var result = dao.Update(user);
                if (result)
                {
                    SetAlert("Chỉnh sửa thành công", "success");
                    return RedirectToAction("Index", "Employee");
                }
                else
                {
                    SetAlert("Cập nhật tài khoản không thành công", "error");
                        return View();
                }
            
           return View(); 
        }
        [HttpPost]
        public JsonResult ChangeStatus(string id)
        {
            var result = new EmployeeDAO().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }

        public ActionResult DashBoard()
        {
            var dao = new EmployeeDAO();
            var list = dao.GetAll();
            List<int> reparttitons = new List<int>();
            var posion = list.Select(x => x.Position).Distinct();
            foreach(var item in posion)
            {
                reparttitons.Add(list.Count(x => x.Position == item));
            }

            var rep = reparttitons;
            ViewBag.POSI = posion ;
            ViewBag.REP = reparttitons.ToList();
            return View();
        }
        
    }
}