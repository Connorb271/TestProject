using System.ComponentModel.DataAnnotations;
using TestProjectAPI.Data;

namespace TestProjectAPI.Models
{
    public class EditBookingModel
    {
        [Required]
        public Guid Id { get; set; }

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
