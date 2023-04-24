using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSale.Api.Infrastructure.Contexts.Management.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Sale");

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Sale",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", maxLength: 5000, nullable: false),
                    BarCode = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true),
                    PLU = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_PLU",
                schema: "Sale",
                table: "Products",
                column: "PLU",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products",
                schema: "Sale");
        }
    }
}
