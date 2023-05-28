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
        public static string userId;
        //GET: Home/Login
        public ActionResult Login()
        {
            return View();
        }
        // POST: Home/Loginconfirm
        [HttpPost]
        public ActionResult LoginConfirm(string user, string password)
        {
            ViewData["user"] = user;
            ViewData["password"] = password;
            if(user == "admin" || user == "emp")
            {
                if(password == db.Accounts.FirstOrDefault(a => a.username == user).password.ToString())
                {
                     userId = user;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Notification = "Wrong password";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ViewBag.Notification = "Wrong username";
                return RedirectToAction("Login");
            }
        }
        // Home/Logout
        [HttpPost]
        public ActionResult Logout()
        {
            userId = "";
            return RedirectToAction("Login");
        }
        //GET: Home/Index
        public ActionResult Index()
        {
            if(userId != "admin" && userId != "emp")
            {
                return View("Login");
            }
            else
            {
                List<Bill> bills = db.Bills.ToList();
                long sum = db.Customers.Count();
                float total = 0f;
                bills.ForEach(x =>
                {
                    total += (float)x.total;
                });
                ViewBag.Total = total.ToString();
                ViewBag.Bill = bills;
                ViewBag.Customer = sum;
                ViewBag.userid = userId;
                ViewBag.Rooms = db.Rooms.OrderByDescending(r => r.love).Take(3).ToList();
                return View();
            }
        }
    }
}