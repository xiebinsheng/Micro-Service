using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace TestService.TestEntities.AccountingManagement
{
    /// <summary>
    /// 账本
    /// </summary>
    public class AccountBook : Entity<long>
    {
        public string BookName { get; set; }

        public string Comments { get; set; }
    }
}
