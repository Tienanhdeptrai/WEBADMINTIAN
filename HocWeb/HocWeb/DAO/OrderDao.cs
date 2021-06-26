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
    public class OrderDao
    {
        private FirebaseContext dBContext;
        private List<OrderModels> OrderCollection = new List<OrderModels>();
        public OrderDao()
        {
            try
            {
                dBContext = new FirebaseContext();
                FirebaseResponse response1 = dBContext.Client.Get("Orders");
                dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response1.Body);
                foreach (var item in data1)
                {
                    OrderCollection.Add(JsonConvert.DeserializeObject<OrderModels>(((JProperty)item).Value.ToString()));
                }
            }
            catch { }

        }
        public List<OrderModels> ListAllPaging()
        {
            List<OrderModels> model = OrderCollection.AsQueryable<OrderModels>().ToList();
            return model;
        }
        public List<OrderModels> GetListOrder()
        {
            List<OrderModels> model = OrderCollection.AsQueryable<OrderModels>().ToList();
            return model;
        }
        public OrderModels ViewDetail(string id)
        {
            return OrderCollection.AsQueryable<OrderModels>().SingleOrDefault(x => x.OrderID == id);
        }     

        public bool hasOrderDetail(string id)
        {
            var result = OrderCollection.AsQueryable<OrderModels>().SingleOrDefault(x => x.CustomerID == id);
            if (result == null)
                return false;
            return true;
        }
        public List<OrderDetailModels> GetAll(string id)
        {
            List<OrderDetailModels> OrderDetailCollection = new List<OrderDetailModels>();
            FirebaseResponse response2 = dBContext.Client.Get("Orders/"+id+ "/OrderDetails/");
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            foreach (var item in data2)
            {
                OrderDetailCollection.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)item).Value.ToString()));
            }
            return OrderDetailCollection.AsQueryable<OrderDetailModels>().ToList();
        }
        public bool update(OrderModels models)
        {
            string dateTime = DateTime.Now.ToString();
            try
            {
                FirebaseResponse response = dBContext.Client.Update("Orders/" + models.OrderID,  models);
                OrderModels data = JsonConvert.DeserializeObject<OrderModels>(response.Body);
                var timeline = new TimeLineModels();
                FirebaseResponse response1 = dBContext.Client.Get("Orders/" + models.OrderID + "/TimeLine/");
                timeline = JsonConvert.DeserializeObject<TimeLineModels>(response1.Body);
                if (models.Status == "2")
                {
                    timeline.ChoXacNhan = dateTime;
                    FirebaseResponse response2 = dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "3")
                {
                    timeline.ChoLayHang = dateTime;
                    FirebaseResponse response2 = dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "4")
                {
                    timeline.DangVanChuyen = dateTime;
                    FirebaseResponse response2 = dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "5")
                {
                    timeline.DaGiaoHang = dateTime;
                    FirebaseResponse response2 = dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "6")
                {
                    timeline.DaHuy = dateTime;
                    FirebaseResponse response2 = dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else 
                {
                    timeline.TraHang = dateTime;
                    FirebaseResponse response2 = dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                return true;

            }
            catch
            {
                return false;
            }
        }
    }
}
