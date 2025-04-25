using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.Mapper;
using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Interfaces.Service;
using FluentValidation;

public class BidAppService : IBidAppService
{
    private readonly IBidService _bidService;
    private readonly IValidator<BidCreateViewModel> _validator;

    public BidAppService(IBidService bidService, IValidator<BidCreateViewModel> validator)
    {
        _bidService = bidService;
        _validator = validator;
    }

    public async Task<BidResponse> PlaceBidAsync(BidCreateViewModel model)
    {
        var validation = await _validator.ValidateAsync(model);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var bid = await _bidService.PlaceBidAsync(model.AuctionId, model.Amount, model.Bidder);
        return BidMapper.ToResponse(bid);
    }

    public async Task<BidResponse?> GetByIdAsync(Guid id)
    {
        var bid = await _bidService.GetByIdAsync(id);
        return bid == null ? null : BidMapper.ToResponse(bid);
    }

    public async Task<IEnumerable<BidResponse>> GetByAuctionIdAsync(Guid auctionId)
    {
        var bids = await _bidService.GetByAuctionIdAsync(auctionId);
        return bids.Select(BidMapper.ToResponse);
    }
}
