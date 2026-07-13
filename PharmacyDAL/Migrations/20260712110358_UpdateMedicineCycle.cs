using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PharmacyDAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMedicineCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicines_MedicineId",
                table: "MedicineBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUnits_Medicines_MedicineId",
                table: "MedicineUnits");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "OrderItems",
                newName: "PurchasePrice");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "int",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "MedicineBatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ManufactureDate",
                table: "MedicineBatches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SupplierId",
                table: "MedicineBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MedicineBatches_SupplierId",
                table: "MedicineBatches",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicines_MedicineId",
                table: "MedicineBatches",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Suppliers_SupplierId",
                table: "MedicineBatches",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUnits_Medicines_MedicineId",
                table: "MedicineUnits",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Medicines_MedicineId",
                table: "MedicineBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineBatches_Suppliers_SupplierId",
                table: "MedicineBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicineUnits_Medicines_MedicineId",
                table: "MedicineUnits");

            migrationBuilder.DropIndex(
                name: "IX_MedicineBatches_SupplierId",
                table: "MedicineBatches");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "MedicineBatches");

            migrationBuilder.DropColumn(
                name: "ManufactureDate",
                table: "MedicineBatches");

            migrationBuilder.DropColumn(
                name: "SupplierId",
                table: "MedicineBatches");

            migrationBuilder.RenameColumn(
                name: "PurchasePrice",
                table: "OrderItems",
                newName: "UnitPrice");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 30);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineBatches_Medicines_MedicineId",
                table: "MedicineBatches",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicineUnits_Medicines_MedicineId",
                table: "MedicineUnits",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
