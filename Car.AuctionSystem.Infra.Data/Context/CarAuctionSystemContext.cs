using Car.AuctionSystem.Domain.Entities;
using Car.AuctionSystem.Domain.Entities.Enum;
using Microsoft.EntityFrameworkCore;

namespace Car.AuctionSystem.Infra.Data.Context
{
    public class CarAuctionSystemContext : DbContext
    {
        public CarAuctionSystemContext(DbContextOptions<CarAuctionSystemContext> options)
            : base(options)
        {
        }

        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<Hatchback> Hatchbacks => Set<Hatchback>();
        public DbSet<Sedan> Sedans => Set<Sedan>();
        public DbSet<Suv> Suvs => Set<Suv>();
        public DbSet<Truck> Trucks => Set<Truck>();
        public DbSet<Auction> Auctions => Set<Auction>();
        public DbSet<Bid> Bids => Set<Bid>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(string) && property.GetMaxLength() == null)
                    {
                        property.SetMaxLength(100); 
                    }
                }
            }

            modelBuilder.Entity<Vehicle>()
                .HasDiscriminator(v => v.Type)
                .HasValue<Hatchback>(VehicleType.Hatchback)
                .HasValue<Sedan>(VehicleType.Sedan)
                .HasValue<Suv>(VehicleType.SUV)
                .HasValue<Truck>(VehicleType.Truck);

            modelBuilder.Entity<Vehicle>()
               .Property(v => v.StartingBid)
               .HasPrecision(18, 2);

            modelBuilder.Entity<Auction>()
             .HasOne(a => a.Vehicle)
             .WithMany(v => v.Auctions)
             .HasForeignKey(a => a.VehicleId);

            modelBuilder.Entity<Bid>()
               .HasOne(b => b.Auction)
               .WithMany(a => a.Bids)
               .HasForeignKey(b => b.AuctionId);

            modelBuilder.Entity<Bid>()
             .Property(b => b.Amount)
             .HasPrecision(18, 2);
        }
    }
}
