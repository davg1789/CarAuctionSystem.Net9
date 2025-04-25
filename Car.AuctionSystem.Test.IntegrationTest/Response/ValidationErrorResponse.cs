namespace Car.AuctionSystem.Test.IntegrationTest.Response
{
    public class ValidationErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string[] Errors { get; set; } = [];
    }
}
