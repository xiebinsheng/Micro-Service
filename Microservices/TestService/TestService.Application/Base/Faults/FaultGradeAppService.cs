using TestService.Application.Contracts.Base.Faults;
using TestService.Application.Contracts.Base.Faults.Dtos;
using TestService.Domain.Base.Faults;
using TestService.Localization;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using Volo.Abp.Users;
using System;
using Volo.Abp.Localization;
using System.Globalization;
//using Abp.Web.Models;

namespace TestService.Application.Base.Faults
{
    //[WrapResult]
    public class FaultGradeAppService : TestServiceAppService, IFaultGradeAppService
    {
        private readonly IRepository<FaultGrade, int> _faultGradeRepository;
        private readonly IStringLocalizer<TestServiceResource> _localizer;
        //private readonly IIdentityUserLookupAppService _identityUserLookupAppService;
        private readonly ICurrentUser _currentUser;
        private readonly ILanguageProvider _languageProvider;
        //private readonly ILanguageInfo _languageInfo;

        public FaultGradeAppService(
            IRepository<FaultGrade, int> faultGradeRepository,
            IStringLocalizer<TestServiceResource> localizer,
            //ILanguageInfo languageInfo,
            ILanguageProvider languageProvider,
            //IIdentityUserLookupAppService identityUserLookupAppService,
            ICurrentUser currentUser)
        {
            _faultGradeRepository = faultGradeRepository;
            _localizer = localizer;
            //_languageInfo = languageInfo;
            _languageProvider = languageProvider;
            //_identityUserLookupAppService = identityUserLookupAppService;
            _currentUser = currentUser;
        }

        /// <summary>
        /// 创建报警级别
        /// </summary> 
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>Create by 2</remarks>
        //[Authorize(TestServicePermissions.FaultGrade.Create)]
        public virtual async Task<FaultGradeDto> CreateAsync(CreateFaultGradeInput input)
        {
            var ccc = _currentUser.Name;
            var languages = await _languageProvider.GetLanguagesAsync();

            var currentLanguage = languages.FindByCulture(
                CultureInfo.CurrentCulture.Name,
                CultureInfo.CurrentUICulture.Name
            );

            //var ddd =await _identityUserLookupAppService.FindByIdAsync(new Guid("B3A4DE5A-0338-F82F-C71F-39FC33D8558A"));

            var existFaultGrade = await _faultGradeRepository.FirstOrDefaultAsync(p => p.FaultGradeNo == input.FaultGradeNo);
            if (existFaultGrade != null)
            {

                var aaa = _localizer["HelloWorld"];
                var bbb = _localizer["FaultNoAlreadyExistsException", input.FaultGradeNo];
                //throw new BusinessException("P000001", _localizer["FaultNoAlreadyExistsException", input.FaultGradeNo]);
                //throw new BusinessException("P00001", $"报警级别编号【{input.FaultGradeNo}】已存在");
                throw new UserFriendlyException(_localizer["FaultNoAlreadyExistsException", input.FaultGradeNo]);
            }

            //var faultGrade = new FaultGrade(
            //    input.FaultGradeNo,
            //    input.FaultGradeName,
            //    input.FaultGradeColor);

            //var result = await _faultGradeRepository.InsertAsync(faultGrade);
            //return ObjectMapper.Map<FaultGrade, FaultGradeDto>(result);


            var entity = ObjectMapper.Map<CreateFaultGradeInput, FaultGrade>(input);
            var result = await _faultGradeRepository.InsertAsync(entity);

            return ObjectMapper.Map<FaultGrade, FaultGradeDto>(result);
        }
    }
}
