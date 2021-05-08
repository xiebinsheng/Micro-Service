using BaseService.Systems.AuditLoggingManagement.Dto;
using BaseService.Permissions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.AuditLogging;
using Volo.Abp;
using Microsoft.Extensions.Localization;
using BaseService.Localization;

namespace BaseService.Systems.AuditLoggingManagement
{
    /// <summary>
    /// 审计日志服务实现类
    /// 2021-04-30 create by sean
    /// </summary>
    //[Authorize(BaseServicePermissions.AuditLogging.Default)]
    public class AuditLoggingAppService : ApplicationService, IAuditLoggingAppService
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IStringLocalizer<BaseServiceResource> _localizer;

        public AuditLoggingAppService(
            IAuditLogRepository auditLogRepository, 
            IStringLocalizer<BaseServiceResource> localizer)
        {
            _auditLogRepository = auditLogRepository;
            _localizer = localizer;
        }

        /// <summary>
        /// 根据ID获取审计日志信息
        /// 2021-04-30 create by sean
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<AuditLogDto> GetAsync(Guid id)
        {
            var existsAuditLog = await _auditLogRepository.FindAsync(id);
            if (existsAuditLog == null)
            {
                throw new UserFriendlyException(_localizer["AuditLogNotExistsException"]);
            }

            return ObjectMapper.Map<AuditLog, AuditLogDto>(existsAuditLog);
        }

        /// <summary>
        /// 根据入参获取审计日志信息
        /// 2021-04-30 create by sean
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<AuditLogDto>> GetListAsync(GetAuditLogsInput input)
        {
            var count = await _auditLogRepository.GetCountAsync(null, null, input.HttpMethod, input.Url,
                input.UserName, input.ApplicationName, input.CorrelationId, input.MaxExecutionDuration,
                input.MinExecutionDuration, input.HasException, input.HttpStatusCode);

            var list = await _auditLogRepository.GetListAsync(input.Sorting,
                input.MaxResultCount, input.SkipCount, null, null, input.HttpMethod, input.Url,
                input.UserName, input.ApplicationName, input.CorrelationId, input.MaxExecutionDuration,
                input.MinExecutionDuration, input.HasException, input.HttpStatusCode, true);

            return new PagedResultDto<AuditLogDto>(
                count,
                ObjectMapper.Map<List<AuditLog>, List<AuditLogDto>>(list)
            );
        }
    }
}
