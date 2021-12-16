using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class editinforecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EditinforecordTable",
                columns: table => new
                {
                    editid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    oldname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    newname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    oldstore = table.Column<int>(type: "int", nullable: false),
                    newstore = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EditinforecordTable", x => x.editid);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EditinforecordTable");
        }
    }
}
