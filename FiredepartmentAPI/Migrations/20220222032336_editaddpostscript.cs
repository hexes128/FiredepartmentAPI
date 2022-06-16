using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class editaddpostscript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Personinfos");

            migrationBuilder.RenameColumn(
                name: "postscript",
                table: "EditinforecordTable",
                newName: "oldpostscript");

            migrationBuilder.AddColumn<string>(
                name: "newpostscript",
                table: "EditinforecordTable",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "newpostscript",
                table: "EditinforecordTable");

            migrationBuilder.RenameColumn(
                name: "oldpostscript",
                table: "EditinforecordTable",
                newName: "postscript");

            migrationBuilder.CreateTable(
                name: "Personinfos",
                columns: table => new
                {
                    stuid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    grade = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    postscript = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personinfos", x => x.stuid);
                });
        }
    }
}
