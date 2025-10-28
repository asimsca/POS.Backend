using POS.Backend.DTO;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace POS.Backend.Helper.ActionFilter
{
    public class ValidateModelAttributeFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                //var errorsObjects = context.ModelState
                //    .Where(x => x.Value.Errors.Count > 0)
                //    .ToDictionary(
                //        kvp => kvp.Key,
                //        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                //    );

                // Flatten all validation errors into a single comma-separated string
                var errorMessages = context.ModelState
                    .SelectMany(x => x.Value.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var allErrors = string.Join(", ", errorMessages);

                // Create a BaseResponse with a dictionary of errors as the Data
                var response = new BaseResponse<string>
                {
                    IsSuccess = false,
                    ResponseCode = HttpStatusCode.BadRequest,
                    Message = "Validation failed",
                    Data = allErrors // Set errors as the Data
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)HttpStatusCode.BadRequest
                };
            }
        }
    }
}
