using System;
using System.Collections.Generic;
using System.Text;

namespace TestService.TestEntities.BillManagement
{
    public class CreateBillMemberInput
    {
        public string MemberName { get; set; }

        public string MemberEName { get; set; }

        public int MemberType { get; set; }

        public string Comments { get; set; }
    }
}
