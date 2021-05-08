using Volo.Abp.Settings;

namespace TestService.Settings
{
    public class TestServiceSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(TestServiceSettings.MySetting1));
        }
    }
}
