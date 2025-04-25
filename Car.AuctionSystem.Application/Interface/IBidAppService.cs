using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;

namespace Car.AuctionSystem.Application.Interface
{
    public interface IBidAppService
    {
        Task<BidResponse> PlaceBidAsync(BidCreateViewModel model);
        Task<BidResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<BidResponse>> GetByAuctionIdAsync(Guid auctionId);
    }
}
