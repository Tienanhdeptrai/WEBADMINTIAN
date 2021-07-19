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
        public List<OrderModels> GetListOrderbySeller (string sellerId)
        {
            List<OrderDetailModels> OrderDetailCollection = new List<OrderDetailModels>();
            List<OrderModels> orderList = new List<OrderModels>();

            foreach (var item in OrderCollection)
            {
                FirebaseResponse response2 = dBContext.Client.Get("Orders/" + item.OrderID + "/OrderDetails/");
                dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
                foreach (var items in data2)
                {
                    OrderDetailCollection.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)items).Value.ToString()));
                }
                foreach (var items in OrderDetailCollection)
                {
                    if(items.UserProduct == true && items.UserID == sellerId)
                    {
                        orderList.Add(item);
                        break;
                    }
                }
            }

            return orderList;
        }
        public List<OrderModels> GetListRefund()
        {
            List<OrderModels> orderModels = new List<OrderModels>();
            foreach(var item in OrderCollection)
            {
                if(item.Status == "6" && item.Payment == "02" && item.isRefund == false)
                {
                    List<OrderDetailModels> orderDetailModels = new List<OrderDetailModels>();
                    FirebaseResponse response = dBContext.Client.Get("Orders/" + item.OrderID + "/OrderDetails/");
                    dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    foreach (var itemDetail in data1)
                    {
                        orderDetailModels.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)itemDetail).Value.ToString()));
                    }
                    double temp = 0;
                    foreach (var itemDetail in orderDetailModels)
                    {
                        if (itemDetail.detailStatus == "1")
                        {
                            temp += itemDetail.Quantity * Convert.ToInt32(itemDetail.Price);
                        }
                    }
                    if (temp != 0)
                    {
                        item.RefundMonney = temp;
                        orderModels.Add(item);
                    }
                   
                }
            }
            return orderModels;
        }
        public List<OrderModels> GetListRejectBySeller()
        {
            List<OrderModels> orderModels = new List<OrderModels>();
            foreach (var item in OrderCollection)
            {
                if (item.Status == "2" && item.Payment == "02" && item.isRefund == false)
                {
                    List<OrderDetailModels> orderDetailModels = new List<OrderDetailModels>();
                    FirebaseResponse response = dBContext.Client.Get("Orders/" + item.OrderID + "/OrderDetails/");
                    dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    foreach (var itemDetail in data1)
                    {
                        orderDetailModels.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)itemDetail).Value.ToString()));
                    }
                    int temp = 0;
                    foreach (var itemDetail in orderDetailModels)
                    {

                        if (itemDetail.UserProduct == true && itemDetail.detailStatus == "2")
                        {
                            temp += itemDetail.Quantity * Convert.ToInt32(itemDetail.Price);
                        }
                    }
                    if (temp != 0)
                    {
                        item.RefundMonney = temp;
                        orderModels.Add(item);
                    }
                }
            }
            return orderModels;
        }
        public int checkOrder(string orderId, string sellerId)
        {
            List<OrderDetailModels> orderDetailModels = new List<OrderDetailModels>();
            FirebaseResponse response = dBContext.Client.Get("Orders/" + orderId + "/OrderDetails/");
            dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach (var item in data1)
            {
                orderDetailModels.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)item).Value.ToString()));
            }
            foreach(var items in orderDetailModels)
            {
                if(items.UserProduct == true && items.UserID == sellerId)
                {
                    if (items.detailStatus == "0")
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }            
            }
            return -1;
        }
        public List<OrderModels> GetByCustomer(string cusID)
        {
            List<OrderModels> orders = new List<OrderModels>();
            foreach (var item in OrderCollection)
            {
                if (item.CustomerID == cusID)
                {
                    orders.Add(item);
                }
            }
            return orders;
        }
        public List<OrderModels> GetByShipper (string shipperId)
        {
            List<OrderModels> orders = new List<OrderModels>();
            foreach(var item in OrderCollection)
            {
                if(item.ShipperID == shipperId)
                {
                    orders.Add(item);
                }
            }
            return orders;
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
        public OrderModels ViewDetailRefund(string id)
        {
            var model = OrderCollection.AsQueryable<OrderModels>().SingleOrDefault(x => x.OrderID == id);
            List<OrderDetailModels> orderDetailModels = new List<OrderDetailModels>();
            FirebaseResponse response = dBContext.Client.Get("Orders/" + model.OrderID + "/OrderDetails/");
            dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach (var itemDetail in data1)
            {
                orderDetailModels.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)itemDetail).Value.ToString()));
            }
            int temp = 0;
            foreach (var itemDetail in orderDetailModels)
            {
                if (itemDetail.detailStatus == "1")
                {
                    temp += itemDetail.Quantity * Convert.ToInt32(itemDetail.Price);
                }
            }
            if (temp != 0)
            {
                model.RefundMonney = temp;
            }
            return model;
        }
        public OrderModels ViewDetailSellerReject(string id)
        {
            var model = OrderCollection.AsQueryable<OrderModels>().SingleOrDefault(x => x.OrderID == id);
            List<OrderDetailModels> orderDetailModels = new List<OrderDetailModels>();
            FirebaseResponse response = dBContext.Client.Get("Orders/" + model.OrderID + "/OrderDetails/");
            dynamic data1 = JsonConvert.DeserializeObject<dynamic>(response.Body);
            foreach (var itemDetail in data1)
            {
                orderDetailModels.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)itemDetail).Value.ToString()));
            }
            int temp = 0;
            foreach (var itemDetail in orderDetailModels)
            {

                if (itemDetail.UserProduct == true && itemDetail.detailStatus == "2")
                {
                    temp += itemDetail.Quantity * Convert.ToInt32(itemDetail.Price);
                }
            }
            if (temp != 0)
            {
                model.RefundMonney = temp;
            }
            return model;
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
        public List<OrderDetailModels> GetAllRefund(string id)
        {
            List<OrderDetailModels> orderDetailModels = new List<OrderDetailModels>();
            List<OrderDetailModels> OrderDetailCollection = new List<OrderDetailModels>();
            FirebaseResponse response2 = dBContext.Client.Get("Orders/" + id + "/OrderDetails/");
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            foreach (var item in data2)
            {
                OrderDetailCollection.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)item).Value.ToString()));
            }
            foreach(var item in OrderDetailCollection)
            {
                if (item.detailStatus == "1")
                {
                    orderDetailModels.Add(item);
                }
            }
            return orderDetailModels;
        }
        public List<OrderDetailModels> GetAllRejectSeller(string id)
        {
            List<OrderDetailModels> orderDetailModels = new List<OrderDetailModels>();
            List<OrderDetailModels> OrderDetailCollection = new List<OrderDetailModels>();
            FirebaseResponse response2 = dBContext.Client.Get("Orders/" + id + "/OrderDetails/");
            dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
            foreach (var item in data2)
            {
                OrderDetailCollection.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)item).Value.ToString()));
            }
            foreach (var item in OrderDetailCollection)
            {
                if (item.UserProduct == true && item.detailStatus == "2")
                {
                    orderDetailModels.Add(item);
                }
            }
            return orderDetailModels;
        }
        public bool updateRefund(OrderModels models)
        {
            try
            {
                models.isRefund = true;
                FirebaseResponse response = dBContext.Client.Update("Orders/" + models.OrderID, models);
                //hoan tien

                PushNotification(models, "Refund");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool update(OrderModels models, string sellerId)
        {
            string dateTime = DateTime.Now.ToString();
            try
            {
                List<OrderDetailModels> OrderDetailCollection = new List<OrderDetailModels>();
                FirebaseResponse response = dBContext.Client.Update("Orders/" + models.OrderID,  models);
                OrderModels data = JsonConvert.DeserializeObject<OrderModels>(response.Body);
                var timeline = new TimeLineModels();
                FirebaseResponse response1 = dBContext.Client.Get("Orders/" + models.OrderID + "/TimeLine/");
                timeline = JsonConvert.DeserializeObject<TimeLineModels>(response1.Body);

                FirebaseResponse response2 = dBContext.Client.Get("Orders/" + models.OrderID + "/OrderDetails/");
                dynamic data2 = JsonConvert.DeserializeObject<dynamic>(response2.Body);
                foreach (var item in data2)
                {
                    OrderDetailCollection.Add(JsonConvert.DeserializeObject<OrderDetailModels>(((JProperty)item).Value.ToString()));
                }

                if (models.Status == "2")
                {
                    PushNotification(models, "XAC_NHAN");
                    timeline.ChoXacNhan = dateTime;
                    dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                    foreach(var item in OrderDetailCollection)
                    {
                        if(item.UserProduct == false)
                        {
                            item.detailStatus = "1";
                            dBContext.Client.Update("Orders/" + models.OrderID + "/OrderDetails/" + item.OrderDetailID, item);
                        }
                        else
                        {
                            if (item.UserID == sellerId)
                            {
                                item.detailStatus = "1";
                                dBContext.Client.Update("Orders/" + models.OrderID + "/OrderDetails/" + item.OrderDetailID, item);
                            }
                        }
                    }
                }
                else if (models.Status == "3")
                {
              
                    PushNotification(models, "CHO_GIAO");
                    timeline.ChoLayHang = dateTime;
                    dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "4")
                {
                    PushNotification(models, "GIAO_HANG");
                    timeline.DangVanChuyen = dateTime;
                    dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "5")
                {
                    PushNotification(models, "DA_GIAO");
                    timeline.DaGiaoHang = dateTime;
                    dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "6")
                {
                    PushNotification(models, "CHUA_GIAO");
                    timeline.DaHuy = dateTime;
                    dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                }
                else if (models.Status == "7")
                {
                    PushNotification(models, "HUY_DON");
                    timeline.DaHuy = dateTime;
                    dBContext.Client.Update("Orders/" + models.OrderID + "/TimeLine/", timeline);
                    foreach (var item in OrderDetailCollection)
                    {
                        if (item.UserProduct == false)
                        {
                            item.detailStatus = "2";
                            dBContext.Client.Update("Orders/" + models.OrderID + "/OrderDetails/" + item.OrderDetailID, item);
                            //
                        }
                        else
                        {
                            if(item.UserID == sellerId)
                            {
                                item.detailStatus = "2";
                                dBContext.Client.Update("Orders/" + models.OrderID + "/OrderDetails/" + item.OrderDetailID, item);
                                //
                            }
                        }
                    }
                }
                return true;

            }
            catch
            {
                return false;
            }
        }
        public void PushNotification(OrderModels obj, string type)
        {
            try
            {
                var fcmToken = new FmcTokenDAO().GetByUserId(obj.CustomerID);
                string[] fcmTokenArray = new string[fcmToken.Count];
                int i = 0;
                foreach (var item in fcmToken)
                {
                    fcmTokenArray[i++] = item.tokenDecide;
                }
                var bodyMess = "Đơn hàng đã được xác nhận";
                if(obj.Status == "2")
                {
                    bodyMess = "Đơn hàng đã được xác nhận thành công";
                }else if (obj.Status == "3")
                {
                    bodyMess = "Đơn hàng đã tới kho đang chờ vận chuyển";
                }
                else if (obj.Status == "4")
                {
                    bodyMess = "Đơn hàng đang được vận chuyển \nNgười giao hàng:" + obj.ShipName;
                   
                }else if (obj.Status == "5")
                {
                    bodyMess = "Đơn hàng giao thành công";
                 
                }else if (obj.Status == "6")
                {
                    bodyMess = "Giao hàng thất bại";
                }
                else if(obj.Status =="7"){
                    bodyMess = "Huỷ đơn hàng thành công";
                }
                if (type == "Refund")
                {
                    bodyMess = "Đơn hàng của bạn đã được hoàn tiền";
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
                        targetModule = type,
                        targetId = obj.OrderID
                    },
                    notification = new
                    {
                        body = bodyMess,
                        title = "Cập nhật trạng thái đơn hàng",
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
