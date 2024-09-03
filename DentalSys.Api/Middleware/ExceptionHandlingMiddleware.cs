using DentalSys.Api.Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DentalSys.Api.Middleware
{
    public class ExceptionHandlingMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { code = "Server.Error", description = ex.Message }));
                //var error = new Error("Server.Error", ex.Message, 500);

                //await context.Response.WriteAsync(JsonConvert.SerializeObject(error, new JsonSerializerSettings
                //{
                //	ContractResolver = new DefaultContractResolver
                //	{
                //		NamingStrategy = new CamelCaseNamingStrategy()
                //	}
                //}));
            }
        }
    }
}
