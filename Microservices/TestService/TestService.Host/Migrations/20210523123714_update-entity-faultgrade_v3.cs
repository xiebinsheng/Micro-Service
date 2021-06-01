using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TestService.Host.Migrations
{
    public partial class updateentityfaultgrade_v3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CREATE_USERID",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UPDATE_TIME",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UPDATE_USERID",
                schema: "BAS",
                table: "BASE_FAULT_GRADE",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CREATE_USERID",
                schema: "BAS",
                table: "BASE_FAULT_GRADE");

            migrationBuilder.DropColumn(
                name: "UPDATE_TIME",
                schema: "BAS",
                table: "BASE_FAULT_GRADE");

            migrationBuilder.DropColumn(
                name: "UPDATE_USERID",
                schema: "BAS",
                table: "BASE_FAULT_GRADE");
        }
    }
}
