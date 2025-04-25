using System.Linq.Expressions;
using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Domain.Interfaces.Service;

namespace Car.AuctionSystem.Domain.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _repository;

        public VehicleService(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {            
            var exists = await _repository.GetByIdAsync(vehicle.Id);
            if (exists != null)
                throw new InvalidOperationException("A vehicle with the same ID already exists.");

            return await _repository.AddAsync(vehicle);
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Vehicle>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<Vehicle>> SearchAsync(Expression<Func<Vehicle, bool>> predicate)
        {
            return await _repository.SearchAsync(predicate);
        }

        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {            
            return await _repository.UpdateAsync(vehicle);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
