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
    public class ShipperDAO
    {
        private FirebaseContext dBContext;
        private List<ShipperModels> Collection = new List<ShipperModels>();
        public ShipperDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Shippers");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<ShipperModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }

        }
        public bool CheckUserName(string username)
        {
            return Collection.AsQueryable().Count(x => x.UserName == username) > 0;
        }
        public bool CheckUserEmail(string email)
        {
            return Collection.AsQueryable().Count(x => x.Email == email) > 0;
        }

        public List<ShipperModels> GetAll()
        {
            return Collection;
        }
        public ShipperModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Shippers/" + id);
            ShipperModels data = JsonConvert.DeserializeObject<ShipperModels>(response.Body);
            return data;
        }
        public bool Insert(ShipperModels models)
        {
            try
            {
                var data = models;
                SetResponse setResponse = dBContext.Client.Set("Shippers/" + data.ShipperID, data);
            }
            catch { }
            if (CheckUserName(models.UserName) == true)
            {
                return false;
            }
            return true;
        }
        public bool Update(ShipperModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Shippers/" + models.ShipperID, models);
                ShipperModels data = JsonConvert.DeserializeObject<ShipperModels>(response.Body);
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
                FirebaseResponse response = dBContext.Client.Delete("Shippers/" + id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatus(string id)
        {
            var model = new ShipperModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.ShipperID == id);
            if (model.Status == "true")
            {
                model.Status = "false";
            }
            else
            {
                model.Status = "true";
            }
            FirebaseResponse response = dBContext.Client.Update("Shippers/" + id, model);
            ShipperModels data = JsonConvert.DeserializeObject<ShipperModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
    }
}