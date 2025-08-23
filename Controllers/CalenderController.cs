using Calender.Models;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace Calender.Controllers
{
    public class CalenderController : Controller
    {
        private AcademicCalendarEntities3 db = new AcademicCalendarEntities3();

        // Renders the calendar page
        public ActionResult Index()
        {
            return View();
        }

        // Returns all events as JSON for FullCalendar
        [HttpGet]
        public JsonResult GetEvents()
        {
            // 1. Fetch into memory
            var raw = db.Events
                .Select(e => new
                {
                    e.EventId,
                    e.Title,
                    e.StartTime,   // DateTimeOffset
                    e.EndTime,     // DateTimeOffset
                    e.Description
                })
                .ToList();

            // 2. Normalize MinValue and convert to DateTime?
            var clean = raw
              .Select(e =>
              {
                  // if StartTime is default (01-01-0001), drop it
                  DateTime? start = e.StartTime != default(DateTimeOffset)
              ? (DateTime?)e.StartTime.UtcDateTime
              : null;

                  // if EndTime is default, fallback to start
                  DateTime? end = e.EndTime != default(DateTimeOffset)
              ? (DateTime?)e.EndTime.UtcDateTime
              : start;

                  return new
                  {
                      e.EventId,
                      e.Title,
                      start,
                      end,
                      e.Description
                  };
              })
              .Where(x => x.start.HasValue)
              .ToList();

            // 3. Project to ISO strings
            var events = clean
              .Select(x => new
              {
                  id = x.EventId,
                  title = x.Title,
                  start = x.start.Value.ToString("s"),
                  end = x.end?.ToString("s"),
                  description = x.Description
              })
              .ToList();

            return Json(events, JsonRequestBehavior.AllowGet);
        }




        // Handles AJAX form submission to create a new event
        [HttpPost]
        public JsonResult CreateEvent(DateTime dateOfClass, string title, string description, Guid? tutorId = null)
        {
            // Use the first available tutor if none is provided (for demo/fallback)
            var selectedTutorId = tutorId ?? db.Tutors.Select(t => t.TutorId).FirstOrDefault();
            if (selectedTutorId == Guid.Empty || !db.Tutors.Any(t => t.TutorId == selectedTutorId))
            {
                return Json(new { success = false, error = "No valid tutor found. Please create a tutor first." });
            }

            var newEvent = new Event
            {
                DateOfClass = dateOfClass,
                Title = title,
                Description = description,
                TutorId = selectedTutorId
            };
            db.Events.Add(newEvent);
            db.SaveChanges();

            return Json(new
            {
                success = true,
                @event = new
                {
                    id = newEvent.EventId,
                    title = newEvent.Title,
                    start = newEvent.StartTime.ToString("s"),
                    end = newEvent.EndTime.ToString("s"),
                    description = newEvent.Description
                }
            });
        }

        // Add this method to enable event editing via AJAX
        [HttpPost]
        public JsonResult EditEvent(int? id, DateTime dateOfClass, string title, string description, DateTime startTime, DateTime endTime)
        {
            if (id == null)
                return Json(new { success = false, error = "Event id is required." });

            var ev = db.Events.Find(id.Value);
            if (ev == null)
                return Json(new { success = false, error = "Event not found." });

            ev.DateOfClass = dateOfClass;
            ev.Title = title;
            ev.Description = description;
            ev.StartTime = startTime;
            ev.EndTime = endTime;
            db.SaveChanges();

            return Json(new
            {
                success = true,
                @event = new
                {
                    id = ev.EventId,
                    title = ev.Title,
                    start = ev.DateOfClass.HasValue ? ev.DateOfClass.Value.ToString("s") : null,
                    description = ev.Description,
                    startTime = ev.StartTime.ToString("s"),
                    endTime = ev.EndTime.ToString("s")
                }
            });
        }

    }
}
