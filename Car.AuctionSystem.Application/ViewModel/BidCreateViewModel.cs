namespace Car.AuctionSystem.Application.ViewModel
{
    public class BidCreateViewModel
    {
        public Guid AuctionId { get; set; }
        public string Bidder { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}

