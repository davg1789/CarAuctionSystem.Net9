namespace Car.AuctionSystem.Api.Middleware
{
    public static class ExceptionMiddlewareRegistration
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
