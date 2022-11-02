using Application.Common.Interface;
using Application.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace server.Filters
{
    public class BlockActionFilter : IAsyncActionFilter
    {
        private readonly IBlockFilter _blockFilter;

        public BlockActionFilter(IBlockFilter blockFilter)
        {
            _blockFilter = blockFilter;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            var resultContext = await next();
            var value = (resultContext.Result as ObjectResult)?.Value;
            if (value == null)
                return;
            var type = value.GetType();

            if (value.GetType().FullName == typeof(List<UserVm>).FullName)
            {
                IList<UserVm>? result = await _blockFilter.GetFilteredUsers(
                    value as List<UserVm>,
                    resultContext.HttpContext.Items["User"]!.ToString()!
                );
                resultContext.Result = await Task.FromResult<IActionResult>(
                    new OkObjectResult(result)
                );
            }
            else if (value.GetType().FullName == typeof(List<TweetVm>).FullName)
            {
                var result = await _blockFilter.GetFilteredTweets(
                    value as List<TweetVm>,
                    resultContext.HttpContext.Items["User"]!.ToString()!
                );
                resultContext.Result = await Task.FromResult<IActionResult>(
                    new OkObjectResult(result)
                );
            }
            else if (value.GetType().FullName == typeof(List<CommentVm>).FullName)
            {
                var result = await _blockFilter.GetFilteredComments(
                    value as List<CommentVm?>,
                    resultContext.HttpContext.Items["User"]!.ToString()!
                );
                resultContext.Result = await Task.FromResult<IActionResult>(
                    new OkObjectResult(result)
                );
            }
        }
    }
}