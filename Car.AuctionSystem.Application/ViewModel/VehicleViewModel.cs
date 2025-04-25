using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Application.ViewModel
{
    public class VehicleViewModel
    {
        public VehicleType Type { get; set; }

        public string Manufacturer { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public int Year { get; set; }

        public decimal StartingBid { get; set; }
        
        public int? NumberOfDoors { get; set; }
        
        public int? NumberOfSeats { get; set; }
        
        public double? LoadCapacity { get; set; }
    }
}
