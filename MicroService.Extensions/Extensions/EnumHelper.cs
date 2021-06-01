using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MicroService.Extensions.Extensions
{
    /// <summary>
    /// 枚举帮助类
    /// </summary>
    public static class EnumHelper
    {
        private static ConcurrentDictionary<string, Assembly> AssemblyList = new ConcurrentDictionary<string, Assembly>();

        /// <summary>
        /// 根据枚举代码获取枚举列表
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="namespace">命名空间名称</param>
        /// <param name="enumClassName">枚举名称</param>
        /// <returns></returns>
        public static Array GetEnumListByCode(string assemblyName, string @namespace, string enumClassName)
        {
            if (!(AssemblyList?.Select(o => o.Key)).Contains(assemblyName))
            {
                AssemblyList.TryAdd(assemblyName, Assembly.Load(assemblyName));
            }

            List<Type> enumlist = new List<Type>();

            foreach (var assembly in AssemblyList)
            {
                var enumInfo = assembly.Value.CreateInstance($"{@namespace}.{enumClassName}", false);
                if (enumInfo != null)
                {
                    enumlist.Add(enumInfo.GetType());
                }
            }

            if (enumlist.Count == 0) return null;
            if (enumlist.Count > 1) throw new EnumException($"枚举【{enumClassName}】存在多个，请检查命名空间【{ @namespace}】");

            return Enum.GetValues(enumlist.FirstOrDefault());
        }

        /// <summary>
        /// 根据枚举代码获取枚举列表
        /// </summary>
        /// <param name="assembly">程序集名称集合</param>
        /// <param name="namespaces">命名空间名称集合</param>
        /// <param name="code">枚举名称</param>
        /// <returns></returns>
        public static Array GetEnumListByCode(IEnumerable<string> assemblys, IEnumerable<string> namespaces, string EnumCode)
        {
            assemblys = assemblys.Distinct();
            namespaces = namespaces.Distinct();
            var Inexistentassembly = assemblys.Except(AssemblyList?.Select(o => o.Key));
            foreach (var assembly in Inexistentassembly)
                AssemblyList.TryAdd(assembly, Assembly.Load(assembly));
            List<Type> enumlist = new List<Type>();
            List<string> enumNamespanList = new List<string>();
            foreach (var enumNamespace in namespaces)
            {
                foreach (var assembly in AssemblyList)
                {
                    var enumInfo = assembly.Value.CreateInstance($"{enumNamespace}.{EnumCode}", false);
                    if (enumInfo != null)
                    {
                        enumNamespanList.Add(enumNamespace);
                        enumlist.Add(enumInfo.GetType());
                    }
                }
            }
            if (enumlist.Count == 0) return default;
            if (enumlist.Count > 1)
                throw new EnumException($"枚举【{EnumCode}】存在多个，请检查命名空间【{string.Join(',', enumNamespanList)}】");
            var enums = Enum.GetValues(enumlist.FirstOrDefault());

            return enums;
            //List<EnumDto> enumDic = new List<EnumDto>();
            //foreach (Enum item in enums)
            //{
            //    enumDic.Add(new EnumDto() { Code = item.ToString(), Value = Convert.ToInt32(item), Description = item.ToDescription() });
            //}
            //return enumDic;
        }
    }
}
