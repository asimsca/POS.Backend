using Newtonsoft.Json;

namespace POS.Backend.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var path = context.Request.Path.Value;

                //skipp swagger and statuc file requests 
                if(path.Contains("swagger") || path.Contains(".js") || path.Contains(".css") || path.Contains(".png"))
                {
                    await _next(context);
                    return;
                }

                //logging incomming API Request
                _logger.LogInformation("---------------------------------------------------------------------------------------------------------------------------------------");
                context.Request.EnableBuffering();
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;

                var endPoint = context.GetEndpoint();
                var actionDescriptor = endPoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
                var controllerName = actionDescriptor?.ControllerName ?? "UnKnownController";
                var actionName = actionDescriptor?.ActionName ?? "UnKnownAction";

                _logger.LogInformation("Incomming API Request\n" +
                    "Http Method : {Method}\nPath : {Path}\nController: {Controller}\nAPI : {action}\nRequestBody : {Body}\n",
                    context.Request.Method,
                    context.Request.Path,
                    controllerName,
                    actionName,
                    requestBody
                    );

                //copy oiginal response stream
                var originalBodyStream = context.Response.Body;

                using var responseBody = new MemoryStream();
                context.Response.Body = responseBody;

                await _next(context); //continue pipeline


                //logging outgoing API Response
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek (0, SeekOrigin.Begin);

                _logger.LogInformation("Outgoing API Response\n" +
                    "Http Status : {Status}\nController: {Controller}\nAPI : {action}\nResponseBody : {Body}\n",
                    context.Response.StatusCode,
                    controllerName,
                    actionName,
                    responseText
                    );

                //copy back to original stream
                await responseBody.CopyToAsync(originalBodyStream);


            }
            catch (Exception ex)
            {
                //_logger.LogInformation(JsonConvert.SerializeObject(ex));
                _logger.LogInformation("Exception Occured(Unhanedled)\nException : {ex}\n", JsonConvert.SerializeObject(ex));
            }
        }
    }
}