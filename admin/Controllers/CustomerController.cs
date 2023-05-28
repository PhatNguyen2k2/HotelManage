using HotelManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HotelManage.Controllers
{
    public class CustomerController : Controller
    {
        private HotelManageEntities db = new HotelManageEntities();
        // GET: Customer/Index
        public ActionResult Index()
        {
            if (HomeController.userId != "admin" && HomeController.userId != "emp")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Customers = db.Customers.ToList();
                return View();
            }
        }
        //POST: Customer/create
        [HttpPost]
        public ActionResult Create(string name, string email, string phone, string address)
        {
            try
            {
                ViewData["name"] = name;
                ViewData["email"] = email;
                ViewData["phone"] = phone;
                ViewData["address"] = address;
                Customer c = new Customer
                {
                    c_name = name,
                    email = email,
                    phone = phone,
                    c_address = address,
                };
                db.Customers.Add(c);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //POST: Customer/update/{id}
        public ActionResult Update(int id, string name, string email, string phone, string address)
        {
            ViewData["name"] = name;
            ViewData["email"] = email;
            ViewData["phone"] = phone;
            ViewData["address"] = address;
            Customer customer = db.Customers.FirstOrDefault(b => b.id == id);
            customer.c_name = name;
            customer.email = email;
            customer.phone = phone;
            customer.c_address = address;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //POST: Customer/deletecustomer/{id}
        [HttpPost]
        public ActionResult DeleteCustomer(int id)
        {
            Customer customer = db.Customers.FirstOrDefault(b => b.id == id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //POST: Customer/search
        [HttpPost]
        public ActionResult Search(string search)
        {
            ViewData["search"] = search;    
            ViewBag.Customers = db.Customers.Where(c => c.c_name.Contains(search)).ToList();           
            return View("Index");
        }
    }
}