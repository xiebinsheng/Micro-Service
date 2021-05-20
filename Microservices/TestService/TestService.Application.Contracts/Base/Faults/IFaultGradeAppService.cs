using TestService.Application.Contracts.Base.Faults.Dtos;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace TestService.Application.Contracts.Base.Faults
{
    public interface IFaultGradeAppService: IApplicationService
    {
        Task<FaultGradeDto> CreateAsync(CreateFaultGradeInput input);
    }
}