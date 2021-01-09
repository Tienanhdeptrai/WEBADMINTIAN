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
    public class CateProductDao
    {
        private FirebaseContext dBContext;
        private List<CateProductModels> Collection = new List<CateProductModels>();
        private List<ProductModels> ProductCollection = new List<ProductModels>();
        public CateProductDao()
        {
            try {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Catogorys");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<CateProductModels>(((JProperty)item).Value.ToString()));
                }
                FirebaseResponse response1 = dBContext.Client.Get("Products");
                dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
                foreach (var item in data1)
                {
                    ProductCollection.Add(JsonConvert.DeserializeObject<ProductModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }
        }

        public List<CateProductModels> ListAll()
        {
            return Collection;
        }
        public bool ChangeStatus(string id)
        {
            var model = new CateProductModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.CateProductID == id);
            if (model.Status == "true")
            {
                model.Status = "false";
            }
            else
            {
                model.Status = "true";
            }
            FirebaseResponse response = dBContext.Client.Update("Catogorys/" + id, model);
            CateProductModels data = JsonConvert.DeserializeObject<CateProductModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
        public bool Insert(CateProductModels models)
        {
            try
            {
                var data = models;
                PushResponse response = dBContext.Client.Push("Catogorys/", data);
                data.CateProductID = response.Result.name;
                SetResponse setResponse = dBContext.Client.Set("Catogorys/" + data.CateProductID, data);
            }
            catch { }
            if (CheckProduct(models.Name) == true)
            {
                return false;
            }
            return true;
        }
        public CateProductModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Catogorys/" + id);
            CateProductModels data = JsonConvert.DeserializeObject<CateProductModels>(response.Body);
            return data;
        }
        public bool update(CateProductModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Catogorys/" + models.CateProductID, models);
                CateProductModels data = JsonConvert.DeserializeObject<CateProductModels>(response.Body);
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
                if (HasUseBrand(id))
                {
                    return false;
                }
                else
                {
                    FirebaseResponse response = dBContext.Client.Delete("Catogorys/" + id);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool CheckProduct(string name)
        {
            return Collection.AsQueryable().Count(x => x.Name == name) > 0;
        }
        public bool HasUseBrand(string cateid)
        {
            return ProductCollection.AsQueryable().Count(x => x.CategoryID == cateid) > 0;
        }
    }
}
