using System;
using System.Threading.Tasks;
using SmartHotel.Services.Booking.Data.Repositories;
using SmartHotel.Services.Booking.Domain;

namespace SmartHotel.Services.Booking.Commands
{
    public class BookingRequest
    {
        public int HotelId { get; set; }
        public string UserId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public byte Adults { get; set; }
        public byte Kids { get; set; }
        public byte Babies { get; set; }
        public int RoomType { get; set; }
        public int Price { get; set; }
    }

    public class CreateBookingCommand
    {
        private readonly BookingRepository _bookingRepository;
        private readonly UnitOfWork _uow;
        public CreateBookingCommand(BookingRepository bookingRepository, UnitOfWork uow)
        {
            _uow = uow;
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> Execute(BookingRequest bookingRequest)
        {
            var booking = new Booking.Domain.Booking
            {
                IdHotel = bookingRequest.HotelId,
                ClientEmail = bookingRequest.UserId,
                CheckInDate = bookingRequest.From,
                CheckOutDate = bookingRequest.To,
                TotalCost = bookingRequest.Price,
                NumberOfAdults = bookingRequest.Adults,
                NumberOfBabies = bookingRequest.Babies,
                NumberOfChildren = bookingRequest.Kids,
                IdRoomType = bookingRequest.RoomType
            };

            _bookingRepository.Add(booking);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}