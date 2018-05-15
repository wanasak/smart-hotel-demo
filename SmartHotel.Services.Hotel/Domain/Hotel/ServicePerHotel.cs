namespace SmartHotel.Services.Hotel.Domain.Hotel
{
    public class ServicePerHotel
    {
        public int HotelId { get; set; }
        public int ServiceId { get; set; }
        public HotelService Service { get; set; }
        public Hotel Hotel { get; set; }
    }
}