using Car.AuctionSystem.Domain.Entities;

namespace Car.AuctionSystem.Domain.Interfaces.Service
{
    public interface IBidService
    {
        Task<Bid> PlaceBidAsync(Guid auctionId, decimal amount, string bidder);
        Task<Bid?> GetByIdAsync(Guid bidId);
        Task<IEnumerable<Bid>> GetByAuctionIdAsync(Guid auctionId);
    }
}
