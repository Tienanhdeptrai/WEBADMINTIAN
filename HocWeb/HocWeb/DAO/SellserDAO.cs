using System;
using System.Collections.Generic;
using System.Linq;
using HocWeb.App_Start;
using HocWeb.Models;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace HocWeb.DAO
{
    public class SellserDAO
    {
        private FirebaseContext dBContext;
        private List<SellserModels> Collection = new List<SellserModels>();
        private List<AddressModels> CollectionAddress = new List<AddressModels>();
        private MechantModel CollectionUser = new MechantModel();
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
                ChangeStatus(models.StoreID);
                if(models.Status=="2")
                {
                    PushNotification(models);
                }
                else 
                {
                    PushNotification(models);
                }
            
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
        public int Counts()
        {
            return Collection.AsQueryable().Count(x => x.Status == "1");
        }
        public MechantModel GetUserById(string id)
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Users/" + id);
                dynamic data = JsonConvert.DeserializeObject(response.Body);
                CollectionUser = data;
            }
            catch { }
            return CollectionUser;
        }
        public bool ChangeStatus(string id)
        {
            var model = GetUserById(id);
            if (model.Merchant == true)
            {
                model.Merchant = false;
            }
            else
            {
                model.Merchant = true;
            }
            dBContext.Client.Update("Users/" + id, model);
            return model.Merchant;
        }
        public void PushNotification(SellserModels obj)
        {
            try
            {
                var fcmToken = new FmcTokenDAO().GetByUserId(obj.StoreID);
                var targetModules = "APPROVE_STORE";
                var bodyMess  = "STORE: " + obj.StoreName + " đã được cho phép kinh doanh!\n Hãy vào theo dõi ngay nào!!!";
                var titleMess = "Đăng kí STORE thành công";
                if (obj.Status != "2")
                {
                    targetModules = "REFUSE_STORE";
                    bodyMess = "STORE: " + obj.StoreName + " chưa đủ điều kiện để kinh doanh hay do thông tin sai lệch!!!";
                    titleMess = "Đăng kí STORE bị từ chối";
                }


                string[] fcmTokenArray = new string[fcmToken.Count];
                int i = 0;
                foreach (var item in fcmToken)
                {
                    fcmTokenArray[i++] = item.tokenDecide;
                }
                string applicationID = "AAAA2WbinkI:APA91bEBVS1RR8PzeEEcnVXieNKReaS4BTcFzKmRHMC-kvXymsbrmITORkNFcEbcqTGjaY1DfF5W6GMvGLOT9JwnOrutVfZ0xUByjSAud2ehowg4cpm2aPpmt1p3cj5IDxQd0ktVp1MX";

                string senderId = "933734030914";

                string webAddr = "https://fcm.googleapis.com/fcm/send";

                //string deviceId = "e3zCddOuSmKAaxmUZkkw8M:APA91bF-r2E-DemyZUgE5O-YAN8tsH-axJi9-Pj5yf7_wN9UOPpf6IaJFKBI7dZNk5kUe83V5Ghe6RV4jkXmyKXojG3aPb9Nrum081qHeU7r_pSV7JkfaENsmJ4LTB81ht-eVjGSNMnP";

                var result = "-1";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                httpWebRequest.Method = "POST";
                var payload = new
                {
                    registration_ids = fcmTokenArray,
                    priority = "high",
                    content_available = true,
                    data = new
                    {
                        targetModule = targetModules,
                        targetId = obj.StoreID
                    },
                    notification = new
                    {
                        body = bodyMess,
                        title = titleMess,
                    },
                };
                var serializer = new JavaScriptSerializer();
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = serializer.Serialize(payload);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();

                }
            }

            catch (Exception ex)
            {

                string str = ex.Message;

            }

        }
    }
}