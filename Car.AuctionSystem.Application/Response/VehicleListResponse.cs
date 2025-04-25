namespace Car.AuctionSystem.Application.Response
{
    public class VehicleListResponse
    {
        public Guid Id { get; set; }
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }
    }
}
