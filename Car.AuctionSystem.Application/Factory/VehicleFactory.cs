using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Entities.Enum;

namespace Car.AuctionSystem.Application.Factory
{
    public static class VehicleFactory
    {
        public static Vehicle Create(VehicleViewModel vehicleViewModel)
        {
            return vehicleViewModel.Type switch
            {
                VehicleType.Hatchback => new Hatchback(
                    vehicleViewModel.Manufacturer,
                    vehicleViewModel.Model,
                    vehicleViewModel.Year,
                    vehicleViewModel.StartingBid,
                    vehicleViewModel.NumberOfDoors!.Value
                ),
                VehicleType.Sedan => new Sedan(
                    vehicleViewModel.Manufacturer,
                    vehicleViewModel.Model,
                    vehicleViewModel.Year,
                    vehicleViewModel.StartingBid,
                    vehicleViewModel.NumberOfDoors!.Value
                ),
                VehicleType.SUV => new Suv(
                    vehicleViewModel.Manufacturer,
                    vehicleViewModel.Model,
                    vehicleViewModel.Year,
                    vehicleViewModel.StartingBid,
                    vehicleViewModel.NumberOfSeats!.Value
                ),
                VehicleType.Truck => new Truck(
                    vehicleViewModel.Manufacturer,
                    vehicleViewModel.Model,
                    vehicleViewModel.Year,
                    vehicleViewModel.StartingBid,
                    vehicleViewModel.LoadCapacity!.Value
                ),
                _ => throw new ArgumentException("Invalid vehicle type. Allowed values: Sedan, Hatchback, SUV, Truck.")
            };
        }
    }
}
