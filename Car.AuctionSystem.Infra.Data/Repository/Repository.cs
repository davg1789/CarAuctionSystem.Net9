using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Infra.Data.Context;

namespace Car.AuctionSystem.Infra.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly CarAuctionSystemContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(CarAuctionSystemContext context)
        {
            Db = context;
            DbSet = Db.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity obj)
        {
            var result = await DbSet.AddAsync(obj);
            await Db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await DbSet.FindAsync(id);
            if (entity != null)
            {
                DbSet.Remove(entity);
                await Db.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<TEntity> UpdateAsync(TEntity obj)
        {
            var existingEntity = await DbSet.FindAsync(GetPrimaryKeyValue(obj));
            if (existingEntity == null)
                throw new ArgumentException("Entity not found.");

            Db.Entry(existingEntity).CurrentValues.SetValues(obj);
            await Db.SaveChangesAsync();
            return existingEntity;          
        }

        private object GetPrimaryKeyValue(TEntity entity)
        {
            var keyName = Db.Model.FindEntityType(typeof(TEntity))!
                .FindPrimaryKey()!
                .Properties
                .Select(x => x.Name)
                .Single();

            return entity.GetType().GetProperty(keyName)!.GetValue(entity)!;
        }
    }
}
