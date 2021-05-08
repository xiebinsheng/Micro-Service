using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;

namespace BaseService.Controllers
{
    [Route("HealthCheck")]
    [ApiController]
    [DisableAuditing]
    public class HealthCheckController : AbpController
    {
        public HealthCheckController(ILogger<HealthCheckController> logger)
        {
        }

        [HttpGet]
        public ActionResult Get()
        {
            var IPAddress = HttpContext.Connection.LocalIpAddress.MapToIPv4();
            var port = HttpContext.Connection.LocalPort;
            var time = DateTime.Now.ToString("HH_mm_ss_fff");

            return Ok(HttpContext.Connection.LocalIpAddress.MapToIPv4() + ":" + HttpContext.Connection.LocalPort+ DateTime.Now.ToString("HH_mm_ss_fff")+"___BaseService 连接正常");
        }
    }
}
