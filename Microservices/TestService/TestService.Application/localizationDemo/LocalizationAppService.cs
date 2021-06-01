using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TestService.Extensions;
using TestService.Localization;
using TestService.TestEntities.BillManagement;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;

namespace TestService.Application.localizationTest
{
    /// <summary>
    /// 多语言简单使用
    /// </summary>
    public class LocalizationAppService : TestServiceAppService
    {
        private readonly IRepository<BillMembers, long> _repository;
        private readonly IStringLocalizer<TestServiceResource> _localizer;

        public LocalizationAppService(IRepository<BillMembers, long> localizationRepository, IStringLocalizer<TestServiceResource> localizer)
        {
            _repository = localizationRepository;
            _localizer = localizer;
        }

        /// <summary>
        /// 返回多语言提示消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<BillMemberDto> CreateAsync(CreateBillMemberInput input)
        {
            var existMember = await _repository.FirstOrDefaultAsync(p => p.MemberName == input.MemberName);
            if (existMember != null)
            {
                throw new UserFriendlyException(_localizer["Exception:MemberNameAlreadyExists", input.MemberName]);
            }

            var entity = ObjectMapper.Map<CreateBillMemberInput, BillMembers>(input);
            var result = await _repository.InsertAsync(entity);

            return ObjectMapper.Map<BillMembers, BillMemberDto>(result);
        }

        public virtual async Task<PagedResultDto<GetBillMemberListDto>> GetListAsync(GeBillMemberListInput input)
        {
            var query1 = await _repository.GetListAsync();

            var query = _repository
                .WhereIf(!string.IsNullOrWhiteSpace(input.MemberName), t => t.MemberName == input.MemberName);


            var totalCount = await query.CountAsync();

            var queryResult = await query.PageBy(input).ToListAsync();

            var dtos = queryResult.Select(t =>
            {
                var dto = ObjectMapper.Map<BillMembers, GetBillMemberListDto>(t);
                dto.MemberTypeDesc = _localizer[dto.MemberType.GetDescription()];
                return dto;
            }).ToList();

            return new PagedResultDto<GetBillMemberListDto>(totalCount, dtos);
        }
    }
}
