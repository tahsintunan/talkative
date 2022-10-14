using Application;
using Application.Common.Interface;
using Application.Common.Mapper;
using Application.Common.ViewModels;
using FluentValidation.AspNetCore;
using Infrastructure.DbConfig;
using Infrastructure.Services;
using Infrastructure.Services.HandlerService;
using MediatR;
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

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddApplicationServices();
builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddTransient<IBsonDocumentMapper<UserVm>, UserBsonDocumentMapper>();
builder.Services.AddTransient<IBsonDocumentMapper<TweetVm?>, TweetBsonDocumentMapper>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<IComment, CommentService>();
builder.Services.AddTransient<IChatService, ChatService>();
builder.Services.AddTransient<ITweetService, TweetService>();
builder.Services.AddTransient<IRetweetService, RetweetService>();
builder.Services.AddTransient<IRabbitmqService, RabbitmqService>();
builder.Services.AddTransient<IChatHub, ChatHub>();
builder.Services.AddHostedService<DbHandlerService>();
builder.Services.AddHostedService<TextDeliveryHandlerService>();

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

app.MapHub<ChatHub>("/chathub");

app.Run();
