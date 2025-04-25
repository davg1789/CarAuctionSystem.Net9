using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;

namespace Car.AuctionSystem.Application.Interface
{
    public interface IAuctionAppService
    {
        Task<AuctionResponse> CreateAuctionAsync(AuctionCreateViewModel model);
        Task<AuctionResponse> StartAuctionAsync(Guid auctionId);
        Task<AuctionResponse> CloseAuctionAsync(Guid auctionId);
        Task<AuctionResponse?> GetByIdAsync(Guid auctionId);
        Task<IEnumerable<AuctionResponse>> GetAllAsync();
    }
}
