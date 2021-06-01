using TestService.Domain.Shared.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace TestService.TestEntities.BillManagement
{
    public class GeBillMemberListInput: PagedAndSortedResultRequestDto
    {
        //[Required]
        //[StringLength(StringPropertyLength.MaxNameLength)]
        public string MemberName { get; set; }

        //[Required]
        //[StringLength(StringPropertyLength.MaxNameLength)]
        public string MemberEName { get; set; }

        //[Required]
        //[StringLength(StringPropertyLength.MaxNameLength)]
        public int MemberType { get; set; }
    }
}
