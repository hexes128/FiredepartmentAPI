using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class rebuildconnection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "StatusChangeTable",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
