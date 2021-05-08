using System;
using System.Collections.Generic;
using System.Text;
using TestService.Localization;
using Volo.Abp.Application.Services;

namespace TestService
{
    /* Inherit your application services from this class.
     */
    public abstract class TestServiceAppService : ApplicationService
    {
        protected TestServiceAppService()
        {
            LocalizationResource = typeof(TestServiceResource);
        }
    }
}
