using FluentValidation;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using server.Domain.Entities;
using server.Infrastructure.DbConfig;

namespace server.Application.Dto.ForgetPasswordDto
{
    public class ForgetPasswordDtoValidator : AbstractValidator<ForgetPasswordDto>
    {
        private readonly IMongoCollection<User>? _userCollection;
        public ForgetPasswordDtoValidator(IOptions<UserDatabaseConfig> userDatabaseConfig)
        {
            var mongoClient = new MongoClient(
            userDatabaseConfig.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                userDatabaseConfig.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<User>(
                userDatabaseConfig.Value.UserCollectionName);


            RuleFor(x => x.Email).NotEmpty().WithMessage("Email cannot be empty");
            //RuleFor(x=>x.Email).MustAsync(async (email, cancellation) =>
            //{
            //    var user = await _userCollection.Find(user => user.Email == email).FirstOrDefaultAsync();
            //    return user!=null;
            //}).WithMessage("User with current email doesn't exist");
        }
    }
}
