using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TestService.Attributes;

namespace TestService.Enum
{
    public enum BillMemberTypeEnum
    {
        [Description("BillMemberTypeEnum:Family")]
        Family=1,
        [Description("BillMemberTypeEnum:Friend")]
        Friend =2
    }
}
