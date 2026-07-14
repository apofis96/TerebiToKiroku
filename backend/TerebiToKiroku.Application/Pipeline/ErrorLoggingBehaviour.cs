using Mediator;
using Microsoft.Extensions.Logging;

namespace TerebiToKiroku.Application.Pipeline
{
    public sealed class ErrorLoggingBehaviour<TMessage, TResponse>(ILogger<ErrorLoggingBehaviour<TMessage, TResponse>> logger) : MessageExceptionHandler<TMessage, TResponse>
    where TMessage : notnull, IMessage
    {
        private readonly ILogger<ErrorLoggingBehaviour<TMessage, TResponse>> _logger = logger;

        protected override ValueTask<ExceptionHandlingResult<TResponse>> Handle(
            TMessage message,
            Exception exception,
            CancellationToken cancellationToken
        )
        {
            _logger.LogError(exception, "Error handling message of type {messageType}", message.GetType().Name);
            return NotHandled;
        }
    }
}
