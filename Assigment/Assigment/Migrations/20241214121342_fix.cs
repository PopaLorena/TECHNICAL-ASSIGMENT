using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Assigment.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemParties_Items_PartyId",
                table: "ItemParties");

            migrationBuilder.CreateIndex(
                name: "IX_ItemParties_ItemId",
                table: "ItemParties",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemParties_Items_ItemId",
                table: "ItemParties",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemParties_Items_ItemId",
                table: "ItemParties");

            migrationBuilder.DropIndex(
                name: "IX_ItemParties_ItemId",
                table: "ItemParties");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemParties_Items_PartyId",
                table: "ItemParties",
                column: "PartyId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
