using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebNoiThatReal.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using PagedList;

namespace WebNoiThatReal.Controllers
{
    public class EventController : Controller
    {
        medecorEntities db = new medecorEntities();
        // GET: Event
        public ActionResult Index()
        {
            return View(db.Events.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            List<Event> list = db.Events.ToList();

            Event eve = new Event
            {
                EventDate = DateTime.Now
            };
            return View(eve);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Event eve)
        {
            List<Event> list = db.Events.ToList();
            try
            {
                if (eve.UploadImage != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(eve.UploadImage.FileName);
                    string extent = Path.GetExtension(eve.UploadImage.FileName);
                    filename = filename + extent;
                    eve.ImageUrl = "~/Content/Images/Event/" + filename;
                    eve.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/Content/Images/Event"), filename));
                }
                
                db.Events.Add(eve);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View(db.Events.Where(s => s.Id == id).FirstOrDefault());
        }
        [HttpPost]
        public ActionResult Delete(int id, Event eve)
        {
            eve = db.Events.Where(s => s.Id == id).FirstOrDefault();
            db.Events.Remove(eve);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult EventView()
        {
            var EventList = db.Events.OrderByDescending(x => x.Title);
            return View(EventList);
        }

        public ActionResult EventDetail(int id)
        {
            // Retrieve the event by id and pass it to the view
            var eve = db.Events.Find(id); // Assuming dbContext is your data context
        if (eve == null)
        {
            return HttpNotFound();
        }
        return View(eve);
        }
    }
}