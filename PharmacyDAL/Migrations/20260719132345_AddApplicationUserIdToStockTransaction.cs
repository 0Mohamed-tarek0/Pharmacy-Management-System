using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationUserIdToStockTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "StockTransactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ApplicationUserId",
                table: "StockTransactions",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactions_AspNetUsers_ApplicationUserId",
                table: "StockTransactions",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactions_AspNetUsers_ApplicationUserId",
                table: "StockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_StockTransactions_ApplicationUserId",
                table: "StockTransactions");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "StockTransactions");
        }
    }
}
