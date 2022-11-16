using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;

    public LoggingBehaviour(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        var requestObject = request;
        string userName = string.Empty;

        if (!string.IsNullOrEmpty(""))
        {
            userName = await Task.Run(() => "", cancellationToken);
        }

        _logger.LogInformation("Talkative Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, "", userName, request);
    }
}