namespace Car.AuctionSystem.Application.Response
{
    public class BidResponse
    {
        public Guid Id { get; set; }
        public Guid AuctionId { get; set; }
        public decimal Amount { get; set; }
        public string Bidder { get; set; } = string.Empty;
        public DateTime PlacedAt { get; set; }
    }
}
