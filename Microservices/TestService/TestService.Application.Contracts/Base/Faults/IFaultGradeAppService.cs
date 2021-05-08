using TestService.Application.Contracts.Base.Faults.Dtos;
using System.Threading.Tasks;

namespace TestService.Application.Contracts.Base.Faults
{
    public interface IFaultGradeAppService
    {
        Task<FaultGradeDto> CreateAsync(CreateFaultGradeInput input);
    }
}