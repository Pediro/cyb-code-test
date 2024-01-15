using angular_cyb_code_test.Exceptions;
using System.Net;
using System.Text.Json;

namespace angular_cyb_code_test.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);

                var response = context.Response;
                response.ContentType = "application/json";
                string message;

                switch (exception)
                {
                    case ArgumentException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        message = exception.Message;
                        break;
                    case EntityNotFoundException ex:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        message = ex.Message; //Custom written message therefore will not return sensitive data to user
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        message = "Internal Server Error";
                        break;
                }

                var result = JsonSerializer.Serialize(new { message });
                await response.WriteAsync(result);
            }
        }
    }
}
