using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class itemaddpostscript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "postscript",
                table: "FireitemsTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "postscript",
                table: "FireitemsTable");
        }
    }
}
