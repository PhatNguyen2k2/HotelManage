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
        public static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public static string DecodeFrom64(string encodedData)
        {
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decode = encoder.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encodedData);
            int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            string result = new String(decoded_char);
            return result;
        }
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
            Account account = db.Accounts.FirstOrDefault(a => a.username == user);
            if(account != null)
            {
                if(password == DecodeFrom64(account.password.ToString()))
                {
                     userId = account.position;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.Notification = "Wrong username or password";
                    return RedirectToAction("Login");
                }
            }
            else
            {
                ViewBag.Notification = "Wrong username or password";
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
        //POST: Home/Forgetpassword
        [HttpPost]
        public ActionResult ForgetPassword(string username, string answer1, string answer2)
        {
            ViewData["username"] = username;
            ViewData["answer1"] = answer1;
            ViewData["answer2"] = answer2;
            Account account = db.Accounts.FirstOrDefault(a => a.username == username);
            if(account != null)
            {
                if(DecodeFrom64(account.answer1) == answer1 && DecodeFrom64(account.answer2) == answer2)
                {
                    userId = account.username;
                    return RedirectToAction("ResetPassword");
                }
                else
                {
                    return RedirectToAction("Login");
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        //GET: Home/ResetPassword
        public ActionResult ResetPassword()
        {
            ViewBag.username = userId;
            return View("ResetPassword");
        }
        //POST: Home/ResetPassword
        public ActionResult ConfirmPassword(string password)
        {
            Account account = db.Accounts.FirstOrDefault(a => a.username == userId);
            account.password = EncodePasswordToBase64(password);
            db.SaveChanges();
            return RedirectToAction("Login");
        }
    }
}       