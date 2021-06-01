using Microsoft.EntityFrameworkCore.Migrations;

namespace TestService.Host.Migrations
{
    public partial class addentityBillMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BillMembers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "成员名称"),
                    MemberEName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "成员英文名称"),
                    MemberType = table.Column<int>(type: "int", nullable: false, defaultValue: 1, comment: "成员类型"),
                    Comments = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "备注")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillMembers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BillMembers");
        }
    }
}
