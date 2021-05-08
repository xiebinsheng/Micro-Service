namespace TestService.Permissions
{
    public static class TestServicePermissions
    {
        public const string GroupName = "TestService";

        //Add your own permission names. Example:
        //public const string MyPermission1 = GroupName + ".MyPermission1";

        public static class FaultGrade
        {
            public const string Default = GroupName + ".FaultGrade";
            //public const string Get = Default + ".Get";
            //public const string GetList = Default + ".GetList";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
            //public const string ExportExcel = Default + ".ExportExcel";
        }
    }
}