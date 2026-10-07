using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartShoppingAssistant.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddProductStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StockQuantity",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Give existing products some stock: mostly plenty, a few almost sold out or sold out
            migrationBuilder.Sql(@"
                UPDATE Products SET StockQuantity =
                    CASE
                        WHEN Id % 13 = 0 THEN 0
                        WHEN Id % 7 = 0 THEN 3
                        ELSE 10 + (Id * 7) % 50
                    END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StockQuantity",
                table: "Products");
        }
    }
}
