using E_Learning_API.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning_API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Configure validation error response
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage)
                        .ToArray();

                    var errorResponse = new ApiValidationErrorResponse(errors);

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            return services;
        }
    }
}
