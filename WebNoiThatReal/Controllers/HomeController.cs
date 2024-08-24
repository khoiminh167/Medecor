using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebNoiThatReal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Trangchu()
        {
            return View();
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
            return View();
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