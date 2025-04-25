using Car.AuctionSystem.Application.Implementation;
using Car.AuctionSystem.Application.Interface;
using Car.AuctionSystem.Domain.Interfaces.Repository;
using Car.AuctionSystem.Domain.Interfaces.Service;
using Car.AuctionSystem.Domain.Services;
using Car.AuctionSystem.Infra.Data.Context;
using Car.AuctionSystem.Infra.Data.Repositories;
using Car.AuctionSystem.Infra.Data.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Car.AuctionSystem.CrossCutting.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddCarAuctionSystemDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            var isIntegrationTest = Environment.GetEnvironmentVariable("IS_INTEGRATION_TEST");

            if (isIntegrationTest != "true")
            {
                services.AddDbContext<CarAuctionSystemContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            }

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IAuctionRepository, AuctionRepository>();
            services.AddScoped<IBidRepository, BidRepository>();

            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IAuctionService, AuctionService>();
            services.AddScoped<IBidService, BidService>();

            services.AddScoped<IVehicleAppService, VehicleAppService>();
            services.AddScoped<IAuctionAppService, AuctionAppService>();
            services.AddScoped<IBidAppService, BidAppService>();            

            services.AddValidatorsFromAssembly(Assembly.Load("Car.AuctionSystem.Application"));

            return services;
        }
    }
}
