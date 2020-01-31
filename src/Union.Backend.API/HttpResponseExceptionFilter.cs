using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Union.Backend.Service.Exceptions;

namespace Union.Backend.API
{
    public class HttpResponseExceptionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null) return;
            if (context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult(exception.Message)
                {
                    StatusCode = exception.Status,
                };
            }
            else
            {
                context.Result = new ObjectResult(context.Exception.ConvertToString())
                {
                    StatusCode = 500,
                };
            }
            context.ExceptionHandled = true;
        }
    }
}
