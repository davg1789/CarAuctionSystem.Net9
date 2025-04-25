using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;

namespace Car.AuctionSystem.Application.Interface
{
    public interface IVehicleAppService
    {
        Task<VehicleResponse> AddVehicleAsync(VehicleViewModel viewModel);
        Task<VehicleResponse> UpdateVehicleAsync(Guid id, VehicleViewModel viewModel);
        Task<VehicleResponse?> GetByIdAsync(Guid id);
        Task<IEnumerable<VehicleListResponse>> GetAllAsync();
        Task<IEnumerable<VehicleListResponse>> SearchAsync(VehicleSearchViewModel filter);
    }
}
