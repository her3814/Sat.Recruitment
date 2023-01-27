using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sat.Recruitment.Application;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyApi.Filters
{
    public class ValidateModelFilter : ActionFilterAttribute
    {
        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    if (context.ModelState.IsValid == false)
        //    {
        //        context.HttpContext.Response.StatusCode = 400;
        //        var result = new AppResult();
        //        ModelStateDictionary validationResults = context.ModelState;

        //        if (validationResults.Count>0)
        //        {
        //            validationResults.Values.ToList().ForEach(x =>
        //            {
        //                result.AddInputDataError(string.Join(",",x.Errors.ToList().Select(e => e.ErrorMessage)));
        //            });
        //            //result.AddInputDataErrors();
        //            //return result;
        //        }
        //        context.HttpContext.Response.WriteAsJsonAsync(result);
        //    }
        //}
    }
}
