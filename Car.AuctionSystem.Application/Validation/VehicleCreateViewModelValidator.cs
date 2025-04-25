using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities.Enum;
using FluentValidation;

namespace Car.AuctionSystem.Application.Validation
{
    public class VehicleViewModelValidator : AbstractValidator<VehicleViewModel>
    {
        public VehicleViewModelValidator()
        {
            RuleFor(x => x.Manufacturer).NotEmpty();
            RuleFor(x => x.Model).NotEmpty();
            RuleFor(x => x.Year).InclusiveBetween(1950, DateTime.UtcNow.Year + 1);
            RuleFor(x => x.StartingBid).GreaterThan(0);
            RuleFor(x => x.Type)
            .NotEqual(VehicleType.Unknown)
            .WithMessage("Invalid vehicle type. Allowed values: Sedan, Hatchback, SUV, Truck.");

            When(x => x.Type == VehicleType.Sedan || x.Type == VehicleType.Hatchback, () =>
            {
                RuleFor(x => x.NumberOfDoors).NotNull().GreaterThan(0);
            });

            When(x => x.Type == VehicleType.SUV, () =>
            {
                RuleFor(x => x.NumberOfSeats).NotNull().GreaterThan(0);
            });

            When(x => x.Type == VehicleType.Truck, () =>
            {
                RuleFor(x => x.LoadCapacity).NotNull().GreaterThan(0);
            });
        }
    }
}
