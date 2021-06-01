using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TestService.Application.Contracts.Base.Faults.Dtos
{
    /// <summary>
    /// 【数据传输对象】创建或更新报警级别
    /// </summary>
    public class CreateFaultGradeInput
    {
        /// <summary>
        /// 报警级别编码
        /// </summary>
        [Required]
        [StringLength(5)]
        public string FaultGradeNo { get; set; }

        /// <summary>
        /// 报警级别名称
        /// </summary>
        [Required]
        [StringLength(5)]
        public string FaultGradeName { get; set; }

        /// <summary>
        /// 报警级别色调
        /// </summary>
        public int FaultGradeColor { get; set; } 
    }
}
