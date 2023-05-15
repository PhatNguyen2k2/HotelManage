using HotelManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManage.Controllers
{
    public class HomeController : Controller
    {
        private HotelManageEntities db = new HotelManageEntities();
        public ActionResult Index()
        {
            List<Bill> bills = db.Bills.ToList();
            long sum = db.Customers.Count();
            float total = 0f;
            bills.ForEach(x =>
            {
                total += (float)x.total;
            });
            ViewBag.Total = total;
            ViewBag.Bill = bills;
            ViewBag.Customer = sum;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}