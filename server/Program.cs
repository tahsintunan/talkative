using Application;
using Application.Common.Interface;
using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.DbConfig;
using server.Hub;
using server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.Configure<UserDatabaseConfig>(
    builder.Configuration.GetSection("UserDatabaseConfig")
);
builder.Services.Configure<MessageDatabaseConfig>(
    builder.Configuration.GetSection("MessageDatabaseConfig")
);
builder.Services.Configure<TweetDatabaseConfig>(
    builder.Configuration.GetSection("TweetDatabaseConfig")
);

builder.Services.Configure<CommentDatabaseConfig>(
    builder.Configuration.GetSection("CommentDatabaseConfig")
);

builder.Services.Configure<FollowerDatabaseConfig>(
    builder.Configuration.GetSection("FollowerDatabaseConfig")
);

builder.Services.Configure<NotificationDatabaseConfig>(
    builder.Configuration.GetSection("NotificationDatabaseConfig")
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

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
                .WithOrigins("http://localhost:4200")
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

app.UseMiddleware<AuthMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/notificationhub");

app.Run();
