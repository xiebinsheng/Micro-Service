using TestService.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace TestService.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class TestServiceController : AbpController
    {
        protected TestServiceController()
        {
            LocalizationResource = typeof(TestServiceResource);
        }
    }
}