using TestService.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace TestService.Permissions
{
    public class TestServicePermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(TestServicePermissions.GroupName, L("TestService"));

            //Define your own permissions here. Example:
            //myGroup.AddPermission(TestServicePermissions.MyPermission1, L("Permission:MyPermission1"));

            var dictionary = myGroup.AddPermission(TestServicePermissions.FaultGrade.Default, L("DataDictionary"));
            dictionary.AddChild(TestServicePermissions.FaultGrade.Update, L("Edit"));
            dictionary.AddChild(TestServicePermissions.FaultGrade.Delete, L("Delete"));
            dictionary.AddChild(TestServicePermissions.FaultGrade.Create, L("Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<TestServiceResource>(name);
        }
    }
}
