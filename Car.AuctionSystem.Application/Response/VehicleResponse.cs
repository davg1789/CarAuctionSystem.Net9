namespace Car.AuctionSystem.Application.Response
{
    public class VehicleResponse
    {
        public Guid Id { get; set; }
        public string Manufacturer { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int Year { get; set; }
        public decimal StartingBid { get; set; }        
        public string Type { get; set; } = string.Empty;
    }
}
