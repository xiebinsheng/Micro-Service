using AutoMapper;
using TestService.Application.Contracts.Base.Faults.Dtos;
using TestService.Domain.Base.Faults;

namespace TestService
{
    public class TestServiceApplicationAutoMapperProfile : Profile
    {
        public TestServiceApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<FaultGrade, FaultGradeDto>();

            CreateMap<CreateFaultGradeInput, FaultGrade>();
        }
    }
}
