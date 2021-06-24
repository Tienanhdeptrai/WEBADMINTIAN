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
    public class FeedBackDAO
    {
        private FirebaseContext dBContext;
        private List<FeedBackModels> Collection = new List<FeedBackModels>();
        public FeedBackDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Warehouse");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<FeedBackModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }

        }
        public bool Insert(FeedBackModels models)
        {
            try
            {
                var data = models;
                PushResponse response = dBContext.Client.Push("Warehouse/", data);
                data.WarehouseId = response.Result.name;
                SetResponse setResponse = dBContext.Client.Set("Warehouse/" + data.WarehouseId, data);
                return true;
            }
            catch {
                return false;
            }
     
        }
        public FeedBackModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Warehouse/" + id);
            FeedBackModels data = JsonConvert.DeserializeObject<FeedBackModels>(response.Body);
            return data;
        }
        public int Counts()
        {
            return Collection.AsQueryable().Count(x => x.status == true);
        }
        public List<FeedBackModels> ListAll()
        {
            return Collection;
        }
        public bool update(FeedBackModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Warehouse/" + models.WarehouseId, models);
                FeedBackModels data = JsonConvert.DeserializeObject<FeedBackModels>(response.Body);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ChangeStatus(string id)
        {
            var model = new FeedBackModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.WarehouseId == id);
            if (model.status == true)
            {
                model.status = false;
            }
            else
            {
                model.status = false;
            }
            FirebaseResponse response = dBContext.Client.Update("Warehouse/" + id, model);
            FeedBackModels data = JsonConvert.DeserializeObject<FeedBackModels>(response.Body);
            return Convert.ToBoolean(model.status);
        }
    }
}