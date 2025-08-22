using Calender.Models.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace Calender.Models
{
    public partial class AcademicCalendarEntities3 : DbContext // <-- Add base class
    {
        public override int SaveChanges()
        {
            var utcNow = DateTimeOffset.Now;

            // Capture timestamps on Added and Modified entries
            foreach (var entry in ChangeTracker.Entries()
                                         .Where(e => e.Entity is IAuditable &&
                                                    (e.State == EntityState.Added ||
                                                     e.State == EntityState.Modified)))
            {
                var auditable = (IAuditable)entry.Entity;

                if (entry.State == EntityState.Added)
                    auditable.CreatedAt = utcNow;

                auditable.UpdatedAt = utcNow;
            }

            return base.SaveChanges();
        }
    }
}