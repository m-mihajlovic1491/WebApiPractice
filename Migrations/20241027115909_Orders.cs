using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiPractice.Migrations
{
    /// <inheritdoc />
    public partial class Orders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Orderid",
                table: "Product",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    Reference1 = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_Orderid",
                table: "Product",
                column: "Orderid");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Order_Orderid",
                table: "Product",
                column: "Orderid",
                principalTable: "Order",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Order_Orderid",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Product_Orderid",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Orderid",
                table: "Product");
        }
    }
}
