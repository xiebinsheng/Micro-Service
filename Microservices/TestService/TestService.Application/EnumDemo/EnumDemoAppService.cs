using MicroService.Extensions.Extensions;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.Application.Contracts.CustomPublicDtos;
using TestService.Extensions;
using TestService.Localization;
using Volo.Abp.Application.Dtos;

namespace TestService.Application.GetEnumListDemo
{
    public class EnumDemoAppService : TestServiceAppService
    {
        private readonly IStringLocalizer<TestServiceResource> _localizer;

        public EnumDemoAppService(IStringLocalizer<TestServiceResource> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// 获取枚举列表
        /// </summary>
        /// <param name="enumClassName"></param>
        /// <returns></returns>
        public virtual async Task<ListResultDto<EnumDto>> GetEnumList(string enumClassName)
        {
            var enumArray = EnumHelper.GetEnumListByCode("TestService.Domain.Shared", "TestService.Enum", enumClassName);

            List<EnumDto> list = new List<EnumDto>();
            await Task.Run(() =>
            {
                
                foreach (System.Enum enumItem in enumArray)
                {
                    list.Add(new EnumDto(enumItem.ToString(), Convert.ToInt32(enumItem), _localizer[enumItem.GetDescription()]));
                }
            });

            return new ListResultDto<EnumDto>(list);
        }
    }
}
