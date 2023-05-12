
using System.Net.Http.Headers;
using System.Net.Http.Json;
using TestProjectFrontEnd.Models.BookingModels;

namespace TestProjectFrontEnd.Services
{
    public interface IBookingService
    {
        Task<BookingModel> GetBookingByIdAsync(string id);
        Task<List<BookingModel>> GetAllBookingsAsync();
        Task<BookingModel> CreateBookingAsync(CreateBookingModel bookingModel);
        Task<bool> UpdateBookingAsync(EditBookingModel bookingModel);
        Task<bool> DeleteBookingAsync(Guid id);
        Task<bool> ApproveBookingAsync(Guid id);
    }

    public class BookingService : IBookingService
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenService _tokenService;

        public BookingService(HttpClient httpClient, ITokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
        }

        private async Task SetBearerTokenAsync()
        {
            var token = await _tokenService.GetStoredTokenAsync();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<BookingModel> GetBookingByIdAsync(string id)
        {
            await SetBearerTokenAsync();
            return await _httpClient.GetFromJsonAsync<BookingModel>($"bookings/{id}");
        }

        public async Task<List<BookingModel>> GetAllBookingsAsync()
        {
            await SetBearerTokenAsync();
            return await _httpClient.GetFromJsonAsync<List<BookingModel>>("bookings");
        }

        public async Task<BookingModel> CreateBookingAsync(CreateBookingModel bookingModel)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.PostAsJsonAsync("bookings", bookingModel);
            return await response.Content.ReadFromJsonAsync<BookingModel>();
        }

        public async Task<bool> UpdateBookingAsync(EditBookingModel bookingModel)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.PutAsJsonAsync($"bookings/{bookingModel.Id}", bookingModel);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteBookingAsync(Guid id)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.DeleteAsync($"bookings/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ApproveBookingAsync(Guid id)
        {
            await SetBearerTokenAsync();
            var response = await _httpClient.GetAsync($"bookings/{id}/approve");
            return response.IsSuccessStatusCode;
        }
    }
}
