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
    public class FmcTokenDAO
    {
        private FirebaseContext dBContext;
        private List<FmcTokenModels> Collection = new List<FmcTokenModels>();
        public FmcTokenDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response1 = dBContext.Client.Get("FmcToken");
                dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
                foreach (var item in data1)
                {
                    Collection.Add(JsonConvert.DeserializeObject<FmcTokenModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }
        }
        public List<FmcTokenModels> GetAll()
        {
            return Collection;
        }

        public List<FmcTokenModels> GetByUserId(string userId)
        {
            List<FmcTokenModels> fcmListUser = new List<FmcTokenModels>();
            foreach(var item in Collection)
            {
                if(item.UserId == userId)
                {
                    fcmListUser.Add(item);
                }
            }
            return fcmListUser;
        }
    }
}