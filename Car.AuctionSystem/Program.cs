using Car.AuctionSystem.Api.Configuration;
using Car.AuctionSystem.Api.Filter;
using Car.AuctionSystem.Api.Middleware;
using Car.AuctionSystem.CrossCutting.DependencyInjection;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.TypeInfoResolver = new DefaultJsonTypeInfoResolver
    {     
        Modifiers = { JsonPolymorphicConfigurator.Configure }
    };
});

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add DI from CrossCutting
builder.Services.AddCarAuctionSystemDependencies(builder.Configuration);

// Build and configure app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Car Auction API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseGlobalExceptionHandler();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
