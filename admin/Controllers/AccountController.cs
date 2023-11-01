using HotelManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HotelManage.Controllers
{
    public class AccountController : Controller
    {
        private HotelManageEntities db = new HotelManageEntities();
        // GET: Account
        public ActionResult Index()
        {
            if (HomeController.userId != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Accounts = db.Accounts.ToList();
                return View();
            }
        }
        //POST: Account/create
        [HttpPost]
        public ActionResult Create(string username, string password)
        {
            try
            {
                ViewData["username"] = username;
                ViewData["password"] = password;

                Account account = new Account
                {
                    username = username,
                    password = HomeController.EncodePasswordToBase64(password),
                   
                };
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //POST: Account/update/{username}
        public ActionResult Update(string username, string password)
        {
            ViewData["username"] = username;
            ViewData["password"] = password;
            
            Account account = db.Accounts.FirstOrDefault(b => b.username == username);
            account.password = HomeController.EncodePasswordToBase64(password);
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //POST: Account/deleteaccount/{username}
        [HttpPost]
        public ActionResult DeleteAccount(string username)
        {
            Account account = db.Accounts.FirstOrDefault(b => b.username == username);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //POST: Account/search
        [HttpPost]
        public ActionResult Search(string search)
        {
            ViewData["search"] = search;
            ViewBag.Accounts = db.Accounts.Where(a => a.username.Contains(search)).ToList();
            return View("Index");
        }
    }
}