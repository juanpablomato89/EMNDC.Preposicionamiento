
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using EMNDC.Preposicionamiento.BasicResponses;
using EMNDC.Preposicionamiento.Exceptions;

namespace EMNDC.Preposicionamiento.Middlewares
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _scopeFactory;

        public ErrorWrappingMiddleware(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            _next = next;
            _scopeFactory = scopeFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            string message = "";
            int customStatusCode = 0;

            try
            {
                await _next.Invoke(context);
            }
            catch (CustomBaseException ex)
            {
                message = ex.CustomMessage;
                customStatusCode = ex.CustomCode;
                context.Response.StatusCode = ex.HttpCode;
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                message = ex.Message;
            }

            if (!context.Response.HasStarted && context.Response.StatusCode != 204)
            {
                context.Response.ContentType = "application/json";

                using var scope = _scopeFactory.CreateScope();
                var localizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<ErrorWrappingMiddleware>>();

                var response = new ApiResponse(
                    context.Response.StatusCode,
                    message ?? localizer.GetString(context.Response.StatusCode.ToString()),
                    customStatusCode
                );

                var json = JsonConvert.SerializeObject(response, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                await context.Response.WriteAsync(json);
            }
        }
    }
}