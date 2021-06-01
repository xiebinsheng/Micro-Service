using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.Enum;
using Volo.Abp.Application.Dtos;

namespace TestService.TestEntities.BillManagement
{
    /// <summary>
    /// 账单成员
    /// </summary>
    public class BillMemberDto : EntityDto<long>
    {
        public string MemberName { get; set; }

        public string MemberEName { get; set; }

        public BillMemberTypeEnum MemberType { get; set; }

        public string Comments { get; set; }
    }
}
