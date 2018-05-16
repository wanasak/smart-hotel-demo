using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartHotel.Services.Booking.Commands;
using SmartHotel.Services.Booking.Queries;

namespace SmartHotel.Services.Booking.Controllers
{
    [Route("[controller]")]
    public class BookingsController : Controller
    {
        private readonly UserBookingQuery _userBookingQuery;
        private readonly CreateBookingCommand _createBookingCommand;
        public BookingsController(UserBookingQuery userBookingQuery, CreateBookingCommand createBookingCommand)
        {
            _createBookingCommand = createBookingCommand;
            _userBookingQuery = userBookingQuery;
        }

        [HttpGet()]
        [Authorize]
        public Task<ActionResult> Get()
        {
            var userId = User.Claims.Single(c => c.Type == "emails").Value;

            return GetBookingsOfUser(userId);
        }

        [HttpGet("{email}")]
        [Authorize]
        public async Task<ActionResult> Get(string email)
        {
            var userId = User.Claims.Single(c => c.Type == "emails").Value;

            if (!string.IsNullOrEmpty(email) && userId != email)
                return NotFound(new { Message = $"Bookings of {email} not found" });

            return await GetBookingsOfUser(userId);
        }

        [HttpGet("latest")]
        [Authorize]
        public Task<ActionResult> GetLatest()
        {
            var userId = User.Claims.Single(c => c.Type == "emails").Value;
            
            return GetLatestBookingOfUser(userId);
        }

        [HttpGet("latest/{email}")]
        [Authorize]
        public async Task<ActionResult> GetLatest(string email)
        {
            var userId = User.Claims.Single(c => c.Type == "emails").Value;

            if (!string.IsNullOrEmpty(email) && userId != email)
            {
                return NotFound(new { Message = $"Bookings of {email} not found" });
            }

            return await GetLatestBookingOfUser(userId);
        }

        private async Task<ActionResult> GetLatestBookingOfUser(string userId)
        {
            var booking = await _userBookingQuery.GetLatest(userId);

            if (booking == null) return NotFound();

            return Ok(booking);
        }

        private async Task<ActionResult> GetBookingsOfUser(string userId)
        {
            var bookings = await _userBookingQuery.GetAll(userId);

            if (bookings == null) return NotFound();

            return Ok(bookings);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Post([FromBody]BookingRequest bookingRequest)
        {
            var userId = User.Claims.Single(c => c.Type == "emails").Value;
            if (!string.IsNullOrEmpty(bookingRequest.UserId) && bookingRequest.UserId != userId)
                return BadRequest("If userId is used its value must be the logged user id");
            
            await _createBookingCommand.Execute(bookingRequest);
            
            return Ok();
        }
    }
}