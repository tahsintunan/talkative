using Application.Common.Interface;
using Application.Common.Mapper;
using Application.Common.ViewModels;
using Infrastructure.Services;
using Infrastructure.Services.RMQHandlerService;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using INotification = Application.Common.Interface.INotification;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddTransient<IBsonDocumentMapper<UserVm>, UserBsonDocumentMapper>();
        services.AddTransient<IBsonDocumentMapper<TweetVm?>, TweetBsonDocumentMapper>();
        services.AddTransient<IUser, UserService>();
        services.AddTransient<IAuth, AuthService>();
        services.AddTransient<IComment, CommentService>();
        services.AddTransient<IFollow, FollowService>();
        services.AddTransient<ITweet, TweetService>();
        services.AddTransient<IRetweet, RetweetService>();
        services.AddTransient<IBlockFilter, BlockFilterService>();
        services.AddTransient<IRabbitmq, RabbitmqService>();
        services.AddTransient<INotification, NotificationService>();
        services.AddHostedService<DbNotificationHandlerService>();
        services.AddHostedService<RtNotificationHandlerService>();
        return services;
    }
}
