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
    public class BrandDAO
    {
        private FirebaseContext dBContext;
        private List<BrandModels> Collection = new List<BrandModels>();
        private List<ProductModels> ProductCollection = new List<ProductModels>();
        public BrandDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Brands");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<BrandModels>(((JProperty)item).Value.ToString()));
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
        public List<BrandModels> ListAll()
        {
            return Collection;
        }
        public bool ChangeStatus(string id)
        {
            var model = new BrandModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.BrandID == id);
            if (model.Status == "true")
            {
                model.Status = "false";
            }
            else
            {
                model.Status = "true";
            }
            FirebaseResponse response = dBContext.Client.Update("Brands/" + id, model);
            BrandModels data = JsonConvert.DeserializeObject<BrandModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
        public bool Insert(BrandModels models)
        {
            try
            {
                var data = models;
                PushResponse response = dBContext.Client.Push("Brands/", data);
                data.BrandID = response.Result.name;
                SetResponse setResponse = dBContext.Client.Set("Brands/" + data.BrandID, data);
            }
            catch { }
            if (CheckProduct(models.Name) == true)
            {
                return false;
            }
            return true;
        }
        public BrandModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Brands/" + id);
            BrandModels data = JsonConvert.DeserializeObject<BrandModels>(response.Body);
            return data;
        }
        public bool update(BrandModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Brands/" + models.BrandID, models);
                BrandModels data = JsonConvert.DeserializeObject<BrandModels>(response.Body);
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
                    FirebaseResponse response = dBContext.Client.Delete("Brands/" + id);
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
        public bool HasUseBrand(string brandid)
        {
            return ProductCollection.AsQueryable().Count(x => x.BrandID == brandid) > 0;
        }
    }
}