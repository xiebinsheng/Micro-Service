using BaseService.Systems.AuditLoggingManagement.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BaseService.Systems.AuditLoggingManagement
{
    public interface IAuditLoggingAppService : IApplicationService
    {
        /// <summary>
        /// 根据ID获取审计日志信息
        /// 2021-04-30 Create by Sean
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AuditLogDto> GetAsync(Guid id);

        /// <summary>
        /// 获取审计日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogsInput input);
    }
}
