using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FiredepartmentAPI.Migrations
{
    public partial class initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlaceTable",
                columns: table => new
                {
                    PlaceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    todaysend = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceTable", x => x.PlaceId);
                });

            migrationBuilder.CreateTable(
                name: "InventoryEventTable",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    InventoryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryEventTable", x => x.EventId);
                    table.ForeignKey(
                        name: "FK_InventoryEventTable_PlaceTable_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "PlaceTable",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PriorityTable",
                columns: table => new
                {
                    StoreId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    SubArea = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriorityNum = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriorityTable", x => x.StoreId);
                    table.ForeignKey(
                        name: "FK_PriorityTable_PlaceTable_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "PlaceTable",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FireitemsTable",
                columns: table => new
                {
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PresentStatus = table.Column<int>(type: "int", nullable: false),
                    InventoryStatus = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FireitemsTable", x => x.ItemId);
                    table.ForeignKey(
                        name: "FK_FireitemsTable_PriorityTable_StoreId",
                        column: x => x.StoreId,
                        principalTable: "PriorityTable",
                        principalColumn: "StoreId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemsTable",
                columns: table => new
                {
                    InventoryItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StatusBefore = table.Column<int>(type: "int", nullable: false),
                    StatusAfter = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemsTable", x => x.InventoryItemId);
                    table.ForeignKey(
                        name: "FK_InventoryItemsTable_FireitemsTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "FireitemsTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryItemsTable_InventoryEventTable_EventId",
                        column: x => x.EventId,
                        principalTable: "InventoryEventTable",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StatusChangeTable",
                columns: table => new
                {
                    StatusChangId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlaceId = table.Column<int>(type: "int", nullable: false),
                    ItemId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StasusCode = table.Column<int>(type: "int", nullable: false),
                    Postscript = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChangeDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusChangeTable", x => x.StatusChangId);
                    table.ForeignKey(
                        name: "FK_StatusChangeTable_FireitemsTable_ItemId",
                        column: x => x.ItemId,
                        principalTable: "FireitemsTable",
                        principalColumn: "ItemId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StatusChangeTable_PlaceTable_PlaceId",
                        column: x => x.PlaceId,
                        principalTable: "PlaceTable",
                        principalColumn: "PlaceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FireitemsTable_StoreId",
                table: "FireitemsTable",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryEventTable_PlaceId",
                table: "InventoryEventTable",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemsTable_EventId",
                table: "InventoryItemsTable",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemsTable_ItemId",
                table: "InventoryItemsTable",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityTable_PlaceId",
                table: "PriorityTable",
                column: "PlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeTable_ItemId",
                table: "StatusChangeTable",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StatusChangeTable_PlaceId",
                table: "StatusChangeTable",
                column: "PlaceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItemsTable");

            migrationBuilder.DropTable(
                name: "StatusChangeTable");

            migrationBuilder.DropTable(
                name: "InventoryEventTable");

            migrationBuilder.DropTable(
                name: "FireitemsTable");

            migrationBuilder.DropTable(
                name: "PriorityTable");

            migrationBuilder.DropTable(
                name: "PlaceTable");
        }
    }
}
