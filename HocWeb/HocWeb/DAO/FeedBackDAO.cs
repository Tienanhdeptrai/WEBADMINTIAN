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
                FirebaseResponse response = dBContext.Client.Get("FeedBacks");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<FeedBackModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }

        }
        public FeedBackModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("FeedBacks/" + id);
            FeedBackModels data = JsonConvert.DeserializeObject<FeedBackModels>(response.Body);
            return data;
        }
        public int Counts()
        {
            return Collection.AsQueryable().Count(x => x.Status == "true");
        }
        public List<FeedBackModels> ListAll()
        {
            return Collection;
        }
        public bool ChangeStatus(string id)
        {
            var model = new FeedBackModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.FeedBackID == id);
            if (model.Status == "true")
            {
                model.Status = "false";
            }
            else
            {
                model.Status = "false";
            }
            FirebaseResponse response = dBContext.Client.Update("FeedBacks/" + id, model);
            FeedBackModels data = JsonConvert.DeserializeObject<FeedBackModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
    }
}