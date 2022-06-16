using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class deleteref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusChangeTable_PlaceTable_PlaceId",
                table: "StatusChangeTable");

            migrationBuilder.DropIndex(
                name: "IX_StatusChangeTable_PlaceId",
                table: "StatusChangeTable");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "StatusChangeTable");

            migrationBuilder.DropColumn(
                name: "Postscript",
                table: "StatusChangeTable");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "StatusChangeTable",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "InventoryEventTable",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EditinforecordTable",
                newName: "UserName");

            migrationBuilder.AddColumn<int>(
                name: "PlaceModelPlaceId",
                table: "StatusChangeTable",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeTable_PlaceModelPlaceId",
                table: "StatusChangeTable",
                column: "PlaceModelPlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusChangeTable_PlaceTable_PlaceModelPlaceId",
                table: "StatusChangeTable",
                column: "PlaceModelPlaceId",
                principalTable: "PlaceTable",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StatusChangeTable_PlaceTable_PlaceModelPlaceId",
                table: "StatusChangeTable");

            migrationBuilder.DropIndex(
                name: "IX_StatusChangeTable_PlaceModelPlaceId",
                table: "StatusChangeTable");

            migrationBuilder.DropColumn(
                name: "PlaceModelPlaceId",
                table: "StatusChangeTable");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "StatusChangeTable",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "InventoryEventTable",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "EditinforecordTable",
                newName: "UserId");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "StatusChangeTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Postscript",
                table: "StatusChangeTable",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeTable_PlaceId",
                table: "StatusChangeTable",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_StatusChangeTable_PlaceTable_PlaceId",
                table: "StatusChangeTable",
                column: "PlaceId",
                principalTable: "PlaceTable",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
