using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fiorello.Migrations
{
    public partial class CreateCustomerTableMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_Discount_DiscountId",
                table: "products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Discount",
                table: "Discount");

            migrationBuilder.RenameTable(
                name: "Discount",
                newName: "discounts");

            migrationBuilder.AddPrimaryKey(
                name: "PK_discounts",
                table: "discounts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    SoftDelete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "Id", "Age", "CreatedDate", "FullName", "SoftDelete" },
                values: new object[] { 1, 19, new DateTime(2023, 5, 14, 1, 26, 7, 990, DateTimeKind.Local).AddTicks(3370), "Musa Afandiyev", false });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "Id", "Age", "CreatedDate", "FullName", "SoftDelete" },
                values: new object[] { 2, 19, new DateTime(2023, 5, 14, 1, 26, 7, 990, DateTimeKind.Local).AddTicks(3420), "Murad Jafarov", false });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "Id", "Age", "CreatedDate", "FullName", "SoftDelete" },
                values: new object[] { 3, 6, new DateTime(2023, 5, 14, 1, 26, 7, 990, DateTimeKind.Local).AddTicks(3420), "Resul Hasanov", false });

            migrationBuilder.AddForeignKey(
                name: "FK_products_discounts_DiscountId",
                table: "products",
                column: "DiscountId",
                principalTable: "discounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_discounts_DiscountId",
                table: "products");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_discounts",
                table: "discounts");

            migrationBuilder.RenameTable(
                name: "discounts",
                newName: "Discount");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Discount",
                table: "Discount",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_products_Discount_DiscountId",
                table: "products",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "Id");
        }
    }
}
