using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestService.Host.Migrations
{
    public partial class updateentityFaultGradev3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationTime",
                schema: "BAS",
                table: "BASE_FAULT_GRADE");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                schema: "BAS",
                table: "BASE_FAULT_GRADE");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                schema: "BAS",
                table: "BASE_FAULT_GRADE");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                schema: "BAS",
                table: "BASE_FAULT_GRADE");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
