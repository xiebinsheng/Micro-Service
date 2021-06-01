using System;
using System.Collections.Generic;
using System.Text;
using TestService.Enum;

namespace TestService.TestEntities.BillManagement
{
    public class GetBillMemberListDto
    {
        public long Id { get; set; }

        public string MemberName { get; set; }

        public string MemberEName { get; set; }

        public BillMemberTypeEnum MemberType { get; set; }

        public string MemberTypeDesc { get; set; }

        public string Comments { get; set; }
    }
}
