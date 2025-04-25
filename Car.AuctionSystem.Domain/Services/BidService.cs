using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Domain.Interfaces.Service;

namespace Car.AuctionSystem.Domain.Services
{
    public class BidService : IBidService
    {
        private readonly IAuctionRepository _auctionRepository;
        private readonly IBidRepository _bidRepository;

        public BidService(IAuctionRepository auctionRepository, IBidRepository bidRepository)
        {
            _auctionRepository = auctionRepository;
            _bidRepository = bidRepository;
        }

        public async Task<Bid> PlaceBidAsync(Guid auctionId, decimal amount, string bidder)
        {
            var auction = await _auctionRepository.GetWithBidsByIdAsync(auctionId)
                ?? throw new KeyNotFoundException("Auction not found.");

            var bid = auction.PlaceBid(amount, bidder);

            var createdBid = await _bidRepository.AddAsync(bid);

            return createdBid;
        }

        public async Task<Bid?> GetByIdAsync(Guid bidId)
        {
            return await _bidRepository.GetByIdAsync(bidId);
        }

        public async Task<IEnumerable<Bid>> GetByAuctionIdAsync(Guid auctionId)
        {
            var auction = await _auctionRepository.GetWithBidsByIdAsync(auctionId)
                ?? throw new KeyNotFoundException("Auction not found.");

            return auction.Bids.OrderByDescending(b => b.PlacedAt);
        }
    }
}
