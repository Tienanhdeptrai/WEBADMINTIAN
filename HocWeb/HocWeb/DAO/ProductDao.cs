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

    public class ProductDao
    {
        private FirebaseContext dBContext;
        private List<ProductModels> ProductCollection = new List<ProductModels>();
        private List<BrandModels> BrandCollection = new List<BrandModels>();
        private List<CateProductModels> CateCollection = new List<CateProductModels>();

        public ProductDao()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response1 = dBContext.Client.Get("Products");
                dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
                foreach (var item in data1)
                {
                    ProductCollection.Add(JsonConvert.DeserializeObject<ProductModels>(((JProperty)item).Value.ToString()));
                }

                FirebaseResponse response2 = dBContext.Client.Get("Brands");
                dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
                foreach (var item in data2)
                {
                    BrandCollection.Add(JsonConvert.DeserializeObject<BrandModels>(((JProperty)item).Value.ToString()));
                }

                FirebaseResponse response3 = dBContext.Client.Get("Catogorys");
                dynamic data3 = JsonConvert.DeserializeObject<dynamic>(response3.Body);
                foreach (var item in data3)
                {
                    CateCollection.Add(JsonConvert.DeserializeObject<CateProductModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }
        }

        public int CountProduct()
        {
            return ProductCollection.AsQueryable().Count(x => x.Status=="True");
        }
        public ProductModels GetByID(string ID)
        {
            return ProductCollection.AsQueryable().SingleOrDefault(x => x.ProductID == ID);
        }
        public List<ProductModels> ListAllProduct()
        {
            List<ProductModels> model = ProductCollection.AsQueryable().ToList();
            return model;
        }       
        public List<ProductModels> ListAllPaging()
        {
            List<ProductModels> model = ProductCollection.AsQueryable<ProductModels>().ToList();
            return model;
        }
        public ProductModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Products/" + id);
            ProductModels data = JsonConvert.DeserializeObject<ProductModels>(response.Body);
            return data;
        }
        public bool update(ProductModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Products/" + models.ProductID, models);
                ProductModels data = JsonConvert.DeserializeObject<ProductModels>(response.Body);
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
                FirebaseResponse response = dBContext.Client.Delete("Products/" + id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatus(string id)
        {
            var model = new ProductModels();
            model = ProductCollection.AsQueryable().SingleOrDefault(x => x.ProductID == id);
            if (model.Status == "true")
            {
                model.Status = "false";
            }
            else
            {
                model.Status = "true";
            }
            FirebaseResponse response = dBContext.Client.Update("Products/" + id, model);
            ProductModels data = JsonConvert.DeserializeObject<ProductModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
        public bool CheckProduct(string name)
        {
            return ProductCollection.AsQueryable<ProductModels>().Count(x => x.Name == name) > 0;
        }
        public bool Insert(ProductModels models)
        {
            try
            {
                var data = models;
                PushResponse response = dBContext.Client.Push("Products/", data);
                data.ProductID = response.Result.name;
                SetResponse setResponse = dBContext.Client.Set("Products/" + data.ProductID, data);
            }
            catch { }
            if (CheckProduct(models.Name) == true)
            {
                return false;
            }
            return true;
        }
        public List<BrandModels> ListAll()
        {
            return BrandCollection;
        }
        public List<ProductModels> GetListProduct()
        {
            List<ProductModels> model = ProductCollection.AsQueryable<ProductModels>().ToList();
            return model;
        }
        public string GetProductName(string id)
        {
            try
            {
                var models = ProductCollection.AsQueryable<ProductModels>().SingleOrDefault(x => x.ProductID == id);
                return models.Name;
            }
            catch
            {
                return "Tên sản phẩm";
            }
          
        }
        public string GetBrandNameByID(string id)
        {
            try
            {
                var brand_models = BrandCollection.AsQueryable<BrandModels>().SingleOrDefault(x => x.BrandID == id);
                return brand_models.Name;
            }
            catch
            {
                return "Tên nhãn";
            }
          
        }
        public string GetCateNameByID(string id)
        {
            var cate_models = CateCollection.AsQueryable<CateProductModels>().SingleOrDefault(x => x.CateProductID == id);
            return cate_models.Name;
        }
        public string GetBrand_Name(ProductModels models)
        {
            try
            {
                var brand_models = BrandCollection.AsQueryable<BrandModels>().SingleOrDefault(x => x.BrandID == models.BrandID);
                return brand_models.Name;
            }
            catch
            {
                return "";
            }
      
        }
        public string GetCate_Name(ProductModels models)
        {
            try
            {
                var cate_models = CateCollection.AsQueryable<CateProductModels>().SingleOrDefault(x => x.CateProductID == models.CategoryID);
                return cate_models.Name;
            }
            catch
            {
                return "";
            }
            
        }
    }
}
