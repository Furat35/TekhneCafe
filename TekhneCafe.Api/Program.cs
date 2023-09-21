using Microsoft.AspNetCore.Diagnostics;
using Serilog.Context;
using System.Text.Json;
using TekhneCafe.Api.Extensions;
using TekhneCafe.Business.Extensions;
using TekhneCafe.Core.Exceptions;
using TekhneCafe.Core.Extensions;
using TekhneCafe.DataAccess.Extensions;
using TekneCafe.SignalR.Extensions;
using TekneCafe.SignalR.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices(builder);
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddBusinessServices(builder.Configuration);
builder.Services.AddSignalRServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(_ =>
    {
        _.SwaggerEndpoint("/swagger/v1/swagger.json", "TekhneCafe API v1");
    });
}

#region ExceptionHandler Configuration
app.UseExceptionHandler(
    options =>
    {
        options.Run(async context =>
        {
            context.Response.ContentType = "application/json";
            var exceptionObject = context.Features.Get<IExceptionHandlerFeature>();

            if (exceptionObject != null)
            {
                context.Response.StatusCode = exceptionObject.Error switch
                {
                    BadRequestException ex => StatusCodes.Status400BadRequest,
                    NotFoundException ex => StatusCodes.Status404NotFound,
                    ForbiddenException ex => StatusCodes.Status403Forbidden,
                    InternalServerErrorException exception => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };
                var errorMessage = $"{exceptionObject.Error.Message}";
                await context.Response
                    .WriteAsync(JsonSerializer.Serialize(new
                    {
                        context.Response.StatusCode,
                        exceptionObject.Error.Message
                    }))
                    .ConfigureAwait(false);
            }
        });
    }
);
#endregion

//app.UseSerilogRequestLogging();
app.UseCors("_myAllowSpecificOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseSentryTracing();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.User?.Identity?.IsAuthenticated != null)
        LogContext.PushProperty("UserId", context.User.ActiveUserId());

    await next();
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<OrderNoficationHub>("/orderNotificationHub");
});

app.Run();
