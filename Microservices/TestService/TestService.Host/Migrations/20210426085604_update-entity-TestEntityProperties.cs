using Microsoft.EntityFrameworkCore.Migrations;

namespace TestService.Host.Migrations
{
    public partial class updateentityTestEntityProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "BAS");

            migrationBuilder.RenameTable(
                name: "BASE_FAULT_GRADE",
                newName: "BASE_FAULT_GRADE",
                newSchema: "BAS");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TEST_ENTITY_PROPERTY",
                newName: "NAME");

            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "TEST_ENTITY_PROPERTY",
                newName: "DESC");

            migrationBuilder.RenameColumn(
                name: "FAULT_GRADE",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                newName: "FAULT_GRADE_NO");

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "TEST_ENTITY_PROPERTY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "示例测试编号",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<decimal>(
                name: "PRICE",
                table: "TEST_ENTITY_PROPERTY",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "FAULT_GRADE_NO",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "警报级别编号",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PRICE",
                table: "TEST_ENTITY_PROPERTY");

            migrationBuilder.RenameTable(
                name: "BASE_FAULT_GRADE",
                schema: "BAS",
                newName: "BASE_FAULT_GRADE");

            migrationBuilder.RenameColumn(
                name: "NAME",
                table: "TEST_ENTITY_PROPERTY",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "DESC",
                table: "TEST_ENTITY_PROPERTY",
                newName: "Desc");

            migrationBuilder.RenameColumn(
                name: "FAULT_GRADE_NO",
                table: "BASE_FAULT_GRADE",
                newName: "FAULT_GRADE");

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "TEST_ENTITY_PROPERTY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "示例测试编号");

            migrationBuilder.AlterColumn<string>(
                name: "FAULT_GRADE",
                table: "BASE_FAULT_GRADE",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "警报级别编号");
        }
    }
}
