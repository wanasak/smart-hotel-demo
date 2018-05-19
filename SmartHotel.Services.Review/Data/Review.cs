using System;

namespace SmartHotel.Services.Review.Data
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime Submiited { get; set; }
        public string Description { get; set; }
        public int HotelId { get; set; }
        public string Username { get; set; }
        public string FormattedDate { get; set; }
    }
}