using Car.AuctionSystem.Application.ViewModel;
using FluentValidation;

namespace Car.AuctionSystem.Application.Validation
{
    public class AuctionCreateViewModelValidator : AbstractValidator<AuctionCreateViewModel>
    {
        public AuctionCreateViewModelValidator()
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("VehicleId is required.");
        }
    }
}
