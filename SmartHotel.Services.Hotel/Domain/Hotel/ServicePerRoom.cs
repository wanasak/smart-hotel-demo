namespace SmartHotel.Services.Hotel.Domain.Hotel
{
    public class ServicePerRoom
    {
        public int RoomTypeId { get; set; }
        public int ServiceId { get; set; }
        public RoomService Service { get; set; }
        public RoomType RoomType { get; set; }
    }
}