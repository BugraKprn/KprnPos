using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Layer.Migrations
{
    public partial class add_cart_tableId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Carts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_TableId",
                table: "Carts",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_Tables_TableId",
                table: "Carts",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "TableId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_Tables_TableId",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_TableId",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Carts");
        }
    }
}
