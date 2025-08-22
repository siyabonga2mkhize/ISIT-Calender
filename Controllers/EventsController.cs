using Calender.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Calender.Controllers
{
    public class EventsController : Controller
    {
        private AcademicCalendarEntities3 db = new AcademicCalendarEntities3();
        // The error CS1061 indicates that 'db.Tutors' does not exist on 'AcademicCalendarEntities1'.
        // To fix this, you need to ensure that the 'Tutors' DbSet is defined in your AcademicCalendarEntities1 context class.
        // If you have a Tutor entity and a corresponding table in your database, add the following property to your context class:

        // In AcademicCalendarEntities1.cs (your DbContext class file), add:
        public virtual DbSet<Tutor> Tutors { get; set; }

        // GET: Events
        public ActionResult Index()
        {


            // With this corrected line:
            var events = db.Events.Include("Tutor");
            return View(events.ToList());
        }

        // GET: Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Events/Create
        public ActionResult Create()
        {
            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "DisplayName");
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,TutorId,Title,Description,StartTime,EndTime,CreatedAt,UpdatedAt")] Event @event)
        {
            if (ModelState.IsValid)
            {
                @event.DateOfClass = @event.StartTime.Date; // or set as needed
                db.Events.Add(@event);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "DisplayName", @event.TutorId);
            return View(@event);
        }

        // GET: Events/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "DisplayName", @event.TutorId);
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,TutorId,Title,Description,StartTime,EndTime,CreatedAt,UpdatedAt")] Event @event)
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TutorId = new SelectList(db.Tutors, "TutorId", "DisplayName", @event.TutorId);
            return View(@event);
        }

        // GET: Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
