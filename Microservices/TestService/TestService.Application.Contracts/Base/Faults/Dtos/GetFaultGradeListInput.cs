using TestService.Domain.Shared.Consts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace TestService.Application.Contracts.Base.Faults.Dtos
{
    public class GetFaultGradeListInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 报警级别编码
        /// </summary>
        [Required]
        [StringLength(StringPropertyLength.MaxNoOrCodeLength)]
        public string FaultGradeNo { get; set; }

        /// <summary>
        /// 报警级别名称
        /// </summary>
        [Required]
        [StringLength(StringPropertyLength.MaxNameLength)]
        public string FaultGradeName { get; set; }

        /// <summary>
        /// 报警级别色调
        /// </summary>
        public int FaultGradeColor { get; set; }
    }
}
