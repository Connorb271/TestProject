using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProjectAPI.Models;
using TestProjectAPI.Services;

namespace TestProjectAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(Guid id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
            {
                return NotFound();
            }

            return Ok(booking);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var booking = await _bookingService.CreateBookingAsync(bookingModel);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(Guid id, [FromBody] EditBookingModel bookingModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bookingModel.Id)
            {
                return BadRequest();
            }

            var isUpdated = await _bookingService.UpdateBookingAsync(bookingModel);
            if (!isUpdated)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(Guid id)
        {
            var isDeleted = await _bookingService.DeleteBookingAsync(id);
            if (!isDeleted)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("{id}/approve")]
        public async Task<IActionResult> ApproveBooking(Guid id)
        {
            var isApproved = await _bookingService.ApproveBookingAsync(id);
            if (!isApproved)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
