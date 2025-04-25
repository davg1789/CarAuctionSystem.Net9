namespace Car.AuctionSystem.Domain.Entities
{
    public class Bid
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AuctionId { get; set; }
        public virtual Auction? Auction { get; set; }
        public decimal Amount { get; set; }
        public string? Bidder { get; set; }
        public DateTime PlacedAt { get; set; }

        public Bid() { }

        public Bid(Auction auction, decimal amount, string bidder)
        {
            Auction = auction ?? throw new ArgumentNullException(nameof(auction));
            AuctionId = auction.Id;
            Amount = amount;
            Bidder = bidder ?? throw new ArgumentNullException(nameof(bidder));
            PlacedAt = DateTime.UtcNow;
        }
    }
}
