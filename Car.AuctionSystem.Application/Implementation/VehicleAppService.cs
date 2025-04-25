using Car.AuctionSystem.Application.Factory;
using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Application.Response;
using Car.AuctionSystem.Application.ViewModel;
using Car.AuctionSystem.Domain.Interfaces.Service;
using FluentValidation;

public class VehicleAppService : IVehicleAppService
{
    private readonly IVehicleService _vehicleService;
    private readonly IValidator<VehicleViewModel> _validator;

    public VehicleAppService(IVehicleService vehicleService, IValidator<VehicleViewModel> validator)
    {
        _vehicleService = vehicleService;
        _validator = validator;
    }

    public async Task<VehicleResponse> AddVehicleAsync(VehicleViewModel viewModel)
    {
        var validation = await _validator.ValidateAsync(viewModel);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);
        
        var vehicle = VehicleFactory.Create(viewModel);
        var vehicleCreated = await _vehicleService.AddAsync(vehicle);
        return VehicleResponseFactory.Create(vehicleCreated);
    }

    public async Task<VehicleResponse?> GetByIdAsync(Guid id)
    {
        var vehicle = await _vehicleService.GetByIdAsync(id);
        return vehicle == null ? null : VehicleResponseFactory.Create(vehicle);
    }

    public async Task<IEnumerable<VehicleListResponse>> GetAllAsync()
    {
        var vehicles = await _vehicleService.GetAllAsync();
        return vehicles.Select(VehicleListResponseFactory.Create);        
    }

    public async Task<IEnumerable<VehicleListResponse>> SearchAsync(VehicleSearchViewModel filter)
    {
        var vehicles = await _vehicleService.SearchAsync(v =>
            (filter.Type == null || v.Type == filter.Type) &&
            (string.IsNullOrEmpty(filter.Manufacturer) || v.Manufacturer == filter.Manufacturer) &&
            (string.IsNullOrEmpty(filter.Model) || v.Model == filter.Model) &&
            (!filter.Year.HasValue || v.Year == filter.Year));

        return vehicles.Select(VehicleListResponseFactory.Create);
    }

    public async Task<VehicleResponse> UpdateVehicleAsync(Guid id, VehicleViewModel viewModel)
    {
        var validation = await _validator.ValidateAsync(viewModel);
        if (!validation.IsValid)
            throw new ValidationException(validation.Errors);

        var updatedVehicle = VehicleFactory.Create(viewModel);
        updatedVehicle.Id = id;

        var result = await _vehicleService.UpdateAsync(updatedVehicle);
        return VehicleResponseFactory.Create(result);
    }
}
