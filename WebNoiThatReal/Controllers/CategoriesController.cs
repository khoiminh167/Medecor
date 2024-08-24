using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThatReal.Models;
using System.Data.Entity;

namespace WebNoiThatReal.Controllers
{
    public class CategoriesController : Controller
    {
        DBEntities database = new DBEntities();
        // GET: Categories
        public PartialViewResult CategoryParital()
        {
            var cateList = database.Categories.ToList();
            return PartialView(cateList);
        }
        public ActionResult Index()
        {
            return View(database.Categories.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Category cate)
        {


            database.Categories.Add(cate);
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cate = database.Categories.Where(s => s.Id == id).FirstOrDefault();
            return View(cate);
        }
        [HttpPost]
        public ActionResult Edit(int id, Category cate)
        {
            database.Entry(cate).State = System.Data.Entity.EntityState.Modified;
            database.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            return View(database.Categories.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Category cate)
        {
            cate = database.Categories.Where(s => s.Id == id).FirstOrDefault();
            database.Categories.Remove(cate);
            database.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}