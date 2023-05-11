using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestProjectAPI.Data;
using TestProjectAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedDatabase(serviceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

async Task SeedDatabase(IServiceProvider serviceProvider)
{
    var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var dbContext = serviceProvider.GetRequiredService<AppDbContext>();

    var email = "test@test.com";
    var password = "Password1!";

    var existingUser = await userManager.FindByEmailAsync(email);
    if (existingUser == null)
    {
        var user = new IdentityUser
        {
            UserName = email,
            Email = email
        };

        await userManager.CreateAsync(user, password);
    }

    if (!dbContext.Bookings.Any())
    {
        var random = new Random();

        var names = new[] { "John Doe", "Jane Smith", "Michael Johnson", "Emily Davis" };
        var emails = new[] { "john@example.com", "jane@example.com", "michael@example.com", "emily@example.com" };
        var contactNumbers = new[] { "1234567890", "9876543210", "5551234567", "5559876543" };

        var startDate = new DateTime(2023, 6, 1);
        var endDate = new DateTime(2023, 6, 30);

        var bookings = new List<Booking>();
        var currentDate = startDate;

        while (currentDate <= endDate)
        {
            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                Name = names[random.Next(names.Length)],
                BookingDate = currentDate,
                Flexibility = (FlexibilityOptions)random.Next(3),
                VehicleSize = (VehicleSizeOptions)random.Next(4),
                ContactNumber = contactNumbers[random.Next(contactNumbers.Length)],
                EmailAddress = emails[random.Next(emails.Length)],
                IsApproved = false
            };

            bookings.Add(booking);

            currentDate = currentDate.AddDays(3);
        }

        dbContext.Bookings.AddRange(bookings);
        await dbContext.SaveChangesAsync();
    }
}