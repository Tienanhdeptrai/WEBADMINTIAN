using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Models;
using HocWeb.DAO;
namespace HocWeb.Areas.Admin.Controllers
{
    public class DefaultController : BaseController
    {
        // GET: Admin/Default
        public ActionResult Index()
        {
            List<decimal?> doanhthu = new List<decimal?>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> doanhthunam = new List<decimal?>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            List<decimal?> CountProduct = new List<decimal?>() { 0 };
            var order = new OrderDao().GetListOrder();
            decimal sanpham = new SellserDAO().Counts();
            CountProduct[0] = sanpham;
            ViewBag.CountProduct = CountProduct;
            decimal? price = 0;
            decimal? pricenam = 0;
            foreach (var item in order)
            {
                price = Convert.ToDecimal(item.Total);
                if ((item.CreatedDate).Substring(6,4)==DateTime.Now.Year.ToString() && item.Status == "4")
                {
                    switch (Convert.ToInt32((item.CreatedDate).Substring(3, 2)))
                    {
                        case 1:
                            doanhthu[0] = doanhthu[0] + price;
                            break;
                        case 2:
                            doanhthu[1] = doanhthu[1] + price;
                            break;
                        case 3:
                            doanhthu[2] = doanhthu[2] + price;
                            break;
                        case 4:
                            doanhthu[3] = doanhthu[3] + price;
                            break;
                        case 5:
                            doanhthu[4] = doanhthu[4] + price;
                            break;
                        case 6:
                            doanhthu[5] = doanhthu[5] + price;
                            break;
                        case 7:
                            doanhthu[6] = doanhthu[6] + price;
                            break;
                        case 8:
                            doanhthu[7] = doanhthu[7] + price;
                            break;
                        case 9:
                            doanhthu[8] = doanhthu[8] + price;
                            break;
                        case 10:
                            doanhthu[9] = doanhthu[9] + price;
                            break;
                        case 11:
                            doanhthu[10] = doanhthu[10] + price;
                            break;
                        default:
                            doanhthu[11] = doanhthu[11] + price;
                            break;
                    }
                }
            }
            ViewBag.DoanhThu = doanhthu;
            Chart a = new Chart();
            int choxacnhan = 0;
            int cholayhang = 0;
            int danggiaohang = 0;
            int dagiaohang = 0;
            int dahuy = 0;
            int trahang = 0;
            foreach (var item in order)
            {
                if (item.Status == "1")
                    choxacnhan++;
                else if (item.Status == "2")
                    cholayhang++;
                else if (item.Status == "3")
                    danggiaohang++;
                else if (item.Status == "4")
                    dagiaohang++;
                else if (item.Status == "5")
                    dahuy++;
                else if (item.Status == "6")
                    trahang++;
                pricenam = Convert.ToDecimal(item.Total);
                if (item.Status == "4")
                {
                    switch (Convert.ToInt32((item.CreatedDate).Substring(6, 4)))
                    {
                        case 2017:
                            doanhthunam[0] = doanhthunam[0] + pricenam;
                            break;
                        case 2018:
                            doanhthunam[1] = doanhthunam[1] + pricenam;
                            break;
                        case 2019:
                            doanhthunam[2] = doanhthunam[2] + pricenam;
                            break;
                        case 2020:
                            doanhthunam[3] = doanhthunam[3] + pricenam;
                            break;
                        case 2021:
                            doanhthunam[4] = doanhthunam[4] + pricenam;
                            break;
                    }
                }
            }
            decimal? tong = 0;
            for(int i=0;i<=4;i++)
            {
                if (doanhthunam[i]>=tong)
                tong = doanhthunam[i];
            }
            a.nam2015 = doanhthunam[0];
            a.nam2016 = doanhthunam[1];
            a.nam2017 = doanhthunam[2];
            a.nam2018 = doanhthunam[3];
            a.nam2019 = doanhthunam[4];
            
            ViewBag.ChoXacNhan = choxacnhan;
            ViewBag.ChoLayHang = cholayhang;
            ViewBag.DangGiaoHang = danggiaohang;
            ViewBag.DaGiao = dagiaohang;
            ViewBag.DaHuy = dahuy;
            ViewBag.TraHang = trahang;

            ViewBag.DoanhThuNam = doanhthunam;
            ViewBag.TongDoanhThuNam = tong;       
            return View();
        }
    }
}