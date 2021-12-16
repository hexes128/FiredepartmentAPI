using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class editinforecordwithitemid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "itemid",
                table: "EditinforecordTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "itemid",
                table: "EditinforecordTable");
        }
    }
}
