using System.ComponentModel.DataAnnotations;

namespace TestProjectFrontEnd.Models.BookingModels
{
    public class CreateBookingModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required]
        public FlexibilityOptions Flexibility { get; set; }

        [Required]
        public VehicleSizeOptions VehicleSize { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
    }
}
