using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestService.Application.Contracts.Base.Faults;
using TestService.Application.Contracts.Base.Faults.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;

namespace BaseService.Systems.CustomRemoteMangement
{
    public class CustomAppService : ApplicationService, ICustomAppService
    {
        private readonly IFaultGradeAppService _faultGradeRemoteAppService;

        public CustomAppService(IFaultGradeAppService faultGradeRemoteAppService)
        {
            _faultGradeRemoteAppService = faultGradeRemoteAppService;
        }

        public virtual async Task<FaultGradeDto> CreateFaultGradeAsync(CreateFaultGradeInput input)
        {

            var result = await _faultGradeRemoteAppService.CreateAsync(input);
            return result;
            //return null;
        }
    }
}
