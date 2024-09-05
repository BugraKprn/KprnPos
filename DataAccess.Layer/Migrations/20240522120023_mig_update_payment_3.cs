using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Layer.Migrations
{
    public partial class mig_update_payment_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_PaymentRequests_PaymentRequestId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "PaymentRequests");

            migrationBuilder.DropTable(
                name: "PaymentResponses");

            migrationBuilder.DropIndex(
                name: "IX_Payments_PaymentRequestId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentRequestId",
                table: "Payments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentRequestId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PaymentRequests",
                columns: table => new
                {
                    PaymentRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentRequests", x => x.PaymentRequestId);
                    table.ForeignKey(
                        name: "FK_PaymentRequests_Tables_TableId",
                        column: x => x.TableId,
                        principalTable: "Tables",
                        principalColumn: "TableId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentResponses",
                columns: table => new
                {
                    PaymentResponseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentResponses", x => x.PaymentResponseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentRequestId",
                table: "Payments",
                column: "PaymentRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentRequests_TableId",
                table: "PaymentRequests",
                column: "TableId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_PaymentRequests_PaymentRequestId",
                table: "Payments",
                column: "PaymentRequestId",
                principalTable: "PaymentRequests",
                principalColumn: "PaymentRequestId");
        }
    }
}
