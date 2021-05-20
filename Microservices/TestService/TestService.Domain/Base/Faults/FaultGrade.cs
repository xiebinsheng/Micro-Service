using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace TestService.Domain.Base.Faults
{
   
    public class FaultGrade : Entity<int>
    {

        #region 属性

        /// <summary>
        /// 报警级别编码
        /// </summary>
        public string FaultGradeNo { get; set; }

        /// <summary>
        /// 报警级别名称
        /// </summary>
        [DisableAuditing]
        public string FaultGradeName { get; set; }

        /// <summary>
        /// 报警级别色调
        /// </summary>
        public int FaultGradeColor { get; set; }

        #endregion

        #region 构造函数

        public FaultGrade()
        {
            // 必须有默认构造函数(FOR ORMS)
        }

        public FaultGrade(string faultGradeNo, string faultGradeName, int faultGradeColor)
        {
            FaultGradeNo = faultGradeNo;
            FaultGradeName = faultGradeName;
            FaultGradeColor = faultGradeColor;
        }

        public FaultGrade(int id, string faultGradeNo, string faultGradeName, int faultGradeColor)
        {
            Id = id;
            FaultGradeNo = faultGradeNo;
            FaultGradeName = faultGradeName;
            FaultGradeColor = faultGradeColor;
        }

        #endregion

        #region 实体行为

        #endregion
    }
}
