using System.Data.Entity;

namespace Calender.Models
{
    public partial class AcademicCalendarEntities1 : DbContext
    {
        public virtual DbSet<Event> Events { get; set; }

        public System.Data.Entity.DbSet<Calender.Models.Tutor> Tutors { get; set; }
        // Existing code...
    }
}
