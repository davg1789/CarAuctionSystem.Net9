using Car.AuctionSystem.Application.ViewModel;
using FluentValidation;

namespace Car.AuctionSystem.Application.Validation
{
    public class BidCreateViewModelValidator : AbstractValidator<BidCreateViewModel>
    {
        public BidCreateViewModelValidator()
        {
            RuleFor(x => x.AuctionId)
                .NotEmpty().WithMessage("AuctionId is required.");

            RuleFor(x => x.Bidder)
                .NotEmpty().WithMessage("Bidder name is required.")
                .MaximumLength(100);

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Bid amount must be greater than zero.");
        }
    }
}
