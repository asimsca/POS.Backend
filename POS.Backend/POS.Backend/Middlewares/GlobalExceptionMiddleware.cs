using System.Net;
using System.Text;
using POS.Backend.DTO;
using POS.Backend.Helper.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using POS.Backend.DTO.Response.Auth;

namespace POS.Backend.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LogHelper logHelper;

        /// <summary>
        /// Middleware for logging unhandled Exceptions. This middleware is called when an exception is not caught anywhere in the code.
        /// </summary>
        /// <param name="next">Next middleware in the pipeline</param>
        /// <param name="logHelper">Log helper to log the exception details</param>
        public GlobalExceptionMiddleware(RequestDelegate next, LogHelper logHelper)
        {
            _next = next;
            this.logHelper = logHelper;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Continue the request processing
            }
            catch (Exception ex)
            {
                // Capture class and method name where the exception occurred
                var classAndMethod = GetClassAndMethodName();

                // Read the request body (for logging purposes)
                var requestBody = await ReadRequestBodyAsync(context);
                var deserializedBody = JsonConvert.DeserializeObject<object>(requestBody);



                // Log the exception with detailed info (class, method, exception, and request)
                logHelper.LogException(deserializedBody, ex, classAndMethod);

                // Return a detailed error response to the client
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new BaseResponse<object>()
                {
                    IsSuccess = false,
                    ResponseCode = HttpStatusCode.InternalServerError,
                    Message = $"An unexpected error occurred in {classAndMethod}.", // Include class and method name in message
                    Data = null
                };

                var jsonResponse = JsonConvert.SerializeObject(response);
                await context.Response.WriteAsync(jsonResponse);

                //await context.Response.WriteAsJsonAsync(response);
            }
        }

        private async Task<string> ReadRequestBodyAsync(HttpContext context)
        {
            // Enable request body buffering to allow reading multiple times
            context.Request.EnableBuffering();
            context.Request.Body.Position = 0;

            using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            context.Request.Body.Position = 0; // Reset the position for downstream middlewares
            return body;
        }

        private string GetClassAndMethodName([CallerMemberName] string methodName = "", [CallerFilePath] string callerFilePath = "")
        {
            // Extract class name from the file path
            var className = Path.GetFileNameWithoutExtension(callerFilePath);
            return $"{className}.{methodName}"; // Format as "ClassName.MethodName"
        }
    }
}
