using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThatReal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;

namespace WebNoiThatReal.Controllers
{
    public class HomeController : Controller
    {
        medecorEntities db = new medecorEntities();
        public ActionResult Trangchu()
        {
            // Lấy danh sách sản phẩm
            var productList = db.Products.OrderByDescending(x => x.NamePro).ToList();

            // Lấy danh sách sự kiện
            var eventList = db.Events.OrderByDescending(x => x.Title).ToList();

            // Tạo CombinedViewModel và gán dữ liệu vào
            var combinedViewModel = new CombinedViewModel
            {
                Products = productList,
                Events = eventList
            };

            // Truyền model vào view
            return View(combinedViewModel);
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        public ActionResult DangKi()
        {
            return View();
        }
        public ActionResult NoiThat()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult DichVu()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            // Lấy danh sách sản phẩm
            var productList = db.Products.OrderByDescending(x => x.NamePro).ToList();

            // Lấy danh sách sự kiện
            var eventList = db.Events.OrderByDescending(x => x.Title).ToList();

            // Tạo CombinedViewModel và gán dữ liệu vào
            var combinedViewModel = new CombinedViewModel
            {
                Products = productList,
                Events = eventList
            };

            // Truyền model vào view
            return View(combinedViewModel);
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