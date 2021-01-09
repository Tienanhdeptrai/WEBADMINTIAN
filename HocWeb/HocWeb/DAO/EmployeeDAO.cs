using System;
using System.Collections.Generic;
using System.Linq;
using HocWeb.App_Start;
using HocWeb.Models;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HocWeb.DAO
{
    public class EmployeeDAO
    {
        private FirebaseContext dBContext;
        private List<EmployeeModels> Collection = new List<EmployeeModels>();
       

        public EmployeeDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Employees");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<EmployeeModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }

        }
        public EmployeeModels GetByID(string userName)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.UserName == userName);
        }
        public int Login(string userName, string password)
        {

            var result = Collection.AsQueryable().SingleOrDefault(x => x.UserName == userName);

            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == "false")
                {
                    return -1;
                }
                else
                {
                    if (result.Passwords == password)
                    {
                        return 1;
                    }
                    else
                        return -2;
                }
            }

        }

        public EmployeeModels GetByEmail(string Email)
        {
            return Collection.AsQueryable().SingleOrDefault(x => x.Email == Email);
        }

        public bool CheckUserName(string username)
        {
            return Collection.Count(x => x.UserName == username) > 0;
        }
        public bool CheckUserEmail(string Email)
        {
            return Collection.Count(x => x.Email == Email) > 0;
        }
              
        public List<EmployeeModels> GetAll()
        {
            return Collection;
        }
        public EmployeeModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Employees/" + id);
            EmployeeModels data = JsonConvert.DeserializeObject<EmployeeModels>(response.Body);
            return data;
        }
      
        public bool Insert(EmployeeModels models)
        {
            try
            {
                var data = models;
                PushResponse response = dBContext.Client.Push("Employees/", data);
                data.EmployeeID = response.Result.name;
                SetResponse setResponse = dBContext.Client.Set("Employees/" + data.EmployeeID, data);

            }
            catch { }
            if (CheckUserName(models.UserName) == true)
            {
                return false;
            }
            return true;
        }
        public bool Update(EmployeeModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Employees/" + models.EmployeeID, models);
                EmployeeModels data = JsonConvert.DeserializeObject<EmployeeModels>(response.Body);
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
                FirebaseResponse response = dBContext.Client.Delete("Employees/" + id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatus(string id)
        {
            var model = new EmployeeModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.EmployeeID == id);
            if(model.Status=="true"){
                model.Status = "false";
            }
            else { 
                model.Status = "true";
            }
            FirebaseResponse response = dBContext.Client.Update("Employees/" + id, model);
            EmployeeModels data = JsonConvert.DeserializeObject<EmployeeModels>(response.Body);
            return Convert.ToBoolean(model.Status);      
        }
    }
}