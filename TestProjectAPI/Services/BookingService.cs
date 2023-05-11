using Microsoft.EntityFrameworkCore;
using TestProjectAPI.Data;
using TestProjectAPI.Models;

namespace TestProjectAPI.Services
{
    public interface IBookingService
    {
        Task<Booking> GetBookingByIdAsync(Guid id);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Booking> CreateBookingAsync(CreateBookingModel bookingModel);
        Task<bool> DeleteBookingAsync(Guid id);
        Task<bool> UpdateBookingAsync(EditBookingModel bookingModel);
        Task<bool> ApproveBookingAsync(Guid id);
    }

    public class BookingService : IBookingService
    {
        private readonly AppDbContext _dbContext;

        public BookingService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Booking> GetBookingByIdAsync(Guid id)
        {
            return await _dbContext.Bookings.FindAsync(id);
        }

        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _dbContext.Bookings.OrderBy(b => b.BookingDate).ToListAsync();
        }

        public async Task<Booking> CreateBookingAsync(CreateBookingModel bookingModel)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                Name = bookingModel.Name,
                BookingDate = bookingModel.BookingDate,
                Flexibility = bookingModel.Flexibility,
                VehicleSize = bookingModel.VehicleSize,
                ContactNumber = bookingModel.ContactNumber,
                EmailAddress = bookingModel.EmailAddress,
                IsApproved = false
            };

            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();

            return booking;
        }

        public async Task<bool> DeleteBookingAsync(Guid id)
        {
            var booking = await _dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return false;
            }

            _dbContext.Bookings.Remove(booking);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateBookingAsync(EditBookingModel bookingModel)
        {
            var booking = await _dbContext.Bookings.FindAsync(bookingModel.Id);
            if (booking == null)
            {
                return false;
            }

            booking.Name = bookingModel.Name;
            booking.BookingDate = bookingModel.BookingDate;
            booking.Flexibility = bookingModel.Flexibility;
            booking.VehicleSize = bookingModel.VehicleSize;
            booking.ContactNumber = bookingModel.ContactNumber;
            booking.EmailAddress = bookingModel.EmailAddress;

            _dbContext.Entry(booking).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ApproveBookingAsync(Guid id)
        {
            var booking = await _dbContext.Bookings.FindAsync(id);
            if (booking == null)
            {
                return false;
            }

            booking.IsApproved = true;
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
