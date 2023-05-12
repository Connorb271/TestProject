namespace TestProjectFrontEnd.Models.BookingModels
{
    public class BookingModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public DateTime BookingDate { get; set; }

        public int Flexibility { get; set; }

        public int VehicleSize { get; set; }

        public string? ContactNumber { get; set; }

        public string? EmailAddress { get; set; }

        public bool IsApproved { get; set; }
    }

    public enum FlexibilityOptions
    {
        PlusMinus1Day = 0,
        PlusMinus2Days = 1,
        PlusMinus3Days = 2
    }

    public enum VehicleSizeOptions
    {
        Small = 0,
        Medium = 1,
        Large = 2,

    }
}
