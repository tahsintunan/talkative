using Application.Interface;
using Application.Mapper;
using Application.ViewModels;
using FluentValidation.AspNetCore;
using Infrastructure.Services;
using MediatR;
using server.Application.Interface;
using server.Application.ViewModels;
using server.Hub;
using server.Infrastructure.DbConfig;
using server.Infrastructure.Services;
using server.Infrastructure.Services.HandlerService;
using server.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.Configure<UserDatabaseConfig>(
    builder.Configuration.GetSection("UserDatabaseConfig"));
builder.Services.Configure<MessageDatabaseConfig>(
    builder.Configuration.GetSection("MessageDatabaseConfig"));
builder.Services.Configure<TweetDatabaseConfig>(
    builder.Configuration.GetSection("TweetDatabaseConfig"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddSingleton<IBsonDocumentMapper<UserVm>, UserBsonDocumentMapper>();
builder.Services.AddSingleton<IBsonDocumentMapper<TweetVm>, TweetBsonDocumentMapper>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAuthService, AuthService>();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<ITweetService, TweetService>();
builder.Services.AddSingleton<IRetweetService, RetweetService>();
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
