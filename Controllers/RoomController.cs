using HotelManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HotelManage.Controllers
{
    public class RoomController : Controller
    {
        private HotelManageEntities db = new HotelManageEntities();
        // GET: Room/booking
        public ActionResult Booking()
        {
            ViewBag.Rooms = db.Rooms.ToList();
            ViewBag.Customers = db.Customers.ToList();
            ViewBag.Bookings = db.Bookings.ToList();
            return View();
        }
        //GET: Room/manage
        public ActionResult Manage()
        {
            ViewBag.Rooms = db.Rooms.ToList();
            return View();
        }
        //POST: Room/create
        [HttpPost]
        public ActionResult Create(string type, float price)
        {
            try
            {
                ViewData["type"] = type;
                ViewData["price"] = price;
                Room r = new Room
                {
                    r_name = type,
                    price = price,
                    status = "ready"
                };
                db.Rooms.Add(r);
                db.SaveChanges();
                return View(Manage());
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //POST: Room/updateroom/{id}
        public ActionResult UpdateRoom(int id, string type, float price , string status)
        {
            ViewData["type"] = type;
            ViewData["price"] = price;
            ViewData["status"] = status;
            Room room = db.Rooms.FirstOrDefault(b => b.id == id);
            room.r_name = type;
            room.price = price;
            room.status = status;
            db.SaveChanges();
            return RedirectToAction("Manage");
        }

        // GET: Room/deleteroom/{id}
        public ActionResult DeleteRoom(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.FirstOrDefault(b => b.id == id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }

        //POST: Room/deleteRoomManage/{id}
        [HttpPost, ActionName("DeleteRoom")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoomConfirm(int id)
        {
            Room room = db.Rooms.FirstOrDefault(b => b.id == id);
            db.Rooms.Remove(room);
            db.SaveChanges();
            return RedirectToAction("Manage");
        }
        //POST: Room/booking/{id}
        [HttpPost]
        public ActionResult Booking(int id, int c_id, DateTime checkin, DateTime checkout)
        {
            ViewData["c_id"] = c_id;
            ViewData["checkin"] = checkin;
            ViewData["checkout"] = checkout;
            Booking booking = new Booking
            {
                r_id = id,
                c_id = c_id,
                checkin = checkin,
                checkout = checkout
            };
            db.Bookings.Add(booking);
            db.SaveChanges();
            return View(Booking());
        }
        //POST: Room/update/{id}
        [HttpPost]
        public ActionResult UpdateBooking (int id, int c_id, DateTime checkin, DateTime checkout)
        {
            ViewData["c_id"] = c_id;
            ViewData["checkin"] = checkin;
            ViewData["checkout"] = checkout;
            Booking booking = db.Bookings.FirstOrDefault(b => b.r_id == id && b.c_id == c_id);
            booking.checkin = checkin;
            booking.checkout = checkout;
            db.SaveChanges();
            return RedirectToAction("Booking");
        }
        // GET: Room/DeleteBooking/{id}
        public ActionResult DeleteBooking(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.FirstOrDefault(b => b.r_id == id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(booking);
        }
        //POST: Room/delete/{id}
        [HttpPost, ActionName("DeleteBooking")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBookingConfirm (int id)
        {
            Booking booking = db.Bookings.FirstOrDefault(b => b.r_id == id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Booking");
        }
    }
}