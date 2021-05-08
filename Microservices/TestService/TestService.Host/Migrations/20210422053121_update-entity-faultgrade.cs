using Microsoft.EntityFrameworkCore.Migrations;

namespace TestService.Host.Migrations
{
    public partial class updateentityfaultgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BASE.FAULT_GRADE",
                table: "BASE.FAULT_GRADE");

            migrationBuilder.RenameTable(
                name: "BASE.FAULT_GRADE",
                newName: "BASE_FAULT_GRADE");

            migrationBuilder.RenameColumn(
                name: "FaultGradeNo",
                table: "BASE_FAULT_GRADE",
                newName: "FAULT_GRADE");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BASE_FAULT_GRADE",
                table: "BASE_FAULT_GRADE",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BASE_FAULT_GRADE",
                table: "BASE_FAULT_GRADE");

            migrationBuilder.RenameTable(
                name: "BASE_FAULT_GRADE",
                newName: "BASE.FAULT_GRADE");

            migrationBuilder.RenameColumn(
                name: "FAULT_GRADE",
                table: "BASE.FAULT_GRADE",
                newName: "FaultGradeNo");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BASE.FAULT_GRADE",
                table: "BASE.FAULT_GRADE",
                column: "Id");
        }
    }
}
