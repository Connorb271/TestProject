using System.ComponentModel.DataAnnotations;

namespace TestProjectFrontEnd.Models.BookingModels
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
        public int Flexibility { get; set; }

        [Required]
        public int VehicleSize { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string? ContactNumber { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }

    }
}
