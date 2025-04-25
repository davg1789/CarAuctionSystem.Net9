using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Application.ViewModel
{
    public class VehicleSearchViewModel
    {
        public VehicleType? Type { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int? Year { get; set; }
    }
}
