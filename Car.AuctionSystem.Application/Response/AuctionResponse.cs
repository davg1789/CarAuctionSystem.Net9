namespace Car.AuctionSystem.Application.Response
{
    public class AuctionResponse
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
