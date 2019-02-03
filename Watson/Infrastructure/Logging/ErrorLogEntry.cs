using System;
using System.Text;
using Nancy;

namespace Watson.Infrastructure.Logging
{
    public class ErrorLogEntry : LogEntry
    {
        public HttpStatusCode? HttpResponse { get; set; }
        public string ErrorType { get; internal set; }
        public string ErrorMessage { get; internal set; }
        public string ErrorStackTrace { get; internal set; }
        public string ErrorDetails { get; internal set; }

        public ErrorLogEntry(Exception ex, NancyContext context = null)
        {
            ErrorType = ex.GetType().Name;
            ErrorMessage = ex.Message;
            ErrorStackTrace = ex.StackTrace;
            ErrorDetails = BuildErrorMessage(ex);

            if (context != null) {
                User = context.CurrentUser?.Identity.Name ?? "anonymous";
                RequestProtocolVersion = context.Request.ProtocolVersion;
                RequestPath = context.Request.Path;
                RequestMethod = context.Request.Method;
                RequestBody = context.Request.Form.ToDictionary();
                RequestQuery = context.Request.Query.ToDictionary();
                HttpResponse = context.Response?.StatusCode;
            }
        }

        private string BuildErrorMessage(Exception ex)
        {
            var builder = new StringBuilder();
            while(ex != null) {
                builder.Append(ex.Message);
                builder.Append(Environment.NewLine);
                builder.Append("-----------------------");
                builder.Append(Environment.NewLine);
                builder.Append(ex.StackTrace);
                builder.Append(Environment.NewLine);
                builder.Append("-----------------------");
                builder.Append(Environment.NewLine);
                ex = ex.InnerException;
            }
            return builder.ToString();
        }
    }
}   