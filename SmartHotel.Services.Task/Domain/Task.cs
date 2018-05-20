using System;

namespace SmartHotel.Services.Task.Domain
{
    public class Task
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rooom { get; set; }
        public bool Resolved { get; set; }
        public int TaskType { get; set; }
        public DateTime Submitted { get; set; }
        public string Description { get; set; }
    }
}