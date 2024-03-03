using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using SMSHub.APIs.Errors;
using SMSHub.APIs.Helpers;
using SMSHub.Core.Repositories;
using SMSHub.Repository;
using SMSHub.Core.Services;

namespace SMSHub.APIs.Extensions
{
    //when to create extension methods?? if i have some related services
    public static class ApplicationServicesExtension 
    {                                         
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services)
        {
            Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Administrator"));
            });
            // Add CORS services
            Services.AddCors(options =>
            {
                options.AddPolicy("AllowMyFrontend",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000") // Allow only this origin
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            Services.AddHttpClient();
            Services.AddSingleton<ISmsService, SmsService>();

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
                                                                                                                                                                                   
            Services.AddAutoMapper(typeof(MappingProfiles));
            //handle validation error
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(p => p.ErrorMessage)
                    .ToArray();

                    var ValidationErrorResponse = new ApiValidationError()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });

            return Services;//b3d ma a add inside the container, return it after addition of services inside container
            //w 34an lma a nest haga y3rf ykml 3leh 
            //y3ni lw msln h3ml kda >> builder.Services.AddApplicationServices().AddScoped....
            //lkn msln lw 3mlt kda msh hykml >> builder.Services.AddControllers().Addscoped..., l2nha msh btreturn services container
        }
    }
}
