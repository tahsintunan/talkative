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
            ProfilePicture = CheckIfDocumentExists(user, "profilePicture")
                ? user["profilePicture"].ToString()
                : null,
            CoverPicture = CheckIfDocumentExists(user, "coverPicture")
                ? user["coverPicture"].ToString()
                : null,
            Email = user.Contains("email") ? user["email"].ToString() : null,
            DateOfBirth = user.Contains("dateOfBirth")
                ? user["dateOfBirth"].ToUniversalTime()
                : null
        };
    }

    public bool CheckIfDocumentExists(BsonDocument document, string documentKey)
    {
        return document.Contains(documentKey) && document[documentKey].BsonType != BsonType.Null;
    }
}