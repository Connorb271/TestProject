using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using TestProjectAPI.Data;
using TestProjectAPI.Services;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Database Configuration
var connectionString = configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

// Identity Configuration
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication Configuration
var jwtSecret = configuration.GetValue<string>("Jwt:Secret");
var jwtIssuer = configuration.GetValue<string>("Jwt:Issuer");
var key = Encoding.ASCII.GetBytes(jwtSecret);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Authorization Configuration
builder.Services.AddAuthorization();

// Controllers Configuration
builder.Services.AddControllers();

// Other Service Registrations
builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();

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

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    await SeedDatabase(serviceProvider);
}

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