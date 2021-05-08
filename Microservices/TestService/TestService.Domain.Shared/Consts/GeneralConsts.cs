using System;
using System.Collections.Generic;
using System.Text;

namespace TestService.Domain.Shared.Consts
{
    /// <summary>
    /// TestService公共常量
    /// </summary>
    public class GeneralConsts
    {
       
    }

    /// <summary>
    /// 字符串属性长度常量
    /// </summary>
    public class StringPropertyLength
    {
        public const int MaxNoOrCodeLength = 50;

        public const int MaxNameLength = 100;

        public const int MaxDescLength = 200;
    }

    /// <summary>
    /// 表名前缀常量
    /// </summary>
    public class DbTablePrefix
    {
        public const string Base = "BASE_";

        public const string Test = "TEST_";
    }
}
