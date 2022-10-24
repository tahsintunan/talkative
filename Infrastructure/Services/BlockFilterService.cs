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

    private static bool IsBlocked(Blockable blockable, IReadOnlySet<string> blockedUserIds)
    {
        if (blockable is TweetVm vm && (vm.IsRetweet || vm.IsQuoteRetweet))
        {
            return blockedUserIds.Contains(vm.UserId!)
                || blockedUserIds.Contains(vm.OriginalTweet!.UserId!);
        }
        return blockedUserIds.Contains(blockable.UserId!);
    }

    private async Task<HashSet<string>> GetBlockedUserIds(string userId)
    {
        var blockedUserIds = new HashSet<string>();
        var user = await _userService.GetUserById(userId);
        if (user == null)
        {
            return blockedUserIds;
        }
        var (blocked, blockedBy) = (user.Blocked, user.BlockedBy);

        if (blocked != null)
        {
            foreach (var id in blocked)
            {
                blockedUserIds.Add(id!);
            }
        }
        if (blockedBy != null)
        {
            foreach (var id in blockedBy)
            {
                blockedUserIds.Add(id!);
            }
        }
        return blockedUserIds;
    }
}
