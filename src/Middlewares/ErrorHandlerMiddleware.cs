namespace FusionTech.src.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CustomException ex)
            {
                context.Response.StatusCode = ex.StatusCode;
                context.Response.ContentType = "application/json";

                // response: status + message
                var response = new
                {
                    ex.StatusCode,
                    ex.Message,
                    // fields
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
