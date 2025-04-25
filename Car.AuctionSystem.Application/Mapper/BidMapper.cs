using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Domain.Entities;

namespace Car.AuctionSystem.Application.Mapper
{
    public static class BidMapper
    {
        public static BidResponse ToResponse(Bid bid)
        {
            return new BidResponse
            {
                Id = bid.Id,
                AuctionId = bid.AuctionId,
                Amount = bid.Amount,
                Bidder = bid.Bidder ?? string.Empty,
                PlacedAt = bid.PlacedAt
            };
        }
    }
}
