using Application.Common.ViewModels;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mapper;

public class NotificationProfile : Profile
{
    public NotificationProfile()
    {
        CreateMap<Notification, NotificationVm>().ReverseMap();
    }
}