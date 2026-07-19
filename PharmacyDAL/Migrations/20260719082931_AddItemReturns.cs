using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddItemReturns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReturnedQuantity",
                table: "SaleItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReturnedQuantity",
                table: "OrderItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReturnedQuantity",
                table: "SaleItems");

            migrationBuilder.DropColumn(
                name: "ReturnedQuantity",
                table: "OrderItems");
        }
    }
}
