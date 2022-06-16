using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class addpostscript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Itempostscript",
                table: "FireitemsTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Itempostscript",
                table: "FireitemsTable");
        }
    }
}
