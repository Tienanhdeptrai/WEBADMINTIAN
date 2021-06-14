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
    public class SellserDAO
    {
        private FirebaseContext dBContext;
        private List<SellserModels> Collection = new List<SellserModels>();
        private List<AddressModels> CollectionAddress = new List<AddressModels>();
        public SellserDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Brief");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<SellserModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }
        }
        public List<SellserModels> GetAll()
        {
            return Collection;
        }
    
        public SellserModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Brief/" + id );
            SellserModels data = JsonConvert.DeserializeObject<SellserModels>(response.Body);
            return data;
        }
        public bool Update(SellserModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Brief/" + models.StoreID, models);
                SellserModels data = JsonConvert.DeserializeObject<SellserModels>(response.Body);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public List<AddressModels> GetListAddress(string id)
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Brief/" + id + "/Address/");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    if(item != null)
                    {
                        CollectionAddress.Add(JsonConvert.DeserializeObject<AddressModels>(((JProperty)item).Value.ToString()));
                    }         
                } 
            }
            catch { }
            return CollectionAddress;
        }
    }
}