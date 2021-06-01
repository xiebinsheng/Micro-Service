using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;

namespace TestService.TestEntities.AccountingManagement
{
    public class Account : Entity<long>
    {
        public string AccountName { get; set; }

        public decimal AccountAmount { get; set; }

        public string Comments { get; set; }
    }
}
