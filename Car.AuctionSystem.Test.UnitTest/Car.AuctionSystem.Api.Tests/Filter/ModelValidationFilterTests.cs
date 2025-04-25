using Car.AuctionSystem.Api.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using System.Text.Json;


namespace Car.AuctionSystem.Test.UnitTest.Api.Filter
{
    public class ModelValidationFilterTests
    {
        private static ActionExecutingContext GetContextWithError(string field, string message)
        {
            var modelState = new ModelStateDictionary();
            modelState.AddModelError(field, message);

            var actionContext = new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ControllerActionDescriptor(),
                modelState
            );

            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                controller: null
            );
        }

        [Theory]
        [InlineData("$.type", "The JSON value could not be converted to Car.AuctionSystem.Domain.Entities.Enum.VehicleType.", "Invalid vehicle type. Allowed values: Sedan, Hatchback, SUV, Truck.")]
        [InlineData("$.loadCapacity", "$.loadCapacity must be a number.", "Load Capacity must be a number.")]
        [InlineData("$.year", "$.year must be a number.", "Year must be a number.")]
        [InlineData("$.model", "The JSON value could not be converted to System.String", "Model must be text (string type).")]
        [InlineData("$.manufacturer", "The JSON value could not be converted to System.String", "Manufacturer must be text (string type).")]
        [InlineData("$.startingBid", "startingBid", "Starting bid must be a number.")]
        [InlineData("$.numberOfDoors", "numberOfDoors", "Number of doors must be a number.")]
        [InlineData("$.numberOfSeats", "numberOfSeats", "Number of seats must be a number.")]
        public void OnActionExecuting_ShouldReturnCustomErrorMessages(string field, string message, string expected)
        {
            // Arrange
            var context = GetContextWithError(field, message);
            var filter = new ModelValidationFilter();

            // Act
            filter.OnActionExecuting(context);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(context.Result);
            var responseJson = JsonSerializer.Serialize(result.Value);

            Assert.Contains(expected, responseJson);
        }
    }

}
