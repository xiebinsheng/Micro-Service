using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace TestService.Domain.EFTest.FluntApi
{
    public class TestEntityProperties : AuditedAggregateRoot<int>
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Desc { get; set; }

        public decimal Price { get; set; }

        public DateTime RecordTime { get; set; }
    }
}
