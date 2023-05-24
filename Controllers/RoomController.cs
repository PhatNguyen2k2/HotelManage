using HotelManage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Web.UI.WebControls;
using cExcel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace HotelManage.Controllers
{
    public class RoomController : Controller
    {
        private HotelManageEntities db = new HotelManageEntities();
        // GET: Room/booking
        public ActionResult Booking()
        {
            if (HomeController.userId != "admin" && HomeController.userId != "emp")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Rooms = db.Rooms.ToList();
                ViewBag.Customers = db.Customers.ToList();
                ViewBag.Bookings = db.Bookings.ToList();
                return View();
            }
        }
        //GET: Room/manage
        public ActionResult Manage()
        {
            if (HomeController.userId != "admin")
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                ViewBag.Rooms = db.Rooms.ToList();
                return View();
            }
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
                    love = 0,
                    status = "ready"
                };
                db.Rooms.Add(r);
                db.SaveChanges();
                return RedirectToAction("Manage");
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

        //POST: Room/deleteRoomManage/{id}
        [HttpPost]
        public ActionResult DeleteRoom(int id)
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
        //POST: Room/delete/{id}
        [HttpPost]
        public ActionResult DeleteBooking (int id)
        {
            Booking booking = db.Bookings.FirstOrDefault(b => b.r_id == id);
            db.Bookings.Remove(booking);
            db.SaveChanges();
            return RedirectToAction("Booking");
        }
        //POST: Room/booking/search
        [HttpPost]
        public ActionResult Search(string search)
        {
            ViewData["search"] = search;
            ViewBag.Rooms = db.Rooms.Where(r => r.r_name.Contains(search)).ToList();
            ViewBag.Customers = db.Customers.ToList();
            ViewBag.Bookings = db.Bookings.ToList();
            return View("Booking");
        }
        //POST: Room/searchroom
        [HttpPost]
        public ActionResult SearchRoom(string search)
        {
            ViewData["search"] = search;
            ViewBag.Rooms = db.Rooms.Where(r => r.r_name.Contains(search)).ToList();
            return View("Manage");
        }
        //GET: Room/bill/{id}
        public ActionResult Bill(int id)
        {
            int eid = db.Employees.FirstOrDefault(e => e.e_name == "Phạm Thu Thủy").id;
            _ = db.Database.ExecuteSqlCommand(@"insert into Bill(c_id,e_id,status) values (@cid,@eid,'unpaid')", new SqlParameter("@cid",id), new SqlParameter("@eid",eid));
            db.SaveChanges();
            Bill bl = new Bill();
            bl = db.Bills.OrderByDescending(b => b.id).First();
            Booking booking = new Booking();
            booking = db.Bookings.FirstOrDefault(b => b.c_id == bl.c_id);
            ViewBag.Bill = bl;
            ViewBag.RoomServices = db.RoomServices.Where(rs => rs.r_id == booking.r_id).ToList();
            return View();
        }
        //POST: Room/printBill/{id}
        [HttpPost]
        public ActionResult PrintBill(int id, string love)
        {
            ViewData["love"] = love;
            Bill bill = new Bill();
            bill = db.Bills.FirstOrDefault(b => b.id == id);
            Booking booking = new Booking();
            booking = db.Bookings.FirstOrDefault(b => b.c_id == bill.c_id);
            Room room = new Room();
            room = db.Rooms.FirstOrDefault(r => r.id == booking.r_id);
            List < RoomService > roomServices = new List<RoomService>();
            roomServices = db.RoomServices.Where(rs => rs.r_id == booking.r_id).ToList();
            cExcel.Application Excel = new cExcel.Application();
            cExcel.Workbook wb = Excel.Workbooks.Add(cExcel.XlSheetType.xlWorksheet);
            cExcel.Worksheet ws = (cExcel.Worksheet)Excel.ActiveSheet;
            Excel.Visible = true;
            ws.Cells[1, 1] = "THOUSAND STARS HOTEL";
            ws.Cells[2, 1] = "Địa Chỉ: 451 Lê Văn Việt";
            ws.Cells[3, 1] = "Điện Thoại: 0354855613";
            ws.Cells[5, 2] = "Hóa đơn chi tiết";
            ws.Cells[6, 1] = "Mã hóa đơn";
            ws.Cells[6, 2] = "Mã khách hàng";
            ws.Cells[6, 3] = "Mã nhân viên";
            ws.Cells[6, 4] = "Ngày thanh toán";
            ws.Cells[6, 5] = "Tổng";
            ws.Cells[7, 1] = bill.id;
            ws.Cells[7, 2] = bill.c_id;
            ws.Cells[7, 3] = bill.e_id;
            ws.Cells[7, 4] = bill.b_date;
            ws.Cells[7, 5] = bill.total;
            ws.Cells[9, 1] = "Dịch vụ chi tiết";
            ws.Cells[10, 1] = "STT";
            ws.Cells[10, 2] = "Tên dịch vụ";
            ws.Cells[10, 3] = "Số lượng";
            int i = 11;
            foreach(var item in roomServices)
            {
                for(int j = 1; j <= 3; j++)
                {
                    if(j == 1)
                    {
                        ws.Cells[i, 1] = i - 10;
                    }
                    else if(j == 2)
                    {
                        ws.Cells[i, j] = item.s_name;
                    }
                    else
                    {
                        ws.Cells[i, j] = item.amount;
                    }
                }
                i++;
            }
            _ = db.Database.ExecuteSqlCommand(@"delete from RoomService where r_id = @rid", new SqlParameter("@rid", booking.r_id));
            db.Bookings.Remove(booking);
            bill.status = "paid";
            if(love == "love")
            {
                room.love += 1;
            }
            db.SaveChanges();
            ViewBag.Rooms = db.Rooms.ToList();
            ViewBag.Customers = db.Customers.ToList();
            ViewBag.Bookings = db.Bookings.ToList();
            return View("Booking");
        }
    }
}