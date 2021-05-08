using System.Net;
using Volo.Abp.Application.Dtos;

namespace BaseService.Systems.AuditLoggingManagement.Dto
{
    /// <summary>
    /// 获取审计日志入参
    /// 2021-04-30 Create by Sean
    /// </summary>
    public class GetAuditLogsInput : PagedResultRequestDto, IPagedAndSortedResultRequest, IPagedResultRequest, ILimitedResultRequest
    {
        public string Sorting { get; set; }

        public string Url { get; set; }

        public string UserName { get; set; }

        public string ApplicationName { get; set; }
        public string CorrelationId { get; set; }

        public string HttpMethod { get; set; }

        public HttpStatusCode? HttpStatusCode { get; set; }

        public int? MaxExecutionDuration { get; set; }


        public int? MinExecutionDuration { get; set; }

        public bool HasException { get; set; }
    }
}
