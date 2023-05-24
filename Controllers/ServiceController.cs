using HotelManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace HotelManage.Controllers
{
    public class ServiceController : Controller
    {
        private HotelManageEntities db = new HotelManageEntities();
        // GET: Service/index
        public ActionResult Index()
        {
            if (HomeController.userId != "admin" && HomeController.userId != "emp")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                List<Room> rooms = new List<Room>();
                Room r = new Room();
                db.Bookings.ToList().ForEach(b =>
                {
                    r = db.Rooms.FirstOrDefault(x => x.id == b.r_id);
                    if (r != null)
                    {
                       rooms.Add(r);
                    }
                });
                ViewBag.Rooms = rooms;
                return View();
            }
        }
        // GET: Service/ManageService
        public ActionResult ManageService()
        {
            if (HomeController.userId != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Services = db.CusServices.ToList();
                ViewBag.Employees = db.Employees.ToList();
                return View();
            }
        }
        // GET: Service/bookingservice/{id}
        public ActionResult BookingService(int id)
        {
            ViewBag.RoomServices = db.RoomServices.Where(r => r.r_id == id).ToList();
            ViewBag.Services = db.CusServices.ToList();
            return View();
        }
        // POST: Service/addservice/{id}
        [HttpPost, ActionName("AddService")]
        public ActionResult AddService(int id, string name, int amount)
        {
            ViewData["amount"] = amount;
            RoomService r = new RoomService();
            r = db.RoomServices.FirstOrDefault(s => s.r_id == id && s.s_name == name);
            if(r != null)
            {
                r.amount += amount;
                db.SaveChanges();
            }
            else
            {
                RoomService rs = new RoomService
                {
                    r_id = id,
                    s_name = name,
                    amount = amount
                };
                db.RoomServices.Add(rs);
                db.SaveChanges();
            }
            return RedirectToAction("BookingService/"+id);
        }
        //POST: Service/deleteservice/{id}
        [HttpPost, ActionName("DeleteService")]
        public ActionResult DeleteConfirm(int id, string name)
        {
            RoomService rs = db.RoomServices.FirstOrDefault(b => b.r_id == id && b.s_name == name);
            db.RoomServices.Remove(rs);
            db.SaveChanges();
            return RedirectToAction("BookingService/" + id);
        }

        //POST: Service/create
        [HttpPost]
        public ActionResult Create(string name, int empID, float price)
        {
            try
            {
                ViewData["name"] = name;
                ViewData["empID"] = empID;
                ViewData["price"] = price;
  
                CusService cs = new CusService
                {
                    s_name = name,
                    e_id = empID,
                    price = price,
                    
                };
                db.CusServices.Add(cs);
                db.SaveChanges();
                return RedirectToAction("ManageService");
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //POST: Service/update/{name}
        public ActionResult Update(string name, int empID, float price)
        {
            ViewData["empID"] = empID;
            ViewData["price"] = price;
            CusService cs = db.CusServices.FirstOrDefault(b => b.s_name == name);
            cs.e_id = empID;
            cs.price = price;
            db.SaveChanges();
            return RedirectToAction("ManageService");
        }
        //POST: Service/deleteservice/{name}
        [HttpPost]
        public ActionResult DeleteCusService(string name)
        {
            CusService cs = db.CusServices.FirstOrDefault(b => b.s_name == name);
            db.CusServices.Remove(cs);
            db.SaveChanges();
            return RedirectToAction("ManageService");
        }
        //POST: Service/searchroom
        [HttpPost]
        public ActionResult SearchRoom(string search)
        {
            ViewData["search"] = search;
            List<Room> rooms = new List<Room>();
            Room r = new Room();
            db.Bookings.ToList().ForEach(b =>
            {
                r = db.Rooms.FirstOrDefault(x => x.id == b.r_id && x.r_name.Contains(search));
                if(r != null)
                {
                   rooms.Add(r);
                }
            });
            ViewBag.Rooms = rooms;
            return View("Index");
        }
        //POST : Service/searchservice
        public ActionResult SearchService(string search)
        {
            ViewData["search"] = search;
            ViewBag.Services = db.CusServices.Where(s => s.s_name.Contains(search)).ToList();
            ViewBag.Employees = db.Employees.ToList();
            return View("ManageService");
        }
    }
}