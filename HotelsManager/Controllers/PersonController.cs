using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HotelsManager.Controllers
{
    public class PersonController : Controller
    {
        private readonly ModelContainer db = new ModelContainer();
        public ActionResult Index()
        {
            return View(db.People);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.SingleOrDefault(p => p.IDPerson == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "FirstName, LatName, Email")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.People.Add(person);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.SingleOrDefault(p => p.IDPerson == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirmed(int id)
        {
            Person personToUpdate = db.People.Find(id);
            if (TryUpdateModel(personToUpdate, "", new string[] { "FirstName", "LatName", "Email" }))
            {
                db.Entry(personToUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personToUpdate);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.SingleOrDefault(p => p.IDPerson == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Reservations.RemoveRange(db.Reservations.Where(r => r.PersonIDPerson == id));
            db.People.Remove(db.People.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}