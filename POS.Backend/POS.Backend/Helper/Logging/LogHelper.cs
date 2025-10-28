using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace POS.Backend.Helper.Logging
{
    public class LogHelper
    {
        public readonly ILogger<LogHelper> logger;

        public LogHelper(ILogger<LogHelper> logger)
        {
            this.logger = logger;
        }

        public void LogRequestResponse(object request, object response, [CallerMemberName] string methodName ="", [CallerFilePath] string callerFilePath = "")
        {
            var className = Path.GetFileNameWithoutExtension(callerFilePath);
            var classAndMethod = $"{className}.{methodName}";

            this.logger.LogInformation("Request\nClass & Method : {classAndMethod}, \nRequest : {request}\n", classAndMethod, JsonConvert.SerializeObject(request));
            this.logger.LogInformation("Response\nClass & Method : {classAndMethod}, \nResponse : {request}\n", classAndMethod, JsonConvert.SerializeObject(response));
        }

        public void LogException(object request, Exception ex, [CallerMemberName] string methodName = "", [CallerFilePath] string callerFilePath = "")
        {
            var className = Path.GetFileNameWithoutExtension(callerFilePath);
            var classAndMethod = $"{className}.{methodName}";

            this.logger.LogInformation("Exception Occured\nClass & Method : {classAndMethod},\nRequest : {request} ,\nException : {ex}\n", classAndMethod, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ex));
        }
    }
}
