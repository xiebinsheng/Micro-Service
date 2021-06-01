using System;
using System.Collections.Generic;
using System.Text;

namespace TestService.Attributes
{
    public class EnumLocalization : Attribute
    {
        /// <summary>
        /// 多语言Code
        /// </summary>
        public string Code { get; set; }

        public EnumLocalization(string code)
        {
            Code = code;
        }
        
    }
}
