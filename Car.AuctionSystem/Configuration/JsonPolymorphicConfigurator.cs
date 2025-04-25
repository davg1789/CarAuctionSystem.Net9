using Car.AuctionSystem.Application.Response;
using System.Text.Json.Serialization.Metadata;

namespace Car.AuctionSystem.Api.Configuration
{
    public static class JsonPolymorphicConfigurator
    {
        public static void Configure(JsonTypeInfo typeInfo)
        {
            if (typeInfo.Type == typeof(VehicleListResponse))
            {
                typeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "type",
                    IgnoreUnrecognizedTypeDiscriminators = true
                };

                typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(SedanListResponse), "Sedan"));
                typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(HatchbackListResponse), "Hatchback"));
                typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(SuvListResponse), "SUV"));
                typeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(typeof(TruckListResponse), "Truck"));
            }
        }
    }
}
