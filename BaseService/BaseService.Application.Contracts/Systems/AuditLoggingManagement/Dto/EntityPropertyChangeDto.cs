using System;
using Volo.Abp.Application.Dtos;

namespace BaseService.Systems.AuditLoggingManagement.Dto
{
    /// <summary>
    /// 实体属性变更DTO
    /// 2021-04-30 Create by Sean
    /// </summary>
    public class EntityPropertyChangeDto : EntityDto<Guid>
    {
        public Guid? TenantId { get; set; }

        public Guid EntityChangeId { get; set; }

        public string NewValue { get; set; }

        public string OriginalValue { get; set; }

        public string PropertyName { get; set; }

        public string PropertyTypeFullName { get; set; }
    }
}
