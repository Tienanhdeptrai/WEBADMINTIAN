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
    public class AnnounceDAO
    {
        private FirebaseContext dBContext;
        private List<AnnounceModels> Collection = new List<AnnounceModels>();
        public AnnounceDAO()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response = dBContext.Client.Get("Announces");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                foreach (var item in data)
                {
                    Collection.Add(JsonConvert.DeserializeObject<AnnounceModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }

        }
        public List<AnnounceModels> ListAll()
        {
            return Collection;
        }
        public bool ChangeStatus(string id)
        {
            var model = new AnnounceModels();
            model = Collection.AsQueryable().SingleOrDefault(x => x.Id == id);
            if (model.Status == "True")
            {
                model.Status = "False";
            }
            else
            {
                model.Status = "True";
            }
            FirebaseResponse response = dBContext.Client.Update("Announces/" + id, model);
            AnnounceModels data = JsonConvert.DeserializeObject<AnnounceModels>(response.Body);
            return Convert.ToBoolean(model.Status);
        }
        public bool Insert(AnnounceModels models)
        {
           
            if (CheckProduct(models.Title) == true)
            {

                return false;
            }
            else
            {
                try
                {
                    var data = models;
                    PushResponse response = dBContext.Client.Push("Announces/", data);
                    data.Id = response.Result.name;
                    SetResponse setResponse = dBContext.Client.Set("Announces/" + data.Id, data);       
                    if (models.Status == "True")
                    {
                        PushNotification(models);
                    }
                }
                catch { }
            }
            return true;
        }
        public AnnounceModels ViewDetail(string id)
        {
            FirebaseResponse response = dBContext.Client.Get("Announces/" + id);
            AnnounceModels data = JsonConvert.DeserializeObject<AnnounceModels>(response.Body);
            return data;
        }
        public bool update(AnnounceModels models)
        {
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Announces/" + models.Id, models);
                AnnounceModels data = JsonConvert.DeserializeObject<AnnounceModels>(response.Body);
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
                FirebaseResponse response = dBContext.Client.Delete("Announces/" + id);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool CheckProduct(string name)
        {
            return Collection.AsQueryable().Count(x => x.Title == name) > 0;
            
        }
        public void PushNotification(AnnounceModels obj)
        {
            try
            {
                var fcmToken = new FmcTokenDAO().GetAll();
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
                        targetModule = obj.Type,
                        targetId = obj.Id
                    },
                    notification = new
                    {
                        body = obj.Details,
                        title = obj.Title,
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