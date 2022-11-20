using Application;
using Application.Common.Interface;
using FluentValidation.AspNetCore;
using Infrastructure;
using server.Filters;
using server.Hub;
using server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options => { options.Filters.Add<BlockActionFilter>(); });

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });
builder.Services.AddTransient<INotificationHub, NotificationHub>();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddCors(o =>
{
    o.AddPolicy(
        "CorsPolicy",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder
                .WithOrigins("http://kernel-panic.learnathon.net/web/")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notificationhub");

app.Run();