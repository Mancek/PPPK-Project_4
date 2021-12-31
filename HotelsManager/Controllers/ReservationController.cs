using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HotelsManager.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ModelContainer db = new ModelContainer();
        public ActionResult Index()
        {
            return View(db.Reservations);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.SingleOrDefault(r => r.IDReservation == id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        public ActionResult Create()
        {
            ViewBag.AllHotels = db.Hotels;
            ViewBag.AllPeople = db.People;
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "FromDate, ToDate, HotelIDHotel, PersonIDPerson")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Reservations.Add(reservation);
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
            Reservation reservation = db.Reservations.SingleOrDefault(r => r.IDReservation == id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.AllHotels = db.Hotels;
            ViewBag.AllPeople = db.People;
            return View(reservation);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirmed(int id)
        {
            Reservation reservationToUpdate = db.Reservations.Find(id);
            if (TryUpdateModel(reservationToUpdate, "", new string[] { "FromDate", "ToDate", "HotelIDHotel", "PersonIDPerson" }))
            {
                db.Entry(reservationToUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reservationToUpdate);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.SingleOrDefault(r => r.IDReservation == id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Reservations.Remove(db.Reservations.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}