using System.Reflection;
using Application.Common.Interface;
using Application.Common.Mapper;
using Application.Common.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient<IBsonDocumentMapper<UserVm>, UserBsonDocumentMapper>();
        services.AddTransient<IBsonDocumentMapper<TweetVm?>, TweetBsonDocumentMapper>();
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}