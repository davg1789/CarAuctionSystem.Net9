using Car.AuctionSystem.Domain.Entities;
using System.Linq.Expressions;

namespace Car.AuctionSystem.Domain.Interfaces.Service
{
    public interface IVehicleService
    {
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<IEnumerable<Vehicle>> GetAllAsync();
        Task<Vehicle> UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(Guid id);
        Task<IEnumerable<Vehicle>> SearchAsync(Expression<Func<Vehicle, bool>> predicate);
    }
}
