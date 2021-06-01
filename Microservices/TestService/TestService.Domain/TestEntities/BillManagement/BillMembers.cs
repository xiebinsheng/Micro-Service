using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace TestService.TestEntities.BillManagement
{
    /// <summary>
    /// 账单成员
    /// </summary>
    public class BillMembers : Entity<long>
    {
        public string MemberName { get; set; }

        public string MemberEName { get; set; }

        public int MemberType { get; set; }

        public string Comments { get; set; }
    }
}
