﻿using Car.AuctionSystem.Infra.Data.Context;
using Car.AuctionSystem.Test.IntegrationTest.Seed;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Car.AuctionSystem.Test.IntegrationTest.Configuration
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        private readonly IConfiguration _configuration;

        public CustomWebApplicationFactory()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) 
                .AddJsonFile("appsettings.IntegrationTests.json", optional: true)
                .Build();
        }

        public HttpClient CreateClientWithHttps()
        {
            var baseUrl = _configuration["TestSettings:BaseUrl"];

            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("BaseUrl is not configured in appsettings.IntegrationTests.json.");

            return CreateClient(new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri(baseUrl)
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            Environment.SetEnvironmentVariable("IS_INTEGRATION_TEST", "true");

            builder.ConfigureServices(services =>
            {
                var descriptors = services
                 .Where(d => d.ServiceType == typeof(DbContextOptions<CarAuctionSystemContext>))
                 .ToList();

                foreach (var descriptor in descriptors)
                    services.Remove(descriptor);

                services.AddDbContext<CarAuctionSystemContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });           

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<CarAuctionSystemContext>();
                    var logger = scopedServices
                        .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                    db.Database.EnsureDeleted();

                    db.Database.EnsureCreated();

                    try
                    {
                        var context = scopedServices.GetRequiredService<CarAuctionSystemContext>();                        
                        DatabaseSeeder.Seed(context);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the " +
                                            "database with test messages. Error: {Message}", ex.Message);
                    }
                }
            });

            return base.CreateHost(builder);
        }
    }
}
