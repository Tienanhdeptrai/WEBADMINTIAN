using System;
using System.Collections.Generic;
using System.Linq;
using HocWeb.App_Start;
using HocWeb.Models;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Firebase.Auth;

namespace HocWeb.DAO
{
    public class USERDAO
    { 
        private FirebaseContext dBContext;
        private List<UserModels> Collection= new List<UserModels>();

        public USERDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Users");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<UserModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }
        }
        public UserModels GetByID(string userName)
        {                     
            return (UserModels)Collection.Where(x => x.UserName == userName);
        }
        public UserModels GetByEmail(string Email)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.Email == Email);
        }

        public bool CheckUserName(string username)
        {
            return Collection.AsQueryable().Count(x => x.UserName == username) > 0;
        }
        public bool CheckUserEmail(string email)
        {
            return Collection.AsQueryable().Count(x => x.UserName == email) > 0;
        }
        public string GetCusName(string id)
        {
            var model = Collection.AsQueryable().SingleOrDefault(x => x.UserID == id);
            return model.FullName;
        }
        public string GetAvatar(string id)
        {
            var model = Collection.AsQueryable().SingleOrDefault(x => x.UserID == id);
            return model.Avatar;
        }

        public List<UserModels> GetAll()
        {
            return Collection;
        }
        public UserModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Users/" + id);
            UserModels data = JsonConvert.DeserializeObject<UserModels>(response.Body);
            return data;
        }
        public  bool  Insert(UserModels models)
        {
            try
            {
                var data = models;
                SetResponse setResponse = dBContext.Client.Set("Users/" + data.UserID, data);
     
            }
            catch { }      
            return true;
        }     
        public bool Update(UserModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Users/" + models.UserID, models );
                UserModels data = JsonConvert.DeserializeObject<UserModels>(response.Body);
                return true;
            }                 
            catch
            {
                return false;
            }
        }
        public bool Delete(string id)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Delete("Users/" + id);          
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatus(string  id)
        {
            var model = new UserModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.UserID == id);
            if (model.Status == "true")
            {
                model.Status = "false";
            }
            else
            {
                model.Status = "true";
            }
            FirebaseResponse response = dBContext.Client.Update("Users/" + id, model);
            UserModels data = JsonConvert.DeserializeObject<UserModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
     
    }
}
