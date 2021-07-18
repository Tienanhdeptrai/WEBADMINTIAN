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

        public List<ProductUserModels> GetProductByUser(string userId)
        {
            List<ProductUserModels> model = new List<ProductUserModels>();
            foreach (var item in ProductCollection)
            {
                if (item.UserID == userId)
                    model.Add(item);
            }
            return model;
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
        public bool CheckProduct(string name)
        {
            return ProductCollection.AsQueryable<ProductUserModels>().Count(x => x.Name == name) > 0;
        }
        public bool Insert(ProductUserModels models)
        {
            try
            {
                var data = models;
                PushResponse response = dBContext.Client.Push("ProductUser/", data);
                data.ProductID = response.Result.name;
                SetResponse setResponse = dBContext.Client.Set("ProductUser/" + data.ProductID, data);
            }
            catch { }
            if (CheckProduct(models.Name) == true)
            {
                return false;
            }
            return true;
        }
        public void ChangeStatusAllProductSeller(string sellerId)
        {
            foreach (var item in ProductCollection)
            {
                if(item.UserID == sellerId)
                {
                    ChangeStatus(item.ProductID);
                }
            }
        }
        public void ChangeStatus(string id)
        {
            var model = new ProductUserModels();
            model = ProductCollection.AsQueryable().SingleOrDefault(x => x.ProductID == id);
            if (model.Status == true)
            {
                model.Status = false;
            }
            else
            {
                model.Status = true;
            }
            FirebaseResponse response = dBContext.Client.Update("ProductUser/" + id, model);
        }

        public string GetProductName(string id)
        {
            try
            {
                var models = ProductCollection.AsQueryable<ProductUserModels>().SingleOrDefault(x => x.ProductID == id);
                return models.Name;
            }
            catch
            {
                return "Tên sản phẩm";
            }

        }

        public string GetProductImage(string id)
        {
            try
            {
                var models = ProductCollection.AsQueryable<ProductUserModels>().SingleOrDefault(x => x.ProductID == id);
                return models.Image;
            }
            catch
            {
                return "Tên sản phẩm";
            }

        }
    }
}