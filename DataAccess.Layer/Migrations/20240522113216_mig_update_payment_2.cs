using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Layer.Migrations
{
    public partial class mig_update_payment_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TableId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TableId",
                table: "Payments",
                column: "TableId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_TableId",
                table: "PaymentRequests",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentRequests_Tables_TableId",
                table: "PaymentRequests",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "TableId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Tables_TableId",
                table: "Payments",
                column: "TableId",
                principalTable: "Tables",
                principalColumn: "TableId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentRequests_Tables_TableId",
                table: "PaymentRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Tables_TableId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_TableId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_PaymentRequests_TableId",
                table: "PaymentRequests");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "TableId",
                table: "Payments");
        }
    }
}
