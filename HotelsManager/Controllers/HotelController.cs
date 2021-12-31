using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HotelsManager.Controllers
{
    public class HotelController : Controller
    {
        private readonly ModelContainer db = new ModelContainer();
        public ActionResult Index()
        {
            return View(db.Hotels);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Include(h => h.UploadedFiles).SingleOrDefault(h => h.IDHotel == id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include ="Address, City, Contact")] Hotel hotel, IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                hotel.UploadedFiles = new List<UploadedFile>();
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var picture = new UploadedFile
                        {
                            Name = System.IO.Path.GetFileName(file.FileName),
                            ContentType = file.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            picture.Content = reader.ReadBytes(file.ContentLength);
                        }
                        hotel.UploadedFiles.Add(picture);
                    }
                }
                db.Hotels.Add(hotel);
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
            Hotel hotel = db.Hotels.Include(h => h.UploadedFiles).SingleOrDefault(h => h.IDHotel == id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirmed(int id, IEnumerable<HttpPostedFileBase> files)
        {
            Hotel hotelToUpdate = db.Hotels.Find(id);
            if (TryUpdateModel(hotelToUpdate, "", new string[] { "Address", "City", "Contact"}))
            {
                if (hotelToUpdate.UploadedFiles == null)
                {
                    hotelToUpdate.UploadedFiles = new List<UploadedFile>();
                }
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var picture = new UploadedFile
                        {
                            Name = System.IO.Path.GetFileName(file.FileName),
                            ContentType = file.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            picture.Content = reader.ReadBytes(file.ContentLength);
                        }
                        hotelToUpdate.UploadedFiles.Add(picture);
                    }
                }
                
                db.Entry(hotelToUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hotelToUpdate);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hotel hotel = db.Hotels.Include(h => h.UploadedFiles).SingleOrDefault(h => h.IDHotel == id);
            if (hotel == null)
            {
                return HttpNotFound();
            }
            return View(hotel);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Reservations.RemoveRange(db.Reservations.Where(r => r.HotelIDHotel == id));
            db.UploadedFiles.RemoveRange(db.UploadedFiles.Where(f => f.HotelIDHotel == id));
            db.Hotels.Remove(db.Hotels.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
