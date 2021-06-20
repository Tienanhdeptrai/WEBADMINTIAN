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
 
    public class ProductUserDao
    {
        private FirebaseContext dBContext;
        private List<ProductUserModels> ProductCollection = new List<ProductUserModels>();
        public ProductUserDao()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response1 = dBContext.Client.Get("ProductUser");
                dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
                foreach (var item in data1)
                {
                    ProductCollection.Add(JsonConvert.DeserializeObject<ProductUserModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }
        }
        public List<ProductUserModels> ListAll()
        {
            List<ProductUserModels> model = ProductCollection.AsQueryable<ProductUserModels>().ToList();
            return model;
        }
        public ProductUserModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("ProductUser/" + id);
            ProductUserModels data = JsonConvert.DeserializeObject<ProductUserModels>(response.Body);
            return data;
        }
        public bool update(ProductUserModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("ProductUser/" + models.ProductID, models);
                ProductUserModels data = JsonConvert.DeserializeObject<ProductUserModels>(response.Body);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}