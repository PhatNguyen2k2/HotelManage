using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HotelManage.Models;

namespace HotelManage.Controllers
{
    public class EmployeeController : Controller
    {
        private HotelManageEntities db = new HotelManageEntities();

        // GET: Employee
        public ActionResult Manage()
        {
            if (HomeController.userId != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Employees = db.Employees.ToList();
                return View();
            }
        }
        //POST: Employee/create
        [HttpPost]
        public ActionResult Create(string name, string phone, string address)
        {
            try
            {
                ViewData["name"] = name;
                ViewData["phone"] = phone;
                ViewData["address"] = address;
                Employee n = new Employee
                {
                    e_name = name,                 
                    phone = phone,
                    e_address = address,
                };
                db.Employees.Add(n);
                db.SaveChanges();
                return RedirectToAction("Manage");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //POST: Employee/update/{id}
        public ActionResult Update(int id, string name, string phone, string address)
        {
            ViewData["name"] = name;
            ViewData["phone"] = phone;
            ViewData["address"] = address;
            Employee employee = db.Employees.FirstOrDefault(b => b.id == id);
            employee.e_name = name;
            employee.phone = phone;
            employee.e_address = address;
            db.SaveChanges();
            return RedirectToAction("Manage");
        }

        //POST: Employee/deleteemployee/{id}
        [HttpPost]
        public ActionResult DeleteEmployee(int id)
        {
            Employee employee = db.Employees.FirstOrDefault(b => b.id == id);
            db.Employees.Remove(employee);
            db.SaveChanges();
            return RedirectToAction("Manage");
        }
        //POST: Employee/search
        [HttpPost]
        public ActionResult Search(string search)
        {
            ViewData["search"] = search;
            ViewBag.Employees = db.Employees.Where(e => e.e_name.Contains(search)).ToList();
            return View("Manage");
        }
    }
}
