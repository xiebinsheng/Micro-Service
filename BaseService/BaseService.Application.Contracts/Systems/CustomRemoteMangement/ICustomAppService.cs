using System.Threading.Tasks;
using TestService.Application.Contracts.Base.Faults.Dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace BaseService.Systems.CustomRemoteMangement
{
    public interface ICustomAppService: IApplicationService
    {
        Task<FaultGradeDto> CreateFaultGradeAsync(CreateFaultGradeInput input);
    }
}