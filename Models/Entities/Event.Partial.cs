using Calender.Models.Interfaces;
using System;

namespace Calender.Models.Entities
{
    public partial class Event : IAuditable
    {
        public DateTimeOffset? CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime DateOfClass { get; set; }
    }
    public partial class Tutor : IAuditable
    {
        public DateTimeOffset? CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTimeOffset? UpdatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
