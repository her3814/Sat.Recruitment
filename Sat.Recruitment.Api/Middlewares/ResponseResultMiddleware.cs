﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Sat.Recruitment.Application;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using static Sat.Recruitment.Application.AppError;

namespace Sat.Recruitment.Api.Filters
{
    public class ResponseResultMiddleware
    {
        private readonly RequestDelegate _next;
        public ResponseResultMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {

            Stream originalBody = context.Response.Body;
            AppResult appResult = null;
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await _next(context);

                    if (context.Response.ContentType != null && context.Response.ContentType.Contains("application/json"))
                    {
                        memStream.Position = 0;
                        string responseBody = new StreamReader(memStream).ReadToEnd();
                        var serializer = new DataContractJsonSerializer(typeof(AppResult));
                        appResult = JsonConvert.DeserializeObject<AppResult>(responseBody);

                        ModifyStatusCodeBasedOnAppResult(context, appResult);
                    }

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);

                }
            }
            finally
            {
                context.Response.Body = originalBody;
            }
        }

        private void ModifyStatusCodeBasedOnAppResult(HttpContext context, AppResult appResult)
        {
            if (appResult == null || appResult.IsSuccess)
            {
                return;
            }
            if (appResult.Errors.Any(err => err.Type == ErrorTypeEnum.InputDataError || err.Type == ErrorTypeEnum.BusinessError))
                context.Response.StatusCode = StatusCodes.Status400BadRequest;

            if (appResult.Errors.Any(err => err.Type == ErrorTypeEnum.InternalError))
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
}
