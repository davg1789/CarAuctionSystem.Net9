namespace Car.AuctionSystem.Domain.Entities
{
    public class Auction
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public virtual Vehicle? Vehicle { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<Bid> Bids { get; set; }

        public Auction() { }

        public Auction(Vehicle vehicle, DateTime? startTime)
        {
            Id = Guid.NewGuid();
            Vehicle = vehicle ?? throw new ArgumentNullException(nameof(vehicle));
            VehicleId = vehicle.Id;
            StartTime = startTime;
            IsActive = EndTime == null && StartTime >= DateTime.Now;
        }

        public Bid PlaceBid(decimal amount, string bidder)
        {
            if (!IsActive)
                throw new InvalidOperationException("Unable to bid on an inactive auction.");

            if (string.IsNullOrWhiteSpace(bidder))
                throw new ArgumentException("Bidder name is required.", nameof(bidder));

            if (amount <= 0)
                throw new ArgumentException("Bid amount must be greater than zero.", nameof(amount)); 
            
            if (Bids == null) 
            {   
                Bids = new List<Bid>();
            }

            var currentHighestBid = Bids.OrderByDescending(b => b.Amount).FirstOrDefault();

            if (currentHighestBid != null && amount <= currentHighestBid.Amount)
                throw new InvalidOperationException($"Bid amount must be greater than the current highest bid: {currentHighestBid}.");

            if (amount <= Vehicle?.StartingBid)
                throw new InvalidOperationException($"Bid amount must be greater than the starting bid: {Vehicle?.StartingBid}.");

            return new Bid(this, amount, bidder);
        }
    }
}
