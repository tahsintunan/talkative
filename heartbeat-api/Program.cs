using heartbeat_api.Interfaces;
using heartbeat_api.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IHeartbeatService, HeartbeatService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(_ =>
    ConnectionMultiplexer.Connect(new ConfigurationOptions()
    {
        Password = builder.Configuration["Redis:Password"],
        EndPoints = { builder.Configuration["Redis:ConnectionString"] },
        Ssl = false,
        AbortOnConnectFail = false,
        ConnectTimeout = 30000,
        ConnectRetry = 3,
        KeepAlive = 30
    }));

builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy", corsPolicyBuilder =>
    {
        corsPolicyBuilder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

