using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThatReal.Models;

namespace WebNoiThatReal.Controllers
{
    public class CustomersController : Controller
    {
        WebNoiThatRealEntities database = new WebNoiThatRealEntities();
        public ActionResult Index()
        {
            return View();
        }

        // GET: Customers
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Customer cus)
        {
            database.Customers.Add(cus);
            database.SaveChanges();
            return RedirectToAction("ShowCart", "ShoppingCart");
        }

        // GET: Customers/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Customers/Login
        [HttpPost]
        public ActionResult LoginAccount(Customer user)
        {
            var check = database.Customers.FirstOrDefault(m => m.UserNameCus == user.UserNameCus && m.PasswordCus == user.PasswordCus);

            if (check == null)
            {
                ViewBag.ErrorLogin = "Tên đăng nhập hoặc mật khẩu không chính xác.";
                return View("Login");
            }
            else
            {
                Session["UserNameCus"] = user.UserNameCus;
                return RedirectToAction("Index", "Home/Trangchu");
            }
        }

        // GET: Customers/Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customers/Register
        [HttpPost]
        public ActionResult Register(Customer user)
        {
            var check = database.Customers.FirstOrDefault(m => m.UserNameCus == user.UserNameCus);
            if (check != null)
            {
                ViewBag.ErrorRegister = "Tên người dùng đã được sử dụng.";
                return View("Register");
            }

            if (ModelState.IsValid)
            {
                database.Customers.Add(user);
                database.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.ErrorRegister = "Vui lòng điền đầy đủ thông tin. (*)";
                return View("Register", user);
            }
        }

        // GET: Customers/LogOutUser
        public ActionResult LogOutUser()
        {
            Session.Abandon();
            return RedirectToAction("Trangchu", "Home");
        }
    }
}