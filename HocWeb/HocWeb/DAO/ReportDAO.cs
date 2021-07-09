
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
    public class ReportDAO
    {
        private FirebaseContext dBContext;
        private List<ReportModels> Collection = new List<ReportModels>();
        public ReportDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("FeedBacks");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<ReportModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }

        }
        public bool update(ReportModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("FeedBacks/" + models.FeedBackID, models);
                ReportModels data = JsonConvert.DeserializeObject<ReportModels>(response.Body);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public ReportModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("FeedBacks/" + id);
            ReportModels data = JsonConvert.DeserializeObject<ReportModels>(response.Body);
            return data;
        }
        public ReportDetailModels GetListReport(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("FeedBacks/" + id + "/Content");
            ReportDetailModels data = JsonConvert.DeserializeObject<ReportDetailModels>(response.Body);
            return data;
        }
        public List<ReportModels> ListAll()
        {
            return Collection;
        }
        public bool ChangeStatus(string id)
        {
            var model = new ReportModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.FeedBackID == id);
            model.Status = "False";
            FirebaseResponse response = dBContext.Client.Update("FeedBacks/" + id, model);
            ReportModels data = JsonConvert.DeserializeObject<ReportModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
    }
}