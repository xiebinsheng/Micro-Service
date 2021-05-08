using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestService.Host.Migrations
{
    public partial class updateentityTestEntityPropertiesdefault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "TEST_ENTITY_PROPERTY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "test",
                comment: "示例测试编号",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "示例测试编号");

            migrationBuilder.AddColumn<decimal>(
                name: "PRICE",
                table: "TEST_ENTITY_PROPERTY",
                type: "decimal(14,2)",
                precision: 14,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "RecordTime",
                table: "TEST_ENTITY_PROPERTY",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getdate()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PRICE",
                table: "TEST_ENTITY_PROPERTY");

            migrationBuilder.DropColumn(
                name: "RecordTime",
                table: "TEST_ENTITY_PROPERTY");

            migrationBuilder.AlterColumn<string>(
                name: "CODE",
                table: "TEST_ENTITY_PROPERTY",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "示例测试编号",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "test",
                oldComment: "示例测试编号");
        }
    }
}
