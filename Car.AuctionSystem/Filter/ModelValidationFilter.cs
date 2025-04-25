using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Car.AuctionSystem.Api.Filter
{
    public class ModelValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var validationErrors = new List<string>();

                foreach (var modelStateEntry in context.ModelState)
                {
                    var propertyName = modelStateEntry.Key;
                    var propertyErrors = modelStateEntry.Value?.Errors;

                    if (propertyErrors == null) continue;

                    foreach (var error in propertyErrors)
                    {
                        validationErrors.Add(GetFriendlyErrorMessage(propertyName, error.ErrorMessage));
                    }
                }

                var result = new
                {
                    message = "Validation failed",
                    errors = validationErrors.ToArray()
                };

                context.Result = new BadRequestObjectResult(result);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }

        private string GetFriendlyErrorMessage(string propertyName, string errorMessage)
        {
            return (propertyName, errorMessage) switch
            {
                (_, var message) when message.Contains("The JSON value could not be converted to Car.AuctionSystem.Domain.Entities.Enum.VehicleType.")
                    => "Invalid vehicle type. Allowed values: Sedan, Hatchback, SUV, Truck.",

                (string field, var message) when field == "$.loadCapacity" && message.Contains("$.loadCapacity")
                    => "Load Capacity must be a number.",

                (string field, var message) when field == "$.year" && message.Contains("$.year")
                    => "Year must be a number.",

                (string field, var message) when field == "$.model" && message.Contains("The JSON value could not be converted to System.String")
                    => "Model must be text (string type).",

                (string field, var message) when field == "$.manufacturer" && message.Contains("The JSON value could not be converted to System.String")
                    => "Manufacturer must be text (string type).",

                (string field, var message) when field == "$.startingBid" && message.Contains("startingBid")
                    => "Starting bid must be a number.",

                (string field, var message) when field == "$.numberOfDoors" && message.Contains("numberOfDoors")
                    => "Number of doors must be a number.",

                (string field, var message) when field == "$.numberOfSeats" && message.Contains("numberOfSeats")
                => "Number of seats must be a number.",

                _ => $"{propertyName}: {errorMessage}"
            };
        }
    }
}
