using System;

namespace Calender.Models.Interfaces
{
    public interface IAuditable
    {
        DateTimeOffset? CreatedAt { get; set; }
        DateTimeOffset? UpdatedAt { get; set; }
    }
}