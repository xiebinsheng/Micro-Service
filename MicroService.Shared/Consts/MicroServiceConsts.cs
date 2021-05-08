using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Shared.Consts
{
    public class MicroServiceConsts
    {
        /// <summary>
        /// 是否启用多租户(Default:false)
        /// </summary>
        public const bool IsMultiTenancyEnabled = false;

        /// <summary>
        /// Redis中存储的Service的Key的前缀
        /// </summary>
        public const string RedisKeyServicePrefix = "Ms-DataProtection-";
    }
}
