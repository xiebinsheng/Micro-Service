using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc.ExceptionHandling;
using Volo.Abp.DependencyInjection;
using Volo.Abp.ExceptionHandling;
using Volo.Abp.Http;
using Volo.Abp.Json;

namespace TestService.Host
{
    /// <summary>
    /// 自定义异常拦截器
    /// 参考ABP vNext异常拦截器设计AbpExceptionFilter
    /// </summary>
    public class CustomExceptionFilter : IAsyncExceptionFilter, ITransientDependency
    {
        public ILogger<AbpExceptionFilter> Logger { get; set; }

        private readonly IExceptionToErrorInfoConverter _errorInfoConverter;
        private readonly IHttpExceptionStatusCodeFinder _statusCodeFinder;
        private readonly IJsonSerializer _jsonSerializer;

        public CustomExceptionFilter(
            IExceptionToErrorInfoConverter errorInfoConverter,
            IHttpExceptionStatusCodeFinder statusCodeFinder,
            IJsonSerializer jsonSerializer)
        {
            _errorInfoConverter = errorInfoConverter;
            _statusCodeFinder = statusCodeFinder;
            _jsonSerializer = jsonSerializer;

            Logger = NullLogger<AbpExceptionFilter>.Instance;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }

            await HandleAndWrapException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            // 判断当前请求是否是一个控制器行为，是则返回 true。
            // 第二个条件会判断当前的接口返回值是 IActionResult、JsonResult、ObjectResult、NoContentResult 的一种，是则返回 true。
            // 这里则会忽略不是控制器的方法，控制器类型不是上述类型任意一种也会被忽略。
            if (context.ActionDescriptor.IsControllerAction() &&
                context.ActionDescriptor.HasObjectResult())
            {
                return true;
            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }

        protected virtual async Task HandleAndWrapException(ExceptionContext context)
        {
            context.HttpContext.Response.Headers.Add(AbpHttpConsts.AbpErrorFormat, "true");
            context.HttpContext.Response.StatusCode = (int)_statusCodeFinder.GetStatusCode(context.HttpContext, context.Exception);

            // 如果需要显示异常明细，需要将includeSensitiveDetails设置为true
            var TestService = _errorInfoConverter.Convert(context.Exception, false);

            context.Result = new ObjectResult(new RemoteServiceErrorResponse(TestService));

            var logLevel = context.Exception.GetLogLevel();

            var TestServiceBuilder = new StringBuilder();
            TestServiceBuilder.AppendLine($"---------- {nameof(TestService)} ----------");
            TestServiceBuilder.AppendLine(_jsonSerializer.Serialize(TestService, indented: true));
            Logger.LogWithLevel(logLevel, TestServiceBuilder.ToString());

            Logger.LogException(context.Exception, logLevel);

            await context.HttpContext
                .RequestServices
                .GetRequiredService<IExceptionNotifier>()
                .NotifyAsync(
                    new ExceptionNotificationContext(context.Exception)
                );

            context.Exception = null;
        }
    }
}