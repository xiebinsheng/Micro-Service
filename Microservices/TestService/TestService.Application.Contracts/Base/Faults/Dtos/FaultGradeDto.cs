using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace TestService.Application.Contracts.Base.Faults.Dtos
{
    public class FaultGradeDto: AuditedEntityDto<int>
    {
        /// <summary>
        /// 报警级别编码
        /// </summary>
        public string FaultGradeNo { get; set; }

        /// <summary>
        /// 报警级别名称
        /// </summary>
        public string FaultGradeName { get; set; }

        /// <summary>
        /// 报警级别色调
        /// </summary>
        public int FaultGradeColor { get; set; }
    }
}
