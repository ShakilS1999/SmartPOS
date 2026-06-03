using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartPOS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPaidAndDueToSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DueAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Sales",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueAmount",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Sales");
        }
    }
}
