using Application.Common.Class;
using Application.Common.Interface;
using Application.Common.ViewModels;
using FakeItEasy;
using Infrastructure.Services;

namespace UnitTest;

public class BlockFilterTest
{
    public static IEnumerable<object[]> IsBlocked_ShouldReturnCorrectBool_Data =>
        new List<object[]>
        {
            new object[]
            {
                new Blockable { UserId = "1" },
                new HashSet<string>(),
                false
            },
            new object[]
            {
                new Blockable { UserId = "1" },
                new HashSet<string> { "2", "3", "4" },
                false
            },
            new object[]
            {
                new Blockable { UserId = "3" },
                new HashSet<string> { "2", "3", "4" },
                true
            }
        };

    public static IEnumerable<object[]> GetBlockedUserIds_ShouldReturnUserIdsOfBlockedUsers_Data =>
        new List<object[]>
        {
            new object[]
            {
                new List<string> { "1", "2" },
                new List<string> { "3", "4" },
                false,
                new HashSet<string> { "1", "2", "3", "4" }
            },
            new object[]
            {
                null!,
                new List<string> { "3", "4" },
                false,
                new HashSet<string> { "3", "4" }
            },
            new object[]
            {
                new List<string> { "1", "2" },
                null!,
                false,
                new HashSet<string> { "1", "2" }
            },
            new object[] { new List<string>(), new List<string>(), false, new HashSet<string>() },
            new object[]
            {
                new List<string> { "1", "2" },
                new List<string> { "3", "4" },
                true,
                new HashSet<string>()
            }
        };

    [Theory]
    [MemberData(nameof(IsBlocked_ShouldReturnCorrectBool_Data))]
    public void IsBlocked_ShouldReturnCorrectBool(
        Blockable blockable,
        IReadOnlySet<string> blockedUserIds,
        bool expected
    )
    {
        // Arrange
        var userService = A.Fake<IUser>();
        var blockFilterService = new BlockFilterService(userService);

        // Act
        var actual = blockFilterService.IsBlocked(blockable, blockedUserIds);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetBlockedUserIds_ShouldReturnUserIdsOfBlockedUsers_Data))]
    public async void GetBlockedUserIds_ShouldReturnUserIdsOfBlockedUsers(
        IList<string> blocked,
        IList<string> blockedBy,
        bool isAdmin,
        HashSet<string> expected
    )
    {
        // Arrange
        var userService = A.Fake<IUser>();
        var blockFilterService = new BlockFilterService(userService);

        var dummyUserId = "0";
        var dummyUserVm = A.Fake<UserVm>();
        dummyUserVm.Blocked = blocked!;
        dummyUserVm.BlockedBy = blockedBy!;
        dummyUserVm.IsAdmin = isAdmin;
        A.CallTo(() => userService.GetUserById(dummyUserId)).Returns(dummyUserVm);

        // Act
        var actual = await blockFilterService.GetBlockedUserIds(dummyUserId);

        // Assert
        Assert.Equal(expected, actual);
    }
}
