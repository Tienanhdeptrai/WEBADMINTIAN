using System;
using System.Collections.Generic;
using System.Linq;
using HocWeb.App_Start;
using HocWeb.Models;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

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
                string applicationID = "AAAA2WbinkI:APA91bEMmV3LwHkor12q2zcC2t-Q5IZ8HZA226f2KNZkoObpqGYQJ4nGQMVpbouvOD635kBhDd5TlR4SofOi_DD4IjseYbg-rN7UI86wi4spOWASIYCB6wf0teBl28xgzaMzsNQoQG8Z";

                string senderId = "933734030914";

                string webAddr = "https://fcm.googleapis.com/fcm/send";

                string deviceId = "fsBTwGjTTMGFG377ZupQkl:APA91bFLN-Jcd5BOzb03pMUVPxpu1san-kPvUVgVwMTziDOgZQ6OULxpIOs3OTRq7iy4wBW3LVQICJfTb-zHJGiYlBTByocWak9C564BxR3Z-ZXA08nQ-Tr4GAGGnPa-v51n42pom90o";

                var result = "-1";
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", applicationID));
                httpWebRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                httpWebRequest.Method = "POST";

                var payload = new
                {
                    to = deviceId,
                    priority = "high",
                    content_available = true,
                    notification = new
                    {
                        body = obj.Details,
                        title = obj.Title
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