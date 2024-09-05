using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Layer.Migrations
{
    public partial class mig_add_TableArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IsOccupied",
                table: "Tables",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TableAreaId",
                table: "Tables",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TableAreas",
                columns: table => new
                {
                    TableAreaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AreaName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AreaOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableAreas", x => x.TableAreaId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tables_TableAreaId",
                table: "Tables",
                column: "TableAreaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tables_TableAreas_TableAreaId",
                table: "Tables",
                column: "TableAreaId",
                principalTable: "TableAreas",
                principalColumn: "TableAreaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tables_TableAreas_TableAreaId",
                table: "Tables");

            migrationBuilder.DropTable(
                name: "TableAreas");

            migrationBuilder.DropIndex(
                name: "IX_Tables_TableAreaId",
                table: "Tables");

            migrationBuilder.DropColumn(
                name: "TableAreaId",
                table: "Tables");

            migrationBuilder.AlterColumn<bool>(
                name: "IsOccupied",
                table: "Tables",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
