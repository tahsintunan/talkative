using Application.Common.Interface;
using Application.Common.ViewModels;
using MongoDB.Bson;

namespace Application.Common.Mapper;

public class UserBsonDocumentMapper : IBsonDocumentMapper<UserVm>
{
    public UserVm map(BsonDocument user)
    {
        return new UserVm
        {
            UserId = user.Contains("_id") ? user["_id"].ToString() : null,
            Username = user.Contains("username") ? user["username"].ToString() : null,
            Email = user.Contains("email") ? user["email"].ToString() : null,
            DateOfBirth = user.Contains("dateOfBirth")
                ? user["dateOfBirth"].ToUniversalTime()
                : null
        };
    }
}