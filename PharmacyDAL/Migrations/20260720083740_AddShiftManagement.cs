using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDAL.Migrations
{
    /// <inheritdoc />
    public partial class AddShiftManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OpeningCash = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SalesTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ReturnsTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ExpectedCash = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ActualCash = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CashDifference = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shifts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_ApplicationUserId",
                table: "Shifts",
                column: "ApplicationUserId",
                unique: true,
                filter: "[ClosedAt] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shifts");
        }
    }
}
