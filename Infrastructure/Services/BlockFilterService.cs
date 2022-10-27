using Application.Common.Class;
using Application.Common.Interface;
using Application.Common.ViewModels;

namespace Infrastructure.Services;

public class BlockFilterService : IBlockFilter
{
    private readonly IUser _userService;

    public BlockFilterService(IUser userService)
    {
        _userService = userService;
    }

    public async Task<List<UserVm>> GetFilteredUsers(IEnumerable<UserVm> userVms, string userId)
    {
        var blockedUserIds = await GetBlockedUserIds(userId);
        return userVms.Where(b => !IsBlocked(b, blockedUserIds)).ToList();
    }

    public async Task<List<CommentVm>> GetFilteredComments(IEnumerable<CommentVm> commentVms, string userId)
    {
        var blockedUserIds = await GetBlockedUserIds(userId);
        return commentVms.Where(b => !IsBlocked(b, blockedUserIds)).ToList();
    }

    public async Task<List<TweetVm>> GetFilteredTweets(IEnumerable<TweetVm> tweetVms, string userId)
    {
        var blockedUserIds = await GetBlockedUserIds(userId);
        return tweetVms.Where(b => !IsBlocked(b, blockedUserIds)).ToList();
    }

    public bool IsBlocked(Blockable blockable, IReadOnlySet<string> blockedUserIds)
    {
        if (blockable is TweetVm { OriginalTweet: { } } vm)
            return blockedUserIds.Contains(vm.UserId!) || IsBlocked(vm.OriginalTweet, blockedUserIds);
        return blockedUserIds.Contains(blockable.UserId!);
    }

    public async Task<HashSet<string>> GetBlockedUserIds(string userId)
    {
        var blockedUserIds = new HashSet<string>();
        var user = await _userService.GetUserById(userId);
        if (user == null) return blockedUserIds;
        var blockedBy = user.BlockedBy;
        if (blockedBy == null) return blockedUserIds;

        foreach (var id in blockedBy) blockedUserIds.Add(id!);
        return blockedUserIds;
    }
}