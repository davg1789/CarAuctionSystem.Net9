namespace Car.AuctionSystem.Application.Response
{
    public class AuctionWithBidsResponse : AuctionResponse
    {
        public List<BidResponse> Bids { get; set; } = new();
    }
}
