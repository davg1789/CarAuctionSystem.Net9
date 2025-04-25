using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.Mapper;
using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Service;
using FluentValidation;

namespace Car.AuctionSystem.Application.Implementation
{
    public class AuctionAppService : IAuctionAppService
    {
        private readonly IAuctionService _auctionService;
        private readonly IVehicleService _vehicleService;
        private readonly IValidator<AuctionCreateViewModel> _validator;

        public AuctionAppService(
            IAuctionService auctionService,
            IVehicleService vehicleService,
            IValidator<AuctionCreateViewModel> validator)
        {
            _auctionService = auctionService;
            _vehicleService = vehicleService;
            _validator = validator;
        }

        public async Task<AuctionResponse> CreateAuctionAsync(AuctionCreateViewModel model)
        {
            var validation = await _validator.ValidateAsync(model);
            if (!validation.IsValid)
                throw new ValidationException(validation.Errors);

            var vehicle = await _vehicleService.GetByIdAsync(model.VehicleId);
            if (vehicle is null)
                throw new KeyNotFoundException("Vehicle not found.");

            var auction = new Auction(vehicle, model.StartTime!.Value);
            var auctionCreated = await _auctionService.AddAsync(auction);
            return AuctionMapper.ToResponse(auctionCreated);
        }

        public async Task<AuctionResponse> StartAuctionAsync(Guid auctionId)
        {
            var auction = await _auctionService.GetByIdAsync(auctionId);
            if (auction == null)
                throw new KeyNotFoundException("Auction not found.");

            if (auction.IsActive)
                throw new InvalidOperationException("Auction is already active.");

            var hadPastBids = await _auctionService.HasPastAuctionWithBidsAsync(auction.VehicleId);

            if (hadPastBids)
                throw new InvalidOperationException("The vehicle is not in the inventory. This vehicle already participated in an auction with bids and cannot be auctioned again.");

            var activeAuction = await _auctionService.GetActiveByVehicleIdAsync(auction.VehicleId);

            if (activeAuction != null)
                throw new InvalidOperationException("There is already an active auction for this vehicle.");

            auction.IsActive = true;
            var auctionUpdated = await _auctionService.UpdateAsync(auction);            
            return AuctionMapper.ToResponse(auctionUpdated);
        }

        public async Task<AuctionResponse> CloseAuctionAsync(Guid auctionId)
        {
            var auction = await _auctionService.GetByIdAsync(auctionId);
            if (auction == null)
                throw new KeyNotFoundException("Auction not found.");

            if (auction.EndTime != null)
                throw new InvalidOperationException("Auction is already closed.");

            if (!auction.IsActive)
                throw new InvalidOperationException("Auction is not active.");

            auction.IsActive = false;
            auction.EndTime = DateTime.Now;
            var auctionUpdated = await _auctionService.UpdateAsync(auction);
            return AuctionMapper.ToResponse(auctionUpdated);
        }

        public async Task<AuctionResponse?> GetByIdAsync(Guid auctionId)
        {
            var auction = await _auctionService.GetWithBidsByIdAsync(auctionId);
            if (auction == null)
                return null;

            return auction.Bids.Count > 0
                ? AuctionMapper.ToResponseWithBids(auction) 
                : AuctionMapper.ToResponse(auction);
        }

        public async Task<IEnumerable<AuctionResponse>> GetAllAsync()
        {
            var auctions = await _auctionService.GetAllAsync();
            return auctions.Select(AuctionMapper.ToResponse);
        }
    }
}
