using FluentValidation.AspNetCore;
using server.Configs.DbConfig;
using server.Hub;
using server.Interface;
using server.Middlewares;
using server.Services;
using server.Services.HandlerServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.Configure<UserDatabaseConfig>(
    builder.Configuration.GetSection("UserDatabaseConfig"));
builder.Services.Configure<MessageDatabaseConfig>(
    builder.Configuration.GetSection("MessageDatabaseConfig"));

builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<IRabbitmqService, RabbitmqService>();
builder.Services.AddSingleton<IChatHub, ChatHub>();
builder.Services.AddHostedService<DbHandlerService>();
builder.Services.AddHostedService<TextDeliveryHandlerService>();


builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy", corsPolicyBuilder =>
    {
        corsPolicyBuilder
       .WithOrigins("http://localhost:4200")
       .AllowAnyMethod()
       .AllowAnyHeader().
       AllowCredentials();
    });
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

app.MapHub<ChatHub>("/chathub");

app.Run();
