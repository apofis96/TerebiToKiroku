namespace TerebiToKiroku.WebAPI.Configuration
{
    internal class CorrelationMiddleware
    {
        internal const string CorrelationHeaderKey = "CorrelationId";

        private readonly RequestDelegate _next;

        public CorrelationMiddleware(
            RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = Guid.NewGuid();

            context.Request?.Headers.Append(CorrelationHeaderKey, correlationId.ToString());

            await this._next.Invoke(context);
        }
    }
}